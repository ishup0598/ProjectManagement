using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProjectDetails
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public DateTime DeadlineDate { get; set; }
        public int NoOfEmployees { get; set; }
        public string ProjectManager { get; set; }
        public string TeamLead { get; set; }
        public string Developers { get; set; }
    }
}
