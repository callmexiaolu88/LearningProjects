using System.Security.Cryptography.X509Certificates;
using System;
using System.Diagnostics;

namespace msbuild
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            #if DEBUG
            Console.WriteLine("This is DEBUG!");
            #endif

            #if TRACE
            Console.WriteLine("This is TRACE!");
            #endif

            #if TEST1
            Console.WriteLine("This is TEST1!");
            #endif

            #if TEST2
            Console.WriteLine("This is TEST2!");
            #endif

            #if TEST3
            Console.WriteLine("This is TEST3!");
            #endif

            #if NETCOREAPP3_1
            Console.WriteLine("This is NETCOREAPP3_1!");
#endif

            var frame = new TestStackFrame();
            var methods= frame.Frame.GetMethod();
            System.Console.WriteLine(frame.Frame);

            Console.Read();
        }
    }

    class TestStackFrame
    {
        public StackFrame Frame { get; } = new StackFrame(1, fNeedFileInfo: false);
    }
}
