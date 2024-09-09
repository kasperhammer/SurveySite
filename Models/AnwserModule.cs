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
        public int SurveyId { get; set; }
        [ForeignKey(nameof(SurveyId))]
        public Survey Survey { get; set;}
        public List<Anwser> anwsers { get; set; }
    }
}
