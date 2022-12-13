using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services.ProjectService;

namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IProjectService projectService)
        {
            _context = context;
            _logger = logger;
            _projectService = projectService;
        }

        public async Task<IActionResult> Main()
        {
            var projects = await _projectService.GetAllProjects();
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