using System;

namespace implicitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Exception exception = new Exception("this is a exception");
            var name = Cast<Name>(exception);
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(name));
            var str = Cast<string>(exception);
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(str));
        }

        static T Cast<T>(Exception exception)
        {
            dynamic dy = exception;
            return exception switch
            {
                T t => t,
                _ => (T)dy
            };
        }

    }

    class Name
    {
        public string Message { get; set; }

        public static explicit operator Name(Exception exception){
            return new Name { Message = exception.Message };
        }

    }
}
