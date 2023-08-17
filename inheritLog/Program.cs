using System.Diagnostics;
using System;
using NLog;

namespace inheritLog
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DerivedClass derivedClass = new DerivedClass();
            derivedClass.Print();
            derivedClass.Print();
        }
    }

    public abstract class BasicClass
    {
        protected virtual Logger logger { get; } = LogManager.GetCurrentClassLogger();
         public BasicClass()
         {
            StackFrame stackFrame = new StackFrame(false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
         }


        public virtual void Print()
        {
            StackFrame stackFrame = new StackFrame(2, false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
            System.Console.WriteLine(logger.Name);
            logger.Info("this is basic class");
        }

    }

    public class DerivedClass1 : BasicClass
    {
        public override void Print()
        {
            base.Print();
            StackFrame stackFrame = new StackFrame(2, false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
            System.Console.WriteLine(logger.Name);
            logger.Info("this is DerivedClass 1 class");
        }
    }

    public class DerivedClass2 : DerivedClass1
    {
        public override void Print()
        {
            base.Print();
            StackFrame stackFrame = new StackFrame(2, false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
            System.Console.WriteLine(logger.Name);
            logger.Info("this is DerivedClass 2 class");
        }
    }

    public class DerivedClass3 : DerivedClass2
    {
        public override void Print()
        {
            base.Print();
            StackFrame stackFrame = new StackFrame(2, false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
            System.Console.WriteLine(logger.Name);
            logger.Info("this is DerivedClass 3 class");
        }
    }


    public class DerivedClass : DerivedClass3
    {
        public override void Print()
        {
            base.Print();
            StackFrame stackFrame = new StackFrame(2, false);
            System.Console.WriteLine($"method: [{stackFrame.GetMethod()}]");
            System.Console.WriteLine($"stackFrame: [{stackFrame}]");
            System.Console.WriteLine(logger.Name);
            logger.Info("this is derived class");
        }
    }


}
