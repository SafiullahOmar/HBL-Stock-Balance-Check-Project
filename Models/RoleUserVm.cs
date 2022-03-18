using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class RoleUserVm
    {
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }
    }

    public class Report {
        public int Quantity { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
