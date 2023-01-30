using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models.ViewComponents;
using System.IO;

namespace ToDo.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<(Project?, string)> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return (null, "Error. Project not found");
            }

            project.Tasks = await _context.Tasks.Where(t => t.ProjectId == id).Where(t => t.ProjectId == id).OrderByDescending(t => t.Priority).ThenByDescending(t => t.CompletedDate).ToListAsync();
            return (project, string.Empty);
        }
        public async Task<(bool, string)> CreateProject(ProjectViewComponent component)
        {
            if (component == null)
            {
                return (false, "Error. Project is null");
            }

            string imagesPath = Path.Combine("images", "projects", component.Name.ToLower());
            string directoryPath = Path.Combine("wwwroot", "images", "projects", component.Name.ToLower());

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (component.Images != null)
            {
                foreach (IFormFile image in component.Images)
                {
                    string fileName = Path.GetFileName(image.FileName);
                    using (FileStream stream = new FileStream(Path.Combine("wwwroot", imagesPath, fileName), FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }
                }
            }

            var project = new Project()
            {
                Name = component.Name,
                CreatedDate = component.CreatedDate,
                Description = component.Description,
                isHidden = component.isHidden,
                Type = component.Type,
                ImagesPath = imagesPath
            };

            if (ProjectExists(project.Id))
            {
                return (false, "Error. Project ID exists");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return (true, "Success. Project created");
        }
        public async Task<(bool, string)> UpdateProject(int id, Project project)
        {
            if (project == null)
            {
                return (false, "Error. Project is null");
            }

            if (id != project.Id)
            {
                return (false, "Error. ID and Project Id do not match");
            }

            _context.Update(project);
            await _context.SaveChangesAsync();
            return (true, "Success. Project updated");
        }
        public async Task<(bool, string)> DeleteProject(int id)
        {
            if (!ProjectExists(id))
            {
                return (true, "Success. Project has already been deleted");
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return (false, "Error. Project is not found");
            }
            
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            var directoryPath = Path.Combine("wwwroot", project.ImagesPath);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            return (true, "Success. Project deleted");
        }

        public async Task<List<Project>?> GetAllProjects() => await _context.Projects.Include(p => p.Tasks.OrderByDescending(t => t.Priority).ThenBy(t => t.CreatedDate)).OrderBy(p => p.isHidden).ToListAsync();

        public bool ProjectExists(int id) => _context.Projects.Any(e => e.Id == id);
    }
}
