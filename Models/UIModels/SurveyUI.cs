using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class SurveyUI : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<CompUI> Comps { get; set; }
        public string OwnerCode { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Name is required", new[] { nameof(Name) });
            }

            if (string.IsNullOrEmpty(OwnerCode))
            {
                yield return new ValidationResult("OwnerCode is required", new[] { nameof(OwnerCode) });
            }

            // Tjekker, om listen 'Comps' er null eller har mindre end 1 element
            // Hvis ja, returnerer den en ValidationResult-fejlmeddelelse
            if (Comps == null || Comps.Count < 1)
            {
                yield return new ValidationResult("You must add at least one component.", new[] { nameof(Comps) });
            }

        }

    }


}
