using System;
using System.Reflection;
using System.Threading.Tasks;

namespace reflections
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var fieldInfo=typeof(Task).GetField("m_action", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Console.WriteLine(fieldInfo);
        }
    }
}
