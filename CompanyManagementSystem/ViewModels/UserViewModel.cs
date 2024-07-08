using System.ComponentModel.DataAnnotations;

namespace CompanyManagementSystem.PL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        [EmailAddress(ErrorMessage = "This Is Not A Correct Email")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public Dictionary<string, bool> AvailableRoles { get; set; } = new Dictionary<string, bool>();


    }
}
