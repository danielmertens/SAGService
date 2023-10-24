using SAGDemo.Models;

namespace SAGDemo.Services;

public interface IAlbumSagService
{
    /// <summary>
    /// Ask a question about the album table. 
    /// This is a simple table that should be easy to query.
    /// I expect Chat-GPT to handle this very well.
    /// </summary>
    /// <param name="question">A question about albums.</param>
    /// <returns>The answer to the question.</returns>
    Task<SagResult> AlbumQuestion(string question);
}

public class AlbumSagService : SagServiceBase, IAlbumSagService
{
    public AlbumSagService(IConfiguration configuration)
        : base(configuration)
    { }

    public async Task<SagResult> AlbumQuestion(string question)
    {
        var table = @"CREATE TABLE MusicAlbums (
id INT IDENTITY(1,1) PRIMARY KEY,
name VARCHAR(255) NOT NULL,
artist VARCHAR(255) NOT NULL,
numOfSongs INT,
releaseYear INT
);";

        return await QuestionDb(question, table);
    }
}
