﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models.ViewComponents;

namespace ToDo.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateProject(ProjectViewComponent component)
        {
            if (component == null)
            {
                return false;
            }

            string imagesPath = Path.Combine("images", "projects", component.Name.ToLower());

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
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

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProject(int? id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));


            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _context.Projects
                .Include(p => p.Tasks.Where(t => !t.isCompleted).OrderByDescending(t => t.Priority).ThenBy(t => t.CreatedDate))
                .OrderBy(p => p.isHidden)
                .ToListAsync();
        }

        public async Task<Project>? GetProject(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var project = await _context.Projects
                .Include(p => p.Tasks.OrderBy(t => t.isCompleted).OrderByDescending(t => t.CreatedDate))
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (project == null) throw new ArgumentNullException($"{nameof(project)}");

            return project;
        }


        public async Task<bool> UpdateProject(int? id, Project project)
        {
            if (id != project.Id || id == null)
            {
                return false;
            }

            _context.Update(project);
            await _context.SaveChangesAsync();

            if (!ProjectExists(project.Id))
            {
                return false;
            }

            return true;
        }

        public bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

    }
}
