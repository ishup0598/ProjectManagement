using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Register
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string EmployeeCode { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public DateTime JoiningDate { get; set; }
        public EmployeeType EmployeeType { get; set; }

    }
}
