using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using Microsoft.Build.Evaluation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using ToDo.Services.ProjectService;

namespace ToDo.Pages
{

    public enum ErrorTask
    {
        [Description("Task not found.")]
        Exists,
        [Description("Model is not valid.")]
        ModelIsNotValid,
        [Description("Task not found.")]
        NotFound,
        [Description("Id is NULL.")]
        IdIsNull,
        [Description("Database table is not created.")]
        DatabaseTableIsNull
    }

    public enum ErrorProject
    {
        [Description("Project not found.")]
        Exists,
        [Description("Model is not valid.")]
        ModelIsNotValid,
        [Description("Project not found.")]
        NotFound,
        [Description("Id is NULL.")]
        IdIsNull,
        [Description("Database table is not created. ")]
        DatabaseTableIsNull
    }
    public class DisplayView : PageModel
    {
        private readonly string Error = "ERROR: ";
        private readonly string Success = "SUCCESS: ";

        private string GetEnumDescription(Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(enumValue));
        }

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProjectService _projectService;

        public DisplayView(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IProjectService projectService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _projectService = projectService;
        }

        [BindProperty]
        public Project Project { get; set; }
        public List<Task>? Tasks { get; set; }

        [TempData]
        public string? ProjectStatusMessage { get; set; }
        [TempData]
        public string? TaskStatusMessage { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                ProjectStatusMessage = Error + GetEnumDescription(ErrorProject.IdIsNull) + " OR " + GetEnumDescription(ErrorProject.DatabaseTableIsNull);
                return Page();
            }

