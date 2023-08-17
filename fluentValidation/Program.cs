using System;
using FluentValidation;

namespace fluentValidationLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var person = new Person();
            Person nullPerson = null;
            var result = ValidatorFactory.Instance.GetValidator<Person>().Validate(person);
            //var result = ValidatorFactory.Instance.GetValidator<Person>().Validate(nullPerson);
            if (result.IsValid)
            {
                System.Console.WriteLine("Excuted");
            }
            else
            {
                System.Console.WriteLine(result.ToString());
            }

            System.Console.WriteLine(int.TryParse(null, out int a));
            System.Console.WriteLine(a);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public string Port { get; set; }
        public string Email { get; set; } = "1212";
    }
}
