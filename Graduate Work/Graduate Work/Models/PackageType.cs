using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("PackageTypes")]
    public class PackageType
    {
        [Key]
        public int PackageTypeId { get; set; }
        [Required(ErrorMessage = "Назва типу є пустою")]
        [MinLength(3, ErrorMessage = "Назва типу не можк бути коротшою за 3 символи")]
        [MaxLength(15, ErrorMessage = "Назва типу не може бути довшою за 15 символів")]
        [Display(Name = "Назва типу")]
        public string NameOfType { get; set; }
        [Required(ErrorMessage = "Відсоткова ставка є пустою")]
        [Display(Name = "Відсоткова ставка")]
        public double InterestRate { get; set; }
    }
}
