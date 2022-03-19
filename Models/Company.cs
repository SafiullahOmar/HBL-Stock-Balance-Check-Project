using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class Company
    {
        [Required(ErrorMessage = "Please Enter Company Name")]
        [Display(Name = "Company Name ( د شرکت نوم)")]
        public string Name { get; set; }

        public int Id { get; set; }

        public ICollection<Color> Colors { get; set; }
    }
}
