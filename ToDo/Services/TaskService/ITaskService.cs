namespace ToDo.Services.TaskService
{
    public interface ITaskService
    {
        Task<(Task?, string)> GetTask(int id);
        Task<(bool, string)> CreateTask(Task task);
        Task<(bool, string)> UpdateTask(int id, Task task);
        Task<(bool, string)> UpdateAllTasks(List<Task> tasks);
        Task<(bool, string)> DeleteTask(int id);
        Task<List<Task>?> GetAllTasks();
        Task<List<Task>?> GetAllTasksByProjectId(int id);
        bool TaskExists(int id);

    }
}
