using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class CompUI
    {
        public int Id { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public string Question { get; set; }
     
        public List<AnwserModuleUI> MultiAnwsers { get; set; }
        public string TextAnwser { get; set; }
        public List<AnwserModuleUI> SingleAnwser { get; set; }

        [Required]
        public int SurveyId { get; set; }

    }
}
