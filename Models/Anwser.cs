using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Anwser
    {
        [Key]
        public int Id { get; set; }
        public string AnwserText { get; set; }  
        public int CompId { get; set; }
        [ForeignKey(nameof(CompId))]
        public SComp Comp { get; set; }
        public int AnwserId { get; set;}
        [ForeignKey(nameof(AnwserId))]
        public AnwserModule AnwserModule { get; set; }

    }
}
