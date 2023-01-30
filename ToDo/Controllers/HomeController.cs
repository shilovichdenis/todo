﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDo.Data;

namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Main()
        {
            ViewData["ProjectStatusMessage"] = TempData["ProjectMessage"] as string;
            var projects = await _context.Projects.Include(p => p.Tasks.Where(t => !t.isCompleted).OrderByDescending(t => t.Priority).ThenBy(t => t.CreatedDate)).OrderBy(p => p.isHidden).ToListAsync();
            return View(projects);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}