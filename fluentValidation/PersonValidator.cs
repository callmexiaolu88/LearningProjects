using FluentValidation;

namespace fluentValidationLearning
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Id).GreaterThan(10);
            RuleFor(p => p.Port).Transform(value => int.TryParse(value, out int ret) ? (int?)ret : null).
                NotEmpty().InclusiveBetween(0, 65535);
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
        }
    }
}