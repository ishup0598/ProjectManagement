using Data;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var project = new ProjectDetails();
            if (id == null || id == 0)
            {
                return View(project);
            }
            else
            {
                var projectDetails = _context.ProjectDetails.Find(id);
                return View(projectDetails);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProjectDetails model)
        {
            if (model.ProjectId == null)
            {
                _context.Add(model);
                _context.SaveChanges();
            }
            else
            {
                _context.Update(model);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
