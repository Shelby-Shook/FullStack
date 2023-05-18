//Task Repository is responsible for storing and manipulating
// our collection of data, in this case TaskModels

namespace ExampleServer.Data;

public class TaskRepository
{
    // Data storage (All of our tasks)
    private readonly List<TaskModel> _taskList = new List<TaskModel>();

    //Create method
    //Not creating an instance, we're creating an entry in the list

    public void AddTask(TaskModel task)
    {
        _taskList.Add(task);
        //_taskList.Contains(task);
    }

    //Read method
    public List<TaskModel> GetTasks()
    {
        return new List<TaskModel>(_taskList);
    }

    public List<TaskModel> GetTasksByStatus(bool isComplete)
    {
        //Start a new list
        List<TaskModel> tasks = new List<TaskModel>();

        // Iterate through all tasks and check the status
        foreach (TaskModel task in _taskList)
        {
            //Add a task to the new list if its status matches the parameter
            if (task.IsComplete == isComplete)
            {
                tasks.Add(task);
            }
        }
        //Return the new list
        return tasks; 
    }

    //Update method
    public bool MarkTaskAsComplete(int taskId)
    {
        TaskModel? task = _taskList.FirstOrDefault(t => t.Id == taskId);

        //Check if task doesnt exist OR already complete
        if (task == null || task.IsComplete)
        {
           // Return false to indicate it didnt change anything
        return false; 
        }

        task.IsComplete = true;
        return true;

       
    }

    //Delete method
    public bool DeleteTaskById(int id)
    {
        //Loop through each task
        foreach(var task in _taskList)
        {
            // Check the task Id against our parameter
            if (task.Id == id)
            {
                // If we find it remove the task and return true/false
                return _taskList.Remove(task);
            }
        }
        //Return false if we don't find the Id in the loop
        return false;
    }
}