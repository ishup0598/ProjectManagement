using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ProjectManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IConfiguration _configuration;
        public EmployeeController(AppDbContext context, UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Employee> signInManager, IConfiguration configuration)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = await _userManager.FindByNameAsync(user.Username);

                    var result = await _signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);

                    var userroles = await _userManager.GetRolesAsync(users);

                    if (result.Succeeded && userroles.Any())
                    {
                        var roles = userroles.FirstOrDefault();
                        switch (roles)
                        {
                            case "Admin":
                                HttpContext.Response.Cookies.Append("role", roles);
                                return RedirectToAction("Index","Project");
                            case "ProjectManager":
                                HttpContext.Response.Cookies.Append("role", roles);
                                return RedirectToAction("Index","Project");
                            case "TeamLead":
                                HttpContext.Response.Cookies.Append("role", roles);
                                return RedirectToAction("Index","Project");
                            case "Developer":
                                HttpContext.Response.Cookies.Append("role", roles);
                                return RedirectToAction("Index", "Project");
                            default:
                                return View("Login");
                        }
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }

        }
        public async Task<IActionResult> Logout()
        {

            if (User.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Employee model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                return BadRequest("User Already Exists");
            }
            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.ValidateUser = ("Password do not match");
            }
            Employee employee = new Employee()
            {
                UserName = model.UserName,
                EmployeeName = model.EmployeeName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmployeeCode = model.EmployeeCode,
                JoiningDate = model.JoiningDate,
                EmployeeType = model.EmployeeType
            };
            var result = await _userManager.CreateAsync(employee, model.Password);
            if (!result.Succeeded)
                return BadRequest("User Creation Failed!! Please check user details and try again.");
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.ProjectManager))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.ProjectManager));
            if (!await _roleManager.RoleExistsAsync(UserRoles.TeamLead))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.TeamLead));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Developer))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Developer));

            if (model.EmployeeType == EmployeeType.ProjectManager)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.ProjectManager))
                {
                    await _userManager.AddToRoleAsync(employee, UserRoles.ProjectManager);
                }
            }
            if (model.EmployeeType == EmployeeType.TeamLead)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.TeamLead))
                {
                    await _userManager.AddToRoleAsync(employee, UserRoles.TeamLead);
                }
            }
            if (model.EmployeeType == EmployeeType.Developer)
            {
                if (await _roleManager.RoleExistsAsync(UserRoles.Developer))
                {
                    await _userManager.AddToRoleAsync(employee, UserRoles.Developer);
                }
            }


            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (User == null)
            {
                return NotFound();
            }
            ViewBag.role = HttpContext.Request.Cookies["role"];
            var admin = User.IsInRole("Admin");
            IEnumerable<Employee> employees = _context.Employees.ToList();
            return View(employees);


        }
    }
}
