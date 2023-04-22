using System.ComponentModel.DataAnnotations;

namespace diplomOriginal.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Введите email")]
        [Required(ErrorMessage = "Необходимо ввести email")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Введите пароль")]
        [Required(ErrorMessage = "Необходимо ввести пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
