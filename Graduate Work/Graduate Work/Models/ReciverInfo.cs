using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("ReciverInfos")]
    public class ReciverInfo
    {
        [Key]
        public int ReciverInfoId { get; set; }
        [ForeignKey("DepartmentOfReciverID")]
        [ValidateNever, Required]
        public Department DepartmentOfReciver { get; set; }
        [ForeignKey("ReciverID")]
        [ValidateNever, Required]
        public Customer Reciver { get; set; }
    }
}
