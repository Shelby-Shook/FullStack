using ExampleServer.Data;

TaskModel.TotalTasks = 0;
Console.WriteLine(TaskModel.TotalTasks);

//Instance of our class
TaskModel task1 = new TaskModel("Task 1", "The first task");
task1.WriteTotalTasks();

TaskModel task2 = new TaskModel("Task 2", "The second task");
task1.WriteTotalTasks();
task2.WriteTotalTasks();

Console.WriteLine(task2.Id);