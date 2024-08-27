using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class AnwserModuleUI
    {
        public int Id { get; set; }
        [Required ]
        public string Text { get; set; }
        public bool Selected { get; set; }
        public int? CompSingleId { get; set; }
        public int? CompMultiId { get; set; }
     
    }
}
