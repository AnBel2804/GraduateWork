using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Ім`я є пустим")]
        [StringLength(25, ErrorMessage = "Ім`я не може бути довшим за 25 символів")]
        [MinLength(4, ErrorMessage = "Ім`я не може бути коротшим за 4 символи")]
        [DisplayName("Ім`я")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Прізвище є пустим")]
        [StringLength(25, ErrorMessage = "Прізвище не може бути довшим за 25 символів")]
        [MinLength(4, ErrorMessage = "Прізвище не може бути коротшим за 4 символи")]
        [DisplayName("Прізвище")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Номер телефону є пустим")]
        [DisplayName("Номер телефону")]
        public string Phone { get; set; }
        [ValidateNever]
        [Required]
        public IdentityUser User { get; set; }

    }
}
