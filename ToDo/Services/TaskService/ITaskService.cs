namespace ToDo.Services.TaskService
{
    public interface ITaskService
    {
        Task<List<Task>>? GetAllTasks();
        Task<Task>? GetTask(int? id);
        Task<Task> CreateTask(Task task);
        Task<List<Task>> UpdateAllTasks(List<Task> tasks);
        Task<Task>? UpdateTask(int? id, Task task);
        Task<bool> DeleteTask(int? id);
        bool TaskExists(int? id);

    }
}
