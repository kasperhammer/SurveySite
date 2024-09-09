using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SComp
    {
        [Key]
        public int Id { get; set; }
        public int Type { get; set; }

        public string Question { get; set; }

        public List<CompModule> MultiAnwsers { get; set; }
        public List <CompModule> SingleAnwser { get; set; }

       
        public int SurveyId { get; set; }
        [ForeignKey(nameof(SurveyId))]
        public Survey Survey { get; set; }

    

    }

  
}
