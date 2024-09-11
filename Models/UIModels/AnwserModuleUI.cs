using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class AnwserModuleUI
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public List<AnwserUI> anwsers { get; set; }
    }
}
