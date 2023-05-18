using System.Text.Json.Serialization;

namespace ExampleServer.Models;

public class ErrorResponse
{
    public ErrorResponse(string message)
    {
        Message = message;
    }

    [JsonPropertyName("message")]
    public string Message { get; }
}