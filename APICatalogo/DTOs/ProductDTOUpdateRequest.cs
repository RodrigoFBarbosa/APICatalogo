using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "O estoque deve estar entre 1 e 9999.")]
    public float Inventory { get; set; }
    public DateTime RegistrationDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RegistrationDate <= DateTime.Now.Date)
        {
            yield return new ValidationResult("A data deve ser maior que a data atual",
                new[] {nameof(this.RegistrationDate)});
            
        }
    }
}
