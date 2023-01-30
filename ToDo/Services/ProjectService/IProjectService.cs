using Microsoft.EntityFrameworkCore;
using ToDo.Models.ViewComponents;

namespace ToDo.Services.ProjectService
{
    public interface IProjectService
    {
        Task<(Project?, string)> GetProject(int id);
        Task<(bool, string)> CreateProject(ProjectViewComponent project);
        Task<(bool, string)> UpdateProject(int id, Project project);
        Task<(bool, string)> DeleteProject(int id);
        Task<List<Project>?> GetAllProjects();
        bool ProjectExists(int id);
    }
}
