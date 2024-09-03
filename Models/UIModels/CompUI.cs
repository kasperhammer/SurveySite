﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class CompUI : IValidatableObject
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


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {


            // Check if Type is 1
            if (Type == 1)
            {
                // Validate that MultiAnwsers has at least 2 items
                if (MultiAnwsers == null || MultiAnwsers.Count < 2)
                {
                    yield return new ValidationResult(
                        "There must be at least 2 answers in MultiAnwsers when Type is 1.",
                        new[] { nameof(MultiAnwsers) }
                    );
                }
            }

        }
    }
}
