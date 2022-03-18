using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    
   
    public class Color
    {
        [Required (ErrorMessage ="Please Enter Color Name")]
        [Display(Name ="Color Name (د رنګ نوم)")]
        public string Name { get; set; }
        
        public int  Id { get; set; }
      
        [Required(ErrorMessage = "Please Enter Color Code")]
        [Display(Name = "Color Code ( کوډ)")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please Select  Company Name")]
        [Display(Name = "Company Name ( د شرکت نوم)")]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [Display(Name = "Company Name ( د شرکت نوم)")]
        public Company Company { get; set; }

    }
}
