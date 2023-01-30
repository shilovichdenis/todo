using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProjectService _projectService;

        public ProjectsController(IWebHostEnvironment webHostEnvironment, IProjectService projectService)
        {
            _webHostEnvironment = webHostEnvironment;
            _projectService = projectService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllProjects());
        }

        // GET: Projects/_Create
        [HttpGet]
        public IActionResult _Create()
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
                TempData["ProjectMessage"] = "Model is not valid";
                return RedirectToAction("Main", "Home");
            }

            var result = await _projectService.CreateProject(component);

            if (!result.Item1)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
        }

        // GET: Projects/_Edit/5
        [HttpGet]
        public async Task<IActionResult> _Edit(int id)
        {
            var result = await _projectService.GetProject(id);

            if (result.Item1 == null)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = id });
            }
            else
            {
                return PartialView(result);
            }
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                TempData["ProjectMessage"] = "Error. Model is not valid";
                return RedirectToAction("Details", "Projects", new { Id = id });
            }

            var result = await _projectService.UpdateProject(id, project);

            if (!result.Item1)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = id });
            }
            else
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = id });
            }
        }

        // GET: Projects/_Delete/5
        [HttpGet]
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _projectService.GetProject(id);
            if (result.Item1 == null)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                return PartialView(result.Item1);
            }
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _projectService.DeleteProject(id);
            if (!result.Item1)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["TaskStatusMessage"] = TempData["TaskMessage"] as string;
            ViewData["ProjectStatusMessage"] = TempData["ProjectMessage"] as string;
            TempData["ProjectId"] = id;
            var result = await _projectService.GetProject(id);
            if (result.Item1 == null)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                return View(result.Item1);
            }
        }

        public async Task<IActionResult> Hide(int id)
        {
            var result = await _projectService.GetProject(id);

            if (result.Item1 == null)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }

            result.Item1.isHidden = true;
            var resultUpdate = await _projectService.UpdateProject(id, result.Item1);

            if (!resultUpdate.Item1)
            {
                TempData["ProjectMessage"] = resultUpdate.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                TempData["ProjectMessage"] = resultUpdate.Item2;
                return RedirectToAction("Main", "Home");
            }
        }

        public async Task<IActionResult> Show(int id)
        {
            var result = await _projectService.GetProject(id);

            if (result.Item1 == null)
            {
                TempData["ProjectMessage"] = result.Item2;
                return RedirectToAction("Main", "Home");
            }

            result.Item1.isHidden = false;
            var resultUpdate = await _projectService.UpdateProject(id, result.Item1);

            if (!resultUpdate.Item1)
            {
                TempData["ProjectMessage"] = resultUpdate.Item2;
                return RedirectToAction("Main", "Home");
            }
            else
            {
                TempData["ProjectMessage"] = resultUpdate.Item2;
                return RedirectToAction("Main", "Home");
            }
        }





    }
}
