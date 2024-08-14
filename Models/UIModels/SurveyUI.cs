using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class SurveyUI
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "The Comps list must contain at least one item.")]
        public List<CompUI> Comps { get; set; }
        [Required]
        public string OwnerCode { get; set; } = string.Empty;

    }
}
