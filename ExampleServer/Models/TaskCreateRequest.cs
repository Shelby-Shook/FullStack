// Define what the request body for creating tasks should look like
// When the client sends information to the server, this is the object

using System.Text.Json.Serialization;

namespace ExampleServer.Models;

public class TaskCreateRequest
{
    // JsonPropertyName is an attribute
    // Defines what the JSON equivalent is for our property
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]

    public string? Description { get; set; }
}