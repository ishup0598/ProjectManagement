using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProjectVM
    {
        public ProjectDetails ProjectDetails { get; set; }
        public IEnumerable<SelectListItem> projectManager { get; set; }
        public IEnumerable<SelectListItem> TeamLead { get; set; }
        public IEnumerable<SelectListItem> Developers { get; set; }

    }
}
