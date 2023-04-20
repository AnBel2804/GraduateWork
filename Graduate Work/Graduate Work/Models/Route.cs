using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Graduate_Work.Models
{
    [Table("Routes")]
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        [ValidateNever]
        public ICollection<Department> Departments { get; set; }
        [Required, Display(Name = "Вартість")]
        public double Cost { get; set; }
        [BindProperty, Display(Name = "Час проходження")]
        public TimeSpan Time { get; set; }
        public Route()
        {
            Departments = new List<Department>();
        }
    }
}