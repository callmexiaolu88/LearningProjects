using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        public static Program Instance => new Program();
        public static Program Instance2 { get; } = new Program();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(TestMessage.Instance.Id);
            Console.WriteLine(TestMessage.Instance.Id);
            Console.WriteLine(TestMessage.Instance2.Id);
            Console.WriteLine(TestMessage.Instance2.Id);

            List<int> ls = new List<int> { 4, 1, 2, 65, 23, 7, 5 };
            ls.Sort((a, b) =>
            {
                if (a == 65)
                {
                    return -1;
                }
                return a.CompareTo(b);
            });
            System.Console.WriteLine(new StackTrace().GetFrame(1));


            List<IB> lsobj = new List<IB> { new Subclass() };
            lsobj.First().B();
            var a=lsobj.OfType<IA>();
            a.First().A();
        }
    }
}

public class TestMessage
{
    public static TestMessage Instance => new TestMessage();
    public static TestMessage Instance2 { get; } = new TestMessage();

    public int Id { get; } = new Random().Next();

    public override string ToString()
    {
        return Id.ToString();
    }
}