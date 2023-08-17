using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace dynamicPropertyAccessor
{
    public static class ModuleConfigHelper
    {
        private class ConfigDescriptor
        {
            private readonly Dictionary<string, ConfigItemDescriptor> _descriptors;

            public Type Type { get; }

            public IReadOnlyDictionary<string, ConfigItemDescriptor> Descriptors { get; }

            public ConfigDescriptor(Type type)
            {
                if (type is null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                _descriptors = new Dictionary<string, ConfigItemDescriptor>();
                Type = type;
                Descriptors = new ReadOnlyDictionary<string, ConfigItemDescriptor>(_descriptors);
                Load();
            }

            public void Load()
            {
                var properties = Type.GetProperties();
                foreach (var info in properties)
                {
                    var itemDescriptor = new ConfigItemDescriptor(Type, info);
                    _descriptors.Add(info.Name, itemDescriptor);
                }
            }

            public bool Contains(string item)
            {
                if (item.NotNullOrWhiteSpace())
                {
                    return _descriptors.ContainsKey(item);
                }
                return false;
            }

            public bool TryGet(string item, out ConfigItemDescriptor itemDescriptor)
            {
                if (item.NotNullOrWhiteSpace())
                {
                    return _descriptors.TryGetValue(item, out itemDescriptor);
                }
                else
                {
                    itemDescriptor = null;
                    return false;
                }
            }
        }

        private class ConfigItemDescriptor
        {
            public string Name => PropertyInfo.Name;
            public Type Type => PropertyInfo.PropertyType;
            public bool CanWrite { get; }
            public bool CanRead { get; }
            public PropertyInfo PropertyInfo { get; }
            public Type ClassType { get; }

            public Delegate Getter { get; private set; }

            public Delegate Setter { get; private set; }

            public ConfigItemDescriptor(Type classType, PropertyInfo propertyInfo)
            {
                PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
                ClassType = classType ?? throw new ArgumentNullException(nameof(classType));
                CanRead = PropertyInfo.GetGetMethod() != null;
                CanWrite = PropertyInfo.GetSetMethod() != null;
                setGetter();
                setSetter();
            }

            private void setGetter()
            {
                var parameter = Expression.Parameter(ClassType);
                Expression body;
                if (CanRead)
                    body = Expression.Property(parameter, PropertyInfo);
                else
                    body = Expression.Throw(Expression.Constant(new InvalidOperationException("Can not read this property.")));
                Getter = Expression.Lambda(body, parameter).Compile();
            }

            private void setSetter()
            {
                var instance = Expression.Parameter(ClassType);
                var value = Expression.Parameter(Type);
                Expression body;
                if (CanWrite)
                {
                    var property = Expression.Property(instance, PropertyInfo);
                    body = Expression.Assign(property, value);
                }
                else
                {
                    body = Expression.Throw(Expression.Constant(new InvalidOperationException("Can not write this property.")));
                }
                Setter = Expression.Lambda(body, instance, value).Compile();
            }
        }

        private static readonly ConcurrentDictionary<Type, ConfigDescriptor> _descriptors = new ConcurrentDictionary<Type, ConfigDescriptor>();

        private static ConfigDescriptor getDescriptor(Type configType)
        {
            if (configType != null)
                return _descriptors.GetOrAdd(configType, type => new ConfigDescriptor(type));
            return null;
        }

        public static Type GetItemType(Type type, string item)
        {
            if (getDescriptor(type)?.TryGet(item, out ConfigItemDescriptor itemDescriptor) == true)
                return itemDescriptor.Type;
            return null;
        }

        public static Type GetItemType<T>(string item)
            => GetItemType(typeof(T), item);

        public static bool TryGetItem(object instance, string item, out object value)
        {
            value = null;
            if (instance != null && item.NotNullOrWhiteSpace())
            {
                if (getDescriptor(instance?.GetType())?.TryGet(item, out ConfigItemDescriptor itemDescriptor) == true)
                {
                    if (itemDescriptor.CanRead)
                    {
                        value = itemDescriptor.Getter.DynamicInvoke(instance);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool SetItem(object instance, string item, object value)
        {
            if (instance != null && item.NotNullOrWhiteSpace())
            {
                if (getDescriptor(instance?.GetType())?.TryGet(item, out ConfigItemDescriptor itemDescriptor) == true)
                {
                    if (itemDescriptor.CanWrite)
                    {
                        itemDescriptor.Setter.DynamicInvoke(instance, value);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}