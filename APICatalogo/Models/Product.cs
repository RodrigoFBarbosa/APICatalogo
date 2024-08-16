using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Product : IValidatableObject
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(80)]
    //[FirstLetterUpperCase] // validação personalizada
    public string? Name { get; set; }

    [Required]
    [StringLength(300)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName="decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public float Inventory { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(this.Name))
        {
            var firstLetter = this.Name[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                yield return new ValidationResult("A primeira letra do produto deve ser maiúscula", new[] { nameof(this.Name) });
            }
        }

        if(this.Inventory <= 0)
        {
            yield return new ValidationResult("O estoque deve ser maior que zero", [nameof(this.Inventory)]);
        }
    }
}
