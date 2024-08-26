using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UIModels
{
    public class SurveyUI : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Du skal angive et navn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Du skal tilføje et spørgsmål")]
        [MinLength(1, ErrorMessage = "Du skal tilføje et spørgsmål")]
        public List<CompUI> Comps { get; set; }
        [Required(ErrorMessage ="Du skal angive en kode")]
        public string OwnerCode { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Tjekker, om listen 'Comps' er null eller har mindre end 1 element
            // Hvis ja, returnerer den en ValidationResult-fejlmeddelelse
            if (Comps == null || Comps.Count < 1)
            {
                yield return new ValidationResult("You must add at least one component.", new[] { nameof(Comps) });
            }

            // Itererer gennem hver 'Comp' i 'Comps'-listen
            foreach (var comp in Comps)
            {
                // Opretter en liste til at gemme valideringsresultater for den enkelte 'Comp'
                var compValidationResults = new List<ValidationResult>();

                // Opretter en ny valideringskontekst for den aktuelle 'Comp'
                var compContext = new ValidationContext(comp, validationContext, validationContext.Items);

                // Udfører validering af det aktuelle 'Comp'-objekt
                // Hvis valideringen fejler, gemmes fejlene i 'compValidationResults'-listen
                Validator.TryValidateObject(comp, compContext, compValidationResults, true);

                // Itererer gennem alle valideringsresultater for den aktuelle 'Comp'
                foreach (var validationResult in compValidationResults)
                {
                    // Returnerer hvert valideringsresultat, der er fundet
                    yield return validationResult;
                }
            }
        }

    }


}