            var result = await _projectService.GetProject(id);
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == id)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.CreatedDate)
                .ToListAsync();

            if (result == null)
            {
                ProjectStatusMessage = Error + GetEnumDescription(ErrorProject.NotFound);
                return Page();
            }
            var provider = new PhysicalFileProvider(_webHostEnvironment.WebRootPath);
            var contents = provider.GetDirectoryContents(result.ImagesPath);
            var objFiles = contents.OrderBy(m => m.LastModified);
            var images = new List<string>();

            foreach (var item in objFiles.ToList())
            {
                images.Add(item.Name);
            }

            result.Images = images;
            Project = result;
            Tasks = tasks;
            return Page();
        }
        public async Task<IActionResult> OnPostEditProject()
        {
            if (!ModelState.IsValid)
            {
                ProjectStatusMessage = Error + GetEnumDescription(ErrorProject.ModelIsNotValid);
                Tasks = await _context.Tasks
                    .Where(t => t.ProjectId == Project.Id)
                    .OrderByDescending(t => t.Priority)
                    .ThenBy(t => t.CreatedDate)
                    .ToListAsync();
                return Page();
            }
            else
            {
                try
                {
                    _context.Attach(Project).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(Project.Id))
                    {
                        ProjectStatusMessage = Error + GetEnumDescription(ErrorProject.Exists);
                        return RedirectToPage(new { projectId = Project.Id });
                    }
                    else
                    {
                        throw;
                    }
                }
                ProjectStatusMessage = "SUCCESS: Project is changed.";
                await OnGet(Project.Id);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCompleteTask(int? id, int projectId)
        {
            if (id == null || _context.Tasks == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.IdIsNull) + " OR " + GetEnumDescription(ErrorTask.DatabaseTableIsNull);
                return RedirectToPage(new { id = projectId });
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.NotFound);
                return RedirectToPage(new { id = projectId });
            }

            task.isCompleted = true;
            task.CompletedDate = DateTime.Now;

            try
            {
                _context.Attach(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    TaskStatusMessage = Error + GetEnumDescription(ErrorTask.Exists);
                    return RedirectToPage(new { id = projectId });
                }
                else
                {
                    throw;
                }
            }
            TaskStatusMessage = "SUCCESS: Task completed.";
            return RedirectToPage(new { id = projectId });
        }

        public async Task<IActionResult> OnPostCompleteAllTasks(int projectId)
        {
            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).Where(t => !t.isCompleted).ToListAsync();

            if (tasks == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.NotFound);
                return RedirectToPage(new { id = projectId });
            }

            foreach (var task in tasks)
            {
                task.isCompleted = true;
                task.CompletedDate = DateTime.Now;
                _context.Attach(task).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            TaskStatusMessage = "SUCCESS: Tasks completed.";
            return RedirectToPage(new { id = projectId });
        }

        public async Task<IActionResult> OnPostUncompleteTask(int? id, int projectId)
        {
            if (id == null || _context.Tasks == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.IdIsNull) + " OR " + GetEnumDescription(ErrorTask.DatabaseTableIsNull);
                return RedirectToPage(new { id = projectId });
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.NotFound);
                return RedirectToPage(new { id = projectId });
            }

            task.isCompleted = false;
            task.CompletedDate = null;

            try
            {
                _context.Attach(task).State = EntityState.Modified;
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

            TaskStatusMessage = "SUCCESS: Task field changed to UNCOMPLITED.";
            return RedirectToPage(new { id = projectId });
        }


        public async Task<IActionResult> OnPostEditTask(int? id, int projectId, Task task)
        {
            if (id != task.Id)
            {
                TaskStatusMessage = "ERROR: Id not equal model Id.";
                return RedirectToPage(new { id = projectId });
            }

            if (!ModelState.IsValid)
            {
                var errors =
                        from value in ModelState.Values
                        where value.ValidationState == ModelValidationState.Invalid
                        select value;
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.ModelIsNotValid);

                foreach (var (index, error) in errors.Select((e, i) => (i, e)))
                {
                    TaskStatusMessage += "Error[" + index + "]: " + error.Errors[0].ErrorMessage + " | " + error.Errors.Count() + "\n";
                }

                TaskStatusMessage += " | " + task.Id + " | " + task.ProjectId + " | " + task.Priority + " | " + task.Title + " | " + task.CreatedDate + " | " + task.CompletedDate + " | " + task.isCompleted + " Task UNCHANGED";
                return RedirectToPage(new { id = projectId });

            }
            else
            {
                try
                {
                    _context.Attach(task).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        TaskStatusMessage = Error + GetEnumDescription(ErrorTask.Exists);
                        return RedirectToPage(new { id = projectId });
                    }
                    else
                    {
                        throw;
                    }
                }
                TaskStatusMessage = "SUCCESS: Task changed.";
                return RedirectToPage(new { id = projectId });
            }
        }

        public async Task<IActionResult> OnPostEditTaskDescription(int? id, int projectId, string textAreaValue)
        {
            if (id == null || _context.Tasks == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.IdIsNull) + " OR " + GetEnumDescription(ErrorTask.DatabaseTableIsNull);
                return RedirectToPage(new { id = projectId });
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.NotFound);
                return RedirectToPage(new { id = projectId });
            }

            task.Description = textAreaValue;

            try
            {
                _context.Attach(task).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    TaskStatusMessage = Error + GetEnumDescription(ErrorTask.Exists);
                    return RedirectToPage(new { id = projectId });
                }
                else
                {
                    throw;
                }
            }
            TaskStatusMessage = "SUCCESS: Task description changed.";
            return RedirectToPage(new { id = projectId });

        }
        public async Task<IActionResult> OnPostDeleteTask(int? id, int projectId)
        {
            if (_context.Tasks == null)
            {
                TaskStatusMessage = "ERROR: Database don't has table.";
                return RedirectToPage(new { id = projectId });
            }
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                TaskStatusMessage = Error + GetEnumDescription(ErrorTask.NotFound);
                return RedirectToPage(new { id = projectId });
            }

            try
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

            }

            TaskStatusMessage = "SUCCESS: Task deleted.";
            return RedirectToPage(new { id = projectId });
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
