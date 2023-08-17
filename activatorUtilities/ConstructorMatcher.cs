using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace activatorUtilities
{
    public struct ConstructorMatcher
    {
        private readonly ConstructorInfo _constructor;
        private readonly ParameterInfo[] _parameters;
        private readonly object?[] _parameterValues;
        public ConstructorMatcher(ConstructorInfo constructor)
        {
            _constructor = constructor;
            _parameters = _constructor.GetParameters();
            _parameterValues = new object?[_parameters.Length];
        }
        public int Match(object[] givenParameters)
        {
            int applyIndexStart = 0;
            int applyExactLength = 0;
            for (int givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
            {
                Type? givenType = givenParameters[givenIndex]?.GetType();
                bool givenMatched = false;
                for (int applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
                {
                    if (_parameterValues[applyIndex] == null &&
                        _parameters[applyIndex].ParameterType.IsAssignableFrom(givenType))
                    {
                        givenMatched = true;
                        _parameterValues[applyIndex] = givenParameters[givenIndex];
                        if (applyIndexStart == applyIndex)
                        {
                            applyIndexStart++;
                            if (applyIndex == givenIndex)
                            {
                                applyExactLength = applyIndex;
                            }
                        }
                    }
                }
                if (givenMatched == false)
                {
                    return -1;
                }
            }
            return applyExactLength;
        }
        public object CreateInstance(IServiceProvider provider)
        {
            for (int index = 0; index != _parameters.Length; index++)
            {
                if (_parameterValues[index] == null)
                {
                    object? value = provider.GetService(_parameters[index].ParameterType);
                    if (value == null)
                    {
                        // if (!ParameterDefaultValue.TryGetDefaultValue(_parameters[index], out object? defaultValue))
                        // {
                        //     throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                        // }
                        // else
                        // {
                        //     _parameterValues[index] = defaultValue;
                        // }
                    }
                    else
                    {
                        _parameterValues[index] = value;
                    }
                }
            }
#if NETFRAMEWORK || NETSTANDARD2_0
                try
                {
                    return _constructor.Invoke(_parameterValues);
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    // The above line will always throw, but the compiler requires we throw explicitly.
                    throw;
                }
#else
            return _constructor.Invoke(BindingFlags.DoNotWrapExceptions, binder: null, parameters: _parameterValues, culture: null);
#endif
        }
    }
}