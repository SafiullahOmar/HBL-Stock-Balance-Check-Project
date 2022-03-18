using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class ProductOrOrder
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Color (رنګونه)")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "Please enter Quantity")]
        [Display(Name = "Quantity (تعداد)")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name ="Type Of Order ( دمعاملی ډول)")]
        public int InOut { get; set; }
        [Required]
        [Display(Name = "Bill Number (بیل شمیره)")]
        public string BillNumber { get; set; }
        [Required]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Invoice Date (بیل تاریخ)")]
        public DateTime Date { get; set; }        

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("ColorId")]
        public Color Color { get; set; }
        
        [NotMapped]
        public string Code { get; set; }
    }

    public class OrderVM
    {

        public string Id { get; set; }
        public string BillNumber { get; set; }
        public string Date { get; set; }

        public int InOut
        {
            get; set;
        }   

        public IEnumerable<ProductOrOrder> Details { get; set; }

    }
}
