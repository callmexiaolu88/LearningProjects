using System;
using System.Collections.Concurrent;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;

namespace fluentValidationLearning
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        readonly ConcurrentDictionary<Type, Lazy<IValidator>> _validators;

        #region Singleton

        public static ValidatorFactory Instance { get; } = new ValidatorFactory();

        #endregion

        #region Constructor

        private ValidatorFactory()
        {
            _validators = new ConcurrentDictionary<Type, Lazy<IValidator>>();
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly()).
                ForEach(asr => _validators.TryAdd(asr.InterfaceType, new Lazy<IValidator>(() => (IValidator)Activator.CreateInstance(asr.ValidatorType))));
            _validators.TryAdd(typeof(NotexistentValidator), new Lazy<IValidator>(new NotexistentValidator()));
        }

        #endregion

        #region ValidatorFactoryBase

        public override IValidator CreateInstance(Type validatorType)
        {
            return _validators.TryGetValue(validatorType, out Lazy<IValidator> validator) ? validator.Value :
                _validators.TryGetValue(typeof(NotexistentValidator), out validator) ? validator.Value :
                new NotexistentValidator();
        }

        #endregion

        class NotexistentValidator : AbstractValidator<object>
        {
            protected override bool PreValidate(ValidationContext<object> context, ValidationResult result)
            {
                var type = context.InstanceToValidate?.GetType().Name;
                if (type != null)
                    result.Errors.Add(new ValidationFailure("", $"Doesn't exist any validator for [{type}]"));
                else
                    result.Errors.Add(new ValidationFailure("", "The object that need to be validated is null and doesn't exist any validator for it"));
                return false;
            }
        }
    }
}