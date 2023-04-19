using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Назва міста є пустою")] 
        [Display(Name = "Місто")]
        public string City { get; set; }
        [Required(ErrorMessage = "Номер відділення є пустим")]
        [Display(Name = "Номер відділення")]
        public int NumberOfDepartment { get; set; }
    }
}
