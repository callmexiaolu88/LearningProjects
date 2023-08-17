using System;

namespace overwrite
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var t = new C();
            t.Print();
        }
    }

    class A
    {
        public virtual void Print(){
            System.Console.WriteLine("this is A");
        }
    }

    class B:A
    {
        public override void Print(){
            System.Console.WriteLine("this is B");
        }
    }

    class C:B
    {
        public override void Print(){
            base.Print();
        }
    }
}
