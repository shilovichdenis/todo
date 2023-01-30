using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;

namespace ToDo.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(Task?, string)> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return (null, "Error. Task is not found.");
            }

            var project = await _context.Projects.FindAsync(id);
            task.Project = project;
            return (task, string.Empty);
        }

        public async Task<(bool, string)> CreateTask(Task task)
        {
            if (task == null)
            {
                return (false, "Error. Task is null");
            }

            if (TaskExists(task.Id))
            {
                return (false, "Error. Task ID exists");
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return (true, "Success. Task created");
        }
        public async Task<(bool, string)> UpdateTask(int id, Task task)
        {
            if (task == null)
            {
                return (false, "Error. Task is null");
            }

            if (id != task.Id)
            {
                return (false, "Error. ID and Task ID do not match");
            }

            _context.Update(task);
            await _context.SaveChangesAsync();
            return (true, "Success. Task updated");
        }
        public async Task<(bool, string)> UpdateAllTasks(List<Task> tasks)
        {
            if (tasks == null)
            {
                return (false, "Error. Tasks are null");
            }

            _context.Tasks.UpdateRange(tasks);
            await _context.SaveChangesAsync();

            return (true, "Success. Tasks updated");
        }
        public async Task<(bool, string)> DeleteTask(int id)
        {
            if (!TaskExists(id))
            {
                return (true, "Success. Task has already been deleted");
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return (false, "Error. Task is not found");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return (true, "Success. Task deleted");
        }

        public async Task<List<Task>?> GetAllTasks() => await _context.Tasks.Include(t => t.Project).OrderBy(t => t.Project.Name).ThenByDescending(t => t.Priority).ThenByDescending(t => t.CompletedDate).ToListAsync();
        public async Task<List<Task>?> GetAllTasksByProjectId(int id) => await _context.Tasks.Include(t => t.Project).Where(t => t.ProjectId == id).OrderBy(t => t.Priority).ThenByDescending(t => t.CompletedDate).ToListAsync();
        public bool TaskExists(int id) => _context.Tasks.Any(t => t.Id == id);

    }
}
