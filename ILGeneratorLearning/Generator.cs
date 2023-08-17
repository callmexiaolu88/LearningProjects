using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ILGeneratorLearning
{
    class Generator
    {
        public static Func<string, string, string> BuildType()
        {
            var dynamicMethod = new DynamicMethod("DynamicTest",
                MethodAttributes.Public | MethodAttributes.Static,
                CallingConventions.Standard,
                typeof(string),
                new[] { typeof(string), typeof(string) },
                typeof(Generator), true);

            //string.Concat("asda", "asdas");
            var concatInfo = typeof(string).GetMethod("Concat", types: new[] { typeof(string), typeof(string) });

            var iLGenerator = dynamicMethod.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Call, concatInfo);
            //var builder= iLGenerator.DeclareLocal(typeof(string));
            //var builder2 = iLGenerator.DeclareLocal(typeof(string));
            //iLGenerator.Emit(OpCodes.Stloc, builder);
            //iLGenerator.Emit(OpCodes.Ldloc, builder2);
            iLGenerator.Emit(OpCodes.Ret);
            var func = (Func<string, string, string>)dynamicMethod.CreateDelegate(typeof(Func<string, string, string>));
            return func;
        }

        public static Type CreateType()
        {
            var assemblyName = new AssemblyName("CustomAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("CustomAssembly.dll");
            var typeBuilder = moduleBuilder.DefineType("CustomClass",TypeAttributes.Class|TypeAttributes.Public);
            typeBuilder.DefineField("Test", typeof(int), FieldAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod("Concast", MethodAttributes.Public, typeof(DateTime), null);
            var ilGenerator = methodBuilder.GetILGenerator();
            var methodInfo = typeof(DateTime).GetProperty("Now").GetMethod;
            var cwmethodInfo = typeof(Console).GetMethod("WriteLine", new[] { typeof(object) });
            var dt = ilGenerator.DeclareLocal(typeof(DateTime));
            ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
            ilGenerator.Emit(OpCodes.Stloc, dt);
            ilGenerator.Emit(OpCodes.Ldloc, dt);
            ilGenerator.Emit(OpCodes.Box, typeof(DateTime));
            ilGenerator.Emit(OpCodes.Call, cwmethodInfo);
            ilGenerator.Emit(OpCodes.Ldloc, dt);
            ilGenerator.Emit(OpCodes.Ret);
            var type= typeBuilder.CreateType();
            return type;
        }

        public static Type CreateDynamicType()
        {
            var assemblyName = new AssemblyName("CustomAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("CustomAssembly.dll");
            var typeBuilder = moduleBuilder.DefineType("CustomClass");
            var methodBuilder = typeBuilder.DefineMethod("Concast", MethodAttributes.Public, CallingConventions.Standard, null, null);
            var ilGenerator = methodBuilder.GetILGenerator();
            var methodInfo = typeof(DateTime).GetProperty("Now").GetMethod;
            var cwmethodInfo = typeof(Console).GetMethod("WriteLine", new[] { typeof(object) });
            var dt = ilGenerator.DeclareLocal(typeof(int));
            //ilGenerator.Emit(OpCodes.Call, methodInfo);
            ilGenerator.Emit(OpCodes.Ldloc, dt);
            ilGenerator.Emit(OpCodes.Initobj, typeof(int));
            ilGenerator.Emit(OpCodes.Ldc_I4_8);
            ilGenerator.Emit(OpCodes.Stloc, dt);
            //ilGenerator.Emit(OpCodes.Ldloc, dt);
            //ilGenerator.Emit(OpCodes.Box, typeof(int));
            //ilGenerator.Emit(OpCodes.Call, cwmethodInfo);
            ilGenerator.EmitWriteLine(dt);
            ilGenerator.Emit(OpCodes.Ret);
            var type = typeBuilder.CreateType();
            return type;
        }

        public DateTime Test()
        {
            DateTime dt;
            dt = DateTime.Now;
            Console.WriteLine(dt);
            return dt;
        }

    }
}
