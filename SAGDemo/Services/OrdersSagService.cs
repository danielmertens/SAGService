using SAGDemo.Models;

namespace SAGDemo.Services;

public interface IOrderSagService
{
    /// <summary>
    /// Ask a question about orders.
    /// This is more complex. There are four tables corralated to each other.
    /// Data may need to be joined together to give a correct answer.
    /// This is harder for Chat-GPT.
    /// This still needs some fine-tuning.
    /// </summary>
    /// <param name="question">A question about orders.</param>
    /// <returns>The answer to the question.</returns>
    Task<SagResult> OrderQuestion(string question);
}

public class OrdersSagService : SagServiceBase, IOrderSagService
{
    public OrdersSagService(IConfiguration configuration)
        : base(configuration)
    { }

    public async Task<SagResult> OrderQuestion(string question)
    {
        var tables = @"CREATE TABLE Users (
UserID INT PRIMARY KEY,
Username VARCHAR(50) NOT NULL,
Email VARCHAR(100) NOT NULL,
Address VARCHAR(255)
);

CREATE TABLE Orders (
OrderID INT PRIMARY KEY,
UserID INT,
OrderDate DATE,
FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Products (
ProductID INT PRIMARY KEY,
ProductName VARCHAR(100) NOT NULL,
Price DECIMAL(10, 2) NOT NULL,
Description TEXT
);

CREATE TABLE OrderDetails (
OrderID INT,
ProductID INT,
PRIMARY KEY (OrderID, ProductID),
FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);";

        return await QuestionDb(question, tables);
    }
}
