using System;
using System.Collections.Generic;
using System.Threading;

namespace ILGeneratorLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var func = Generator.BuildType();
            var result= func("123", "456");
            Console.WriteLine(result);
            var type = Generator.CreateType();
            dynamic dyobj = Activator.CreateInstance(type);
            Console.WriteLine(dyobj.Test);
            var dt= dyobj.Concast();
            Console.WriteLine(dt);
            var obj = Activator.CreateInstance(type);
            Console.WriteLine(type.GetField("Test").GetValue(obj));
            var cwmethodInfo = type.GetMethod("Concast");
            cwmethodInfo.Invoke(obj, null);
            Console.Read();
        }

        class T
        {
            public int Test;
        }

        //public static object ResolveService(ILEmitResolverBuilderRuntimeContext context, ServiceProviderEngineScope scope)
        //{
        //    IDictionary<object, object> resolvedServicesLocal;
        //    bool lockTakenLocal;
        //    try
        //    {
        //        var value = scope.ResolvedServices; //Dictionary<object, object>
        //        var value1 = value;
        //        resolvedServicesLocal = value1;
        //        lockTakenLocal = false;
        //        Monitor.Enter(resolvedServicesLocal, out lockTakenLocal);
        //        ServiceType resultLocal;
        //        object cacheKeyLocal;
        //        var vaule2 = resolvedServicesLocal;
        //        resolvedServicesLocal.tr
        //    }

        //}

    }
}
