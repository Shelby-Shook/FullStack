using System.Text.Json.Serialization;

namespace ExampleServer.Models;

public class CompleteTaskRequest
{
    [JsonPropertyName("taskId")]
    public int TaskId { get; set; }
}