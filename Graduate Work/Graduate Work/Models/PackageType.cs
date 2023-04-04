using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("PackageTypes")]
    public class PackageType
    {
        [Key]
        public int PackageTypeId { get; set; }
        [Required, Display(Name = "Назва типу")]
        public string NameOfType { get; set; }
        [Required, Display(Name = "Відсоткова ставка")]
        public double InterestRate { get; set; }
    }
}
