//Define our actual class that serves and handles web requests
// Listen to HTTP requests, handle back and forth with out client
//Client being our web page (HTML, CSS, JS)

using System.Net;
using System.Text;
using System.Text.Json;
using ExampleServer.Data;
using ExampleServer.Models;

namespace ExampleServer.Server;

public class WebServer
{
    private readonly TaskRepository _taskRepository;
    private readonly HttpListener _httpListener = new();

    //Constructor
    public WebServer(TaskRepository repository, string url)
    {
        //Dependency Injection (injecting something we're dependent on)
        // Passing the TaskRepository into our class
        // rather than making a new repository

        _taskRepository = repository;

        _httpListener.Prefixes.Add(url);

    }

    public void Run()
    {
        //start the server (http listener)
        _httpListener.Start();

        // Add somedebug feedback (console writeLine)
        Console.WriteLine($"Listening for connections on {_httpListener.Prefixes.First()}");

        //Handle our incoming connections/requests
        //Bulk of our logic
        HandleIncomingRequests();

        //Stop the server
        _httpListener.Stop();
    }

    private void HandleIncomingRequests()
    {
        while (true)
        {
            HttpListenerContext context = _httpListener.GetContext();

            // Get the Request and Response objects from the context
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            switch (request.HttpMethod)
            {
                case "GET":
                    HandleGetRequests(request, response);
                    break;
                case "POST":
                    //Handle POST requests
                    HandlePostRequests(request, response);
                    break;
                case "PUT":
                    HandlePutRequests(request, response);
                    break;
                case "OPTIONS":
                    HandleOptionsRequests(response);
                    break;
                default:
                    SendResponse(response, HttpStatusCode.NotFound, null);
                    break;
            }


        }

    }

    private void HandleGetRequests(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url?.AbsolutePath == "/")
        {
            var tasks = _taskRepository.GetTasks();
            SendResponse(response, HttpStatusCode.OK, tasks);
        }
        else
        {
            SendResponse(response, HttpStatusCode.NotFound, null);
        }
    }

    private void HandlePostRequests(HttpListenerRequest request, HttpListenerResponse response)
    {
        // check that the request has a body
        if (request.HasEntityBody)
        {
            // Deserialize our request body into the C# request type
            TaskCreateRequest? body = JsonSerializer.Deserialize<TaskCreateRequest>(request.InputStream);

            // Check to make sure it is not null
            if (body != null)
            {
                //Create the new TaskModel
                TaskModel newTask = new TaskModel(body.Title ?? "Title", body.Description ?? "");

                //Add that task to our repository
                _taskRepository.AddTask(newTask);

                //Create a response message
                string logOutput = $"Added new item: #{newTask.Id}: {newTask.Title}";
                Console.WriteLine(logOutput);

                //Send that response 
                SendResponse(response, HttpStatusCode.Created, newTask);
            }
        }
        else
        {
            // if our POST request does not have a body
            string errorMessage = "Failed to add task as there was no request body.";
            Console.WriteLine(errorMessage);

            ErrorResponse error = new ErrorResponse(errorMessage);
            SendResponse(response, HttpStatusCode.BadRequest, error);
        }

    }

    //Handle Update Requests
    private void HandlePutRequests(HttpListenerRequest req, HttpListenerResponse res)
    {
        if (req.HasEntityBody)
        {
            CompleteTaskRequest? body = JsonSerializer.Deserialize<CompleteTaskRequest>(req.InputStream);
            
           
              bool result =  _taskRepository.MarkTaskAsComplete(body?.TaskId ?? -1);

              HttpStatusCode code = result ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
              SendResponse(res, code, null);
           

        }
        else
        {
            string errorMessage = "Could not update task.";
            Console.WriteLine(errorMessage);
            ErrorResponse error = new ErrorResponse(errorMessage);
            SendResponse(res, HttpStatusCode.BadRequest, error);
        }
    }

    private void HandleOptionsRequests(HttpListenerResponse res)
    {
        res.AddHeader("Access-Control-Allow-Methods", "*");
        SendResponse(res, HttpStatusCode.OK, null);
    }


    private void SendResponse(HttpListenerResponse response, HttpStatusCode statusCode, object? data)
    {
        //Convert our C# object to JSON, which allows our browser to understand it
        // We need to also tell our response the content is JSON
        string json = JsonSerializer.Serialize(data);
        response.ContentType = "Application/json";

        // Convert our JSON to a byte[] -> basic numbers we can send over the internet
        // Breaking down JSON to a stream of numbers
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        //We need to tell the response how much content to listen for
        //Tells the recipient (browser) how much of the data is the content
        response.ContentLength64 = buffer.Length;

        //Setting our response status code (Ok, Bad, Good, etc.)
        //Casting our statusCode variable from type enum to type int

        response.StatusCode = (int)statusCode;

        // Simply here because CORS Sucks
        response.AddHeader("Access-Control-Allow-Origin", "*");

        //Writing or sending our response
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }
}