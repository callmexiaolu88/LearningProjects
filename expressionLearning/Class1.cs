using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace expressionLearning
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// 根据权限动态查找
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static IQueryable<object> SelectPermissionDynamic<TSource>(this IQueryable<TSource> sources, IEnumerable<string> permissions)
        {
            var selector = BuildExpression<TSource>(permissions);
            return sources.Select(selector);
        }

        /// <summary>
        /// 构建表达式
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static Expression<Func<TSource, object>> BuildExpression<TSource>(IEnumerable<string> permissions)
        {
            Type sourceType = typeof(TSource);
            Dictionary<string, PropertyInfo> sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop =>
            {
                if (!prop.CanRead) { return false; }
                var perms = prop.GetCustomAttribute<PermissionAttribute>();
                return perms == null || perms.Permissions.Intersect(permissions).Any();
            }).ToDictionary(p => p.Name, p => p);

            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

            ParameterExpression sourceItem = Expression.Parameter(sourceType, "t");
            IEnumerable<MemberBinding> bindings = dynamicType.GetRuntimeProperties().Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name])));

            return Expression.Lambda<Func<TSource, object>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), sourceItem);
        }
    }

    public static class LinqRuntimeTypeBuilder
    {
        private static readonly AssemblyName AssemblyName = new AssemblyName() { Name = "LinqRuntimeTypes4iTheoChan" };
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly Dictionary<string, Type> BuiltTypes = new Dictionary<string, Type>();

        static LinqRuntimeTypeBuilder()
        {
            ModuleBuilder = AssemblyBuilder.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(AssemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            //TODO: optimize the type caching -- if fields are simply reordered, that doesn't mean that they're actually different types, so this needs to be smarter
            string key = string.Empty;
            foreach (var field in fields)
                key += field.Key + ";" + field.Value.Name + ";";

            return key;
        }

        private const MethodAttributes RuntimeGetSetAttrs = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        public static Type BuildDynamicType(Dictionary<string, Type> properties)
        {
            if (null == properties)
                throw new ArgumentNullException(nameof(properties));
            if (0 == properties.Count)
                throw new ArgumentOutOfRangeException(nameof(properties), "fields must have at least 1 field definition");

            try
            {
                // Acquires an exclusive lock on the specified object.
                Monitor.Enter(BuiltTypes);
                string className = GetTypeKey(properties);

                if (BuiltTypes.ContainsKey(className))
                    return BuiltTypes[className];

                TypeBuilder typeBdr = ModuleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                foreach (var prop in properties)
                {
                    var propertyBdr = typeBdr.DefineProperty(name: prop.Key, attributes: PropertyAttributes.None, returnType: prop.Value, parameterTypes: null);
                    var fieldBdr = typeBdr.DefineField("itheofield_" + prop.Key, prop.Value, FieldAttributes.Private);

                    MethodBuilder getMethodBdr = typeBdr.DefineMethod("get_" + prop.Key, RuntimeGetSetAttrs, prop.Value, Type.EmptyTypes);
                    ILGenerator getIL = getMethodBdr.GetILGenerator();
                    getIL.Emit(OpCodes.Ldarg_0);
                    getIL.Emit(OpCodes.Ldfld, fieldBdr);
                    getIL.Emit(OpCodes.Ret);

                    MethodBuilder setMethodBdr = typeBdr.DefineMethod("set_" + prop.Key, RuntimeGetSetAttrs, null, new Type[] { prop.Value });
                    ILGenerator setIL = setMethodBdr.GetILGenerator();
                    setIL.Emit(OpCodes.Ldarg_0);
                    setIL.Emit(OpCodes.Ldarg_1);
                    setIL.Emit(OpCodes.Stfld, fieldBdr);
                    setIL.Emit(OpCodes.Ret);

                    propertyBdr.SetGetMethod(getMethodBdr);
                    propertyBdr.SetSetMethod(setMethodBdr);
                }

                BuiltTypes[className] = typeBdr.CreateType();

                return BuiltTypes[className];
            }
            catch
            {
                throw;
            }
            finally
            {
                Monitor.Exit(BuiltTypes);
            }
        }

        private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
        {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return BuildDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }
    }

    ///<summary>
    /// 访问许可属性
    ///</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class PermissionAttribute : Attribute
    {
        public readonly IEnumerable<string> Permissions;
        public PermissionAttribute(params string[] permissions)
        {
            this.Permissions = permissions.Distinct();
        }
    }
}
