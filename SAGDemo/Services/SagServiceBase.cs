using Microsoft.Data.SqlClient;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using SAGDemo.Models;

namespace SAGDemo.Services;

public abstract class SagServiceBase
{
    private readonly OpenAIAPI _openAi;
    private readonly string _connectionString;

    public SagServiceBase(IConfiguration configuration)
    {
        _openAi = new OpenAIAPI(configuration["OpenAiToken"]);
        _connectionString = configuration.GetConnectionString("default");
    }

    protected async Task<SagResult> QuestionDb(string question, string tables)
    {
        var sql = await GenerateSQL(question, tables);

        if (!HasGeneratedSQL(sql))
        {
            return new SagResult
            {
                Error = sql
            };
        }

        var data = RetrieveData(sql);
        var response = await GenerateResponse(question, data);

        return new SagResult
        {
            Answer = response,
            GeneratedSQL = sql
        };
    }

    private bool HasGeneratedSQL(string sql)
    {
        return sql.Contains("SELECT", StringComparison.InvariantCulture);
    }

    private async Task<string> GenerateSQL(string question, string tables)
    {
        var system = @"You are an expert SQL server engineer. The user will ask you a question about a table in your database and will provide the schema.

The output must only contain the SQL query that will retrieve the requested information from the database.
If you are unable to generate the query with the provided information, respond with ""I'm unable to answer this question"".

The database is an SQL Server, so always use T-SQL.

Only create SELECT queries. Never update or delete any data or drop any tables.";

// Refine this. ChatGPT tends to use "Limit 10" for this, which is mySQL.
// The output could contain a big number of rows, so made sure you only retrieve the first 10 rows.

                var prompt = @$"Assume the database has a table that is defined as follows:
{tables}

Generate a well-formed SQL query from the following prompt:
PROMPT: {question}
";

        var response = await _openAi.Chat.CreateChatCompletionAsync(
            model: Model.ChatGPTTurbo,
            messages: new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, system),
                new ChatMessage(ChatMessageRole.User, prompt)
            });

        return response.Choices[0].Message.Content;
    }

    private string RetrieveData(string sql)
    {
        using var con = new SqlConnection(_connectionString);
        con.Open();

        using SqlCommand cmd = new SqlCommand(sql, con);

        SqlDataReader reader = cmd.ExecuteReader();

        // Create header row
        var output = reader.GetColumnSchema()
            .Select(schema => schema.ColumnName)
            .Aggregate((agg, next) => $"{agg},{next}");

        // Retrieve query results
        while (reader.Read())
        {
            output += "\n";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                output += reader.GetValue(i).ToString();
                if (i + 1 < reader.FieldCount)
                    output += ",";
            }
        }

        return output;
    }

    private async Task<string> GenerateResponse(string question, string data)
    {
        var system = @"You are a helpfull assistant. You are positive and don't use rude language towards the user.

The user will pose a question and give you some data corresponding with the question. Use only this data to formulate an answer.
If you are unable to give an answer with the provided data, tell the user ""I was unable to find an answer to your question.""";

        var prompt = $@"Given the following question and query result, formulate an answer.
QUESTION: {question}
RESULT: {data}";

        var response = await _openAi.Chat.CreateChatCompletionAsync(
            model: Model.ChatGPTTurbo,
            messages: new List<ChatMessage>()
            {
                new ChatMessage(ChatMessageRole.System, system),
                new ChatMessage(ChatMessageRole.User, prompt)
            });

        return response.Choices[0].Message.Content;
    }
}
