using System.ComponentModel.DataAnnotations;

namespace CompanyManagementSystem.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "New Password Is Required")]
		[DataType(DataType.Password)]
        public string NewPassword { get; set; }
		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Password does not match")]
		public string ConfirmPassword { get; set; }
    }
}
