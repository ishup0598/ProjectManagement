using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        public ProjectController(AppDbContext context, UserManager<Employee> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.role = HttpContext.Request.Cookies["role"];
            var admin = User.IsInRole("Admin");
            IEnumerable<ProjectDetails> projectDetails = _context.ProjectDetails.ToList();
            var pManager = await _userManager.GetUsersInRoleAsync(EmployeeType.ProjectManager.ToString());
            var teamLead = await _userManager.GetUsersInRoleAsync(EmployeeType.TeamLead.ToString());
            var developers = await _userManager.GetUsersInRoleAsync(EmployeeType.Developer.ToString());
            ViewBag.pManager = pManager.ToList();
            ViewBag.teamLead = teamLead.ToList();
            ViewBag.developers = developers.ToList();
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                projectDetails = projectDetails.Where(s => s.ProjectName.Contains(searchString)
                                       || s.ProjectManager.Contains(searchString) || s.TeamLead.Contains(searchString));

            }

            return View(projectDetails);
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {

            if (id == null || id == 0)
            {

                ProjectVM project = new ProjectVM()
                {
                    ProjectDetails = new(),
                    projectManager = _context.Employees.Where(x => x.EmployeeType == EmployeeType.ProjectManager).Select(x => new SelectListItem
                    {
                        Text = x.EmployeeName,
                        Value = x.Id
                    }),
                    TeamLead = _context.Employees.Where(x => x.EmployeeType == EmployeeType.TeamLead).Select(x => new SelectListItem
                    {
                        Text = x.EmployeeName,
                        Value = x.Id
                    }),
                    Developers = _context.Employees.Where(x => x.EmployeeType == EmployeeType.Developer).Select(x => new SelectListItem
                    {
                        Text = x.EmployeeName,
                        Value = x.Id
                    })
                };
                ViewBag.Developers = project.Developers.ToList();
                return View(project);
            }
            else
            {
                var projectDetails = _context.ProjectDetails.Find(id);
                return View(projectDetails);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProjectVM model)
        {
            if (model.ProjectDetails.ProjectId != 0)
            {
                _context.ProjectDetails.Update(model.ProjectDetails);
                _context.SaveChanges();
            }
            else
            {
                _context.ProjectDetails.Add(model.ProjectDetails);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
