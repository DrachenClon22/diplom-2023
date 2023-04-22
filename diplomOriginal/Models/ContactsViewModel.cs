using diplomOriginal.Controllers;
using System.ComponentModel.DataAnnotations;

namespace diplomOriginal.Models
{
    public class ContactsViewModel
    {
        [Display(Name = "Введите почту")]
        [Required(ErrorMessage = "Необходимо ввести почту")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Введите сообщение")]
        [Required(ErrorMessage = "Сообщение не может быть пустым")]
        [StringLength(100, ErrorMessage = "Сообщение не может быть более 100 символов")]
        public string Message { get; set; } = string.Empty;
    }
}
