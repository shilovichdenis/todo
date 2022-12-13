using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ToDo.Data;
using ToDo.Models;
using ToDo.Models.ViewComponents;
using ToDo.Services.ProjectService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDo.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProjectService _projectService;

        public ProjectsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IProjectService projectService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _projectService = projectService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllProjects());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var result = _projectService.GetProject(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewComponent component)
        {
            if (!ModelState.IsValid)
            {
                return View(component);
            }

            var task = _projectService.CreateProject(component);
            if (!task.Result)
            {
                return View(component);
            }
            return RedirectToAction("Index", "Projects");
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.isHidden = true;
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!ProjectExists(project.Id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }
            return RedirectToAction("Main", "Home");
        }

        public async Task<IActionResult> Show(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            project.isHidden = false;
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!ProjectExists(project.Id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }
            return RedirectToAction("Main", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> _Edit(int id, Project project)
        {
            return PartialView(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var result = await _projectService.GetProject(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(project);
            }

            var result = await _projectService.UpdateProject(id, project);
            if (!result)
            {
                return PartialView(project);
            }
            return RedirectToAction("Main", "Home");
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var project = await _projectService.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }
            return PartialView(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _projectService.DeleteProject(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Main", "Home");
        }
    }
}
