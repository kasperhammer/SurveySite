using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.UIModels
{
    public class AnwserModuleUI 
    {
       
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public List<AnwserUI> anwsers { get; set; } = new List<AnwserUI>();
        
    }
}
