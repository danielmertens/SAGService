namespace SAGDemo.Models;

public class SagResult
{
    public string? Answer { get; set; } = null;
    public string? GeneratedSQL { get; set; } = null;
    public string? Error { get; set; } = null;
}
