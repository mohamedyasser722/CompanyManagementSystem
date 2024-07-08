using CompanyManagementSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace CompanyManagementSystem.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Max Length Is 50 Chars ")]
        [MinLength(5, ErrorMessage = "Min Length Is 5 Chars ")]
        public string Name { get; set; }
        [Range(18, 60, ErrorMessage = "Age Must Be In Range Between 18 To 60 ")]
        public int? Age { get; set; }
        [DataType(DataType.Currency)]
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
