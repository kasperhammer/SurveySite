using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SComp> SComps { get; set; }
        public string OwnerCode { get; set; } = string.Empty;
        public bool SurveyAnwser { get; set; }
        public int OriginId { get; set; }
        
    }
}
