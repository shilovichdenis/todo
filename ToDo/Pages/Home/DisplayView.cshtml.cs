using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ToDo.Models;
using Task = ToDo.Models.Task;
using Project = ToDo.Models.Project;
using ToDo.Data;
using Microsoft.Build.Evaluation;

namespace ToDo.Pages
{
    public class DisplayView : PageModel
    {

        private readonly ApplicationDbContext _context;
        public DisplayView(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; }

        [TempData]
        public string EditProjectStatusMessage { get; set; }
        [TempData]
        public string CompletedTaskStatusMessage { get; set; }
        [TempData]
        public string DeletedTaskStatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? projectId)
        {
            if (projectId == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(projectId);
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.Priority)
                .ToListAsync();


            if (project == null)
            {
                return NotFound();
            }

            Project = project;
            Tasks = tasks;

            return Page();
        }

        public async Task<IActionResult> OnPostEditProjectAsync()
        {
            if (!ModelState.IsValid)
            {
                EditProjectStatusMessage = "Error";
                Tasks = await _context.Tasks
                    .Where(t => t.ProjectId == Project.Id)
                    .OrderByDescending(t => t.Priority)
                    .ToListAsync();
                return Page();
            }

            _context.Attach(Project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(Project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            EditProjectStatusMessage = "Your data is changed.";
            return RedirectToPage(new { projectId = Project.Id });
        }

        public async Task<IActionResult> OnPostCompleteTaskAsync(int? taskId)
        {
            if (taskId == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(taskId);
            task.isCompleted = true;
            task.CompletedDate = DateTime.Now.ToString();

            _context.Attach(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            CompletedTaskStatusMessage = "Your task completed.";
            return RedirectToPage(new { projectId = task.ProjectId });
        }
        public async Task<IActionResult> OnPostDeleteTaskAsync(int? taskId)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();

            DeletedTaskStatusMessage = "Your task deleted.";
            return RedirectToPage(new { projectId = task.ProjectId });
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
