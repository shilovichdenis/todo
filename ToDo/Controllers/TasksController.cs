using Microsoft.AspNetCore.Mvc;

namespace ToDo.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public TasksController(ITaskService taskService, IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasks();
            return View(tasks);
        }

        // GET: Tasks/_Create
        [HttpGet]
        public async Task<IActionResult> _Create(int projectId)
        {
            var result = await _projectService.GetProject(projectId);
            ViewData["Project"] = result.Item1;

            TempData["ProjectId"] = projectId;

            return PartialView();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Task task)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            if (!ModelState.IsValid)
            {
                return PartialView(task);
            }

            var result = await _taskService.CreateTask(task);

            if (!result.Item1)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
        }

        // GET: Tasks/_Edit/5
        [HttpGet]
        public async Task<IActionResult> _Edit(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());
            TempData["ProjectId"] = tempProjectId;

            var result = await _taskService.GetTask(id);

            if (result.Item1 == null)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                return PartialView(result.Item1);
            }
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Task task)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            if (!ModelState.IsValid)
            {
                //return partial or redirect to action
                TempData["TaskMessage"] = "Error. Model is not valid";
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
                return PartialView("_Edit", task);
            }

            var result = await _taskService.UpdateTask(id, task);

            if (!result.Item1)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
        }

        public async Task<IActionResult> Complete(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            var result = await _taskService.GetTask(id);
            if (result.Item1 == null)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                result.Item1.isCompleted = true;
                result.Item1.CompletedDate = DateTime.Now;

                var resultUpdate = await _taskService.UpdateTask(id, result.Item1);
                if (!resultUpdate.Item1)
                {
                    TempData["TaskMessage"] = resultUpdate.Item2;
                    return RedirectToAction("Details", "Projects", new { Id = result.Item1.ProjectId });
                }
                else
                {
                    TempData["TaskMessage"] = "Success. Task completed";
                    return RedirectToAction("Details", "Projects", new { Id = result.Item1.ProjectId });
                }
            }
        }

        public async Task<IActionResult> Uncomplete(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            var result = await _taskService.GetTask(id);
            if (result.Item1 == null)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                result.Item1.isCompleted = false;
                result.Item1.CompletedDate = null;

                var resultUpdate = await _taskService.UpdateTask(id, result.Item1);
                if (!resultUpdate.Item1)
                {
                    TempData["TaskMessage"] = resultUpdate.Item2;
                    return RedirectToAction("Details", "Projects", new { Id = result.Item1.ProjectId });
                }
                else
                {
                    TempData["TaskMessage"] = "Success. Task uncompleted";
                    return RedirectToAction("Details", "Projects", new { Id = result.Item1.ProjectId });
                }
            }
        }

        public async Task<IActionResult> CompleteAll(int projectId)
        {
            var tasks = await _taskService.GetAllTasksByProjectId(projectId);
            if (tasks == null || tasks.Count == 0)
            {
                TempData["TaskMessage"] = "Error. No Tasks found";
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }

            if (!tasks.Where(t => !t.isCompleted).Any())
            {
                TempData["TaskMessage"] = "Error. All Tasks have already been completed";
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }

            foreach (var task in tasks)
            {
                task.isCompleted = true;
                task.CompletedDate = DateTime.Now;
            }

            var result = await _taskService.UpdateAllTasks(tasks);
            if (!result.Item1)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }
            else
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = projectId });
            }

        }

        // GET: Tasks/_Delete/5
        [HttpGet]
        public async Task<IActionResult> _Delete(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());
            TempData["ProjectId"] = tempProjectId;

            var result = await _taskService.GetTask(id);
            if (result.Item1 == null)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                return PartialView(result.Item1);
            }
        }

        //POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            var result = await _taskService.DeleteTask(id);

            if (!result.Item1)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
        }

        // GET: Tasks/Details/5
        [ActionName("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var tempProjectId = int.Parse(TempData["ProjectId"].ToString());

            var result = await _taskService.GetTask(id);
            if (result.Item1 == null)
            {
                TempData["TaskMessage"] = result.Item2;
                return RedirectToAction("Details", "Projects", new { Id = tempProjectId });
            }
            else
            {
                return View(result.Item1);
            }
        }
    }
}
