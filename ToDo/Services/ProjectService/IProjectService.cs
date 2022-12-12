﻿using Microsoft.EntityFrameworkCore;
using ToDo.Models.ViewComponents;

namespace ToDo.Services.ProjectService
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjects();
        Task<Project>? GetProject(int? id);
        Task<Project>? UpdateProject(int? id, Project project);
        Task<bool> DeleteProject(int? id);
        Task<Project> CreateProject(ProjectViewComponent project);
        bool ProjectExists(int id);
    }
}