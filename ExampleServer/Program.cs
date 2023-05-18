using ExampleServer.Data;
using ExampleServer.Server;

TaskModel.TotalTasks = 0;
Console.WriteLine(TaskModel.TotalTasks);

//Instance of our class
TaskModel task1 = new TaskModel("Task 1", "The first task");
task1.WriteTotalTasks();

TaskModel task2 = new TaskModel("Task 2", "The second task");
task1.WriteTotalTasks();
task2.WriteTotalTasks();

Console.WriteLine(task2.Id);

// Implicit Types
// var assumes the type from the righthand side of the expression
var task3 = new TaskModel("Task 3", "The third task");

//Target-typed new
// Implicit new will assume the type from the lefthand side
TaskModel task4 = new("Task 4", "The fourth task");

task1.IsComplete = true;
task4.IsComplete = true;

TaskRepository repo = new();
repo.AddTask(task1);
repo.AddTask(task2);
repo.AddTask(task3);
repo.AddTask(task4);


repo.DeleteTaskById(3);

// tasks gets its type from the GetTasks() return type
var tasks = repo.GetTasksByStatus(true);
foreach (var task in tasks)
{
    Console.WriteLine(task.Description);
}

//Initializing a WebServer instance
// Passing in a repository instance, and a URL to listen on
WebServer server = new WebServer(repo, "http://localhost:8000/");
server.Run(); //Public method in the class WebServer