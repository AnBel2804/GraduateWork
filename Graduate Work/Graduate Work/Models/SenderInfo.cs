using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduate_Work.Models
{
    [Table("SenderInfos")]
    public class SenderInfo
    {
        [Key]
        public int SenderInfoId { get; set; }
        [ForeignKey("DepartmentOfSenderID")]
        [ValidateNever]
        public Department DepartmentOfSender { get; set; }
        [ForeignKey("SenderID")]
        [ValidateNever]
        public Customer Sender { get; set; }
    }
}
