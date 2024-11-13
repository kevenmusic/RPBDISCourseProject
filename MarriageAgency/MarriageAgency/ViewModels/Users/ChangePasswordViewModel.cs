using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.Users
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле пароль обязательно для заполнения.")]
        public string NewPassword { get; set; }
    }
}
