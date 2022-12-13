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

        public async Task<Task> CreateTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int? id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            var result = _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Task>>? GetAllTasks()
        {
            return await _context.Tasks.Include(t => t.Project).ToListAsync();
        }

        public async Task<Task> GetTask(int? id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return null;
            }

            return task;
        }

        public async Task<List<Task>> UpdateAllTasks(List<Task> tasks)
        {
            if (tasks == null) throw new ArgumentNullException(nameof(tasks));

            foreach (var task in tasks)
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }

            return tasks;
        }

        public async Task<Task>? UpdateTask(int? id, Task task)
        {
            if (id != task.Id)
            {
                return null;
            }

            _context.Update(task);
            await _context.SaveChangesAsync();

            if (!TaskExists(task.Id))
            {
                return null;
            }

            return task;
        }

        public bool TaskExists(int? id)
        {
            return _context.Tasks.Any(t => t.Id == id);
        }

    }
}
