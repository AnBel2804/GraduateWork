using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Graduate_Work.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "E-mail є пустим")] 
        [EmailAddress, DisplayName("E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль є пустим")] 
        [MinLength(8, ErrorMessage = "Довжина паролю не може бути меншою 8 символів")]
        [DisplayName("Пароль"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
