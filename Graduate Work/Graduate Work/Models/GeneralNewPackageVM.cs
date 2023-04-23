using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Graduate_Work.Models
{
    public class GeneralNewPackageVM
    {
        [Required(ErrorMessage = "Номер телефону відправника є пустим")]
        [DisplayName("Номер телефону відправника")]
        public string SenderPhone { get; set; }
        [Required(ErrorMessage = "Номер відділення відправника є пустим")]
        [Display(Name = "Номер відділення відправника")]
        public int SenderDepartmentId { get; set; }
        [Required(ErrorMessage = "Номер телефону отримувача є пустим")]
        [DisplayName("Номер телефону отримувача")]
        public string ReciverPhone { get; set; }
        [Required(ErrorMessage = "Номер відділення отримувача є пустим")]
        [Display(Name = "Номер відділення отримувача")]
        public int ReciverDepartmentId { get; set; }
        [Required(ErrorMessage = "Назва типу є пустою")]
        [MinLength(3, ErrorMessage = "Назва типу не можк бути коротшою за 3 символи")]
        [MaxLength(15, ErrorMessage = "Назва типу не може бути довшою за 15 символів")]
        [Display(Name = "Назва типу")]
        public string PackageTypeName { get; set; }

    }
}
