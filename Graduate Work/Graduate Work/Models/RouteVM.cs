using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Graduate_Work.Models
{
    public class RouteVM
    {
        [Required(ErrorMessage = "Відділення відправлення є пустим")]
        [Display(Name = "Відділення відправлення")]
        public int NumberOfSendingDepartment { get; set; }
        [Required(ErrorMessage = "Відділення прибуття є пустим")]
        [Display(Name = "Відділення прибуття")]
        public int NumberOfReceivingDepartment { get; set; }
        [Required(ErrorMessage = "Вартість є пустою")]
        [Display(Name = "Вартість")]
        public double Cost { get; set; }
        [Required(ErrorMessage = "Час проходження є пустим")]
        [BindProperty, Display(Name = "Час проходження")]
        public TimeSpan Time { get; set; }
    }
}
