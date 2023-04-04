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
        [Required, Display(Name = "Місто")]
        public string City { get; set; }
        [Required, Display(Name = "Номер відділення")]
        public double NumberOfDepartment { get; set; }
    }
}
