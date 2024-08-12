using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class FirstLetterUpperCaseAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();
            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("A primeira letra deve ser maiúscula");
            }

            return ValidationResult.Success;
        }

    }
}
