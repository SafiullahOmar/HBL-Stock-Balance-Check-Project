using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class ColorBox
    {
        public int Id { get; set; }
        public string Bill { get; set; }
        public DateTime? Date { get; set; }

        public int  InOut { get; set; }

        public int ColorId { get; set   ; }
        [ForeignKey("ColorId")]
        public Color color { get; set; }

        public Guid UserId { get; set; }
        public DateTime EntryDate { get; set; }

    }
}
