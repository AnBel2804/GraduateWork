using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("Packages")]
    public class Package
    {
        [Key]
        public int PackageId { get; set; }
        [ForeignKey("SenderInfoId")]
        [ValidateNever]
        public SenderInfo SenderInfo { get; set; }
        [ForeignKey("ReciverInfoId")]
        [ValidateNever]
        public ReciverInfo ReciverInfo { get; set; }
        [ForeignKey("PackageTypeID")]
        [ValidateNever, Required]
        public PackageType PackageType { get; set; }
        [ValidateNever]
        [Display(Name = "Статус")]
        public string Status { get; set; }
        [BindProperty, Display(Name = "Орієнтована дата доставки")]
        [ValidateNever]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }
        [Display(Name = "Сума до сплати")]
        [ValidateNever]
        public double Cost { get; set; }
    }
}
