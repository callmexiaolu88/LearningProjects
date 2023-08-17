using System;
using Itenso.TimePeriod;

namespace timeperiodlibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DateTime dt = DateTime.Now;
            var tr1 = new TimeRange(dt, TimeSpan.FromHours(1));
            var tr2 = new TimeRange(dt.AddHours(0.5));
            System.Console.WriteLine(tr1.GetRelation(tr2));
        }
    }
}
