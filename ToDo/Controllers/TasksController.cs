using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services.TaskService;

namespace ToDo.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskService _taskService;

        public TasksController(ApplicationDbContext context, ITaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tasks.Include(t => t.Project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }




        // GET: Tasks/Create
        public async Task<IActionResult> Create(int projectId)
        {
            ViewData["Project"] = await _context.Projects.FindAsync(projectId);
            return PartialView();
        }
        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Task task)
        {
            task = await _taskService.CreateTask(task);
            return PartialView(task);
        }

        public async Task<IActionResult> Complete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            task.isCompleted = true;
            task.CompletedDate = DateTime.Now;

            try
            {
                _context.Update(task);
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
            return RedirectToAction("Main", "Home");
        }

        public async Task<IActionResult> CompleteAll(int? projectId)
        {
            var tasks = await _context.Tasks.Where(t => t.ProjectId == projectId).Where(t => !t.isCompleted).ToListAsync();
            foreach (var task in tasks)
            {
                task.isCompleted = true;
                task.CompletedDate = DateTime.Now;
                _context.Update(task);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Home");
        }

        // GET: Tasks/_Edit/5
        [HttpGet]
        public async Task<IActionResult> _Edit(int? id)
        {
            var task = await _taskService.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }

            return PartialView(task);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Task task)
        //{
        //    if (id != task.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(task);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TaskExists(task.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToPage("/Home/DisplayView", new { projectId = task.ProjectId });
        //    }
        //    ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", task.ProjectId);
        //    return PartialView(task);
        //}

        // GET: Tasks/_Delete/5
        [HttpGet]
        public async Task<IActionResult> _Delete(int? id)
        {
            var task = _taskService.GetTask(id);
            if (task == null)
            {
                return NotFound();
            }

            return PartialView(task);
        }

        //POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _taskService.DeleteTask(id);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Main", "Home");
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
