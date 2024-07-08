namespace CompanyManagementSystem.PL.ViewModels
{
    public class UserRoleViewModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public IList<string>? Roles { get; set; }
        public string? SelectedRole { get; set; }
    }

}
