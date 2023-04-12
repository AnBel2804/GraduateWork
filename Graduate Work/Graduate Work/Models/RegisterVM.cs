using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Graduate_Work.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Логін є пустим")] 
        [StringLength(25, ErrorMessage = "Логін не може бути довшим за 25 символів")] 
        [MinLength(4, ErrorMessage = "Логін не може бути коротшим за 4 символи")] 
        [DisplayName("Логін")]
        public string Login { get; set; }
        [Required(ErrorMessage = "E-mail є пустим"), EmailAddress] 
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль є пустим")] 
        [MinLength(8, ErrorMessage = "Довжина паролю не може бути меншою 8 символів")] 
        [DisplayName("Пароль"), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Підтвердження паролю є пустим")] 
        [MinLength(8, ErrorMessage = "Довжина паролю не може бути меншою 8 символів")] 
        [DisplayName("Підтвердження паролю"), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
