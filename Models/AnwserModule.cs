using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AnwserModule
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }

        public int? CompSingleId { get; set; }
        [ForeignKey(nameof(CompSingleId))]
        public SComp CompSingle { get; set; }

        public int? CompMultiId { get; set; }
        [ForeignKey(nameof(CompMultiId))]
        public SComp CompMulti { get; set; }

      
    }
}
