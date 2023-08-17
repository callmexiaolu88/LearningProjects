using System;
using System.Text.Json;

namespace jsonSerializerLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = null;
            if (str.IsNull())
            {
                var url = new Uri("https://pp-va-001.s3.amazonaws.com/362881b36f554796a15f43aadb877f4b/recordings/BreakingLiveSource/0000-Univision-S1-SDI_Replay_20211019_065413/play.mp4");
                Console.WriteLine("Hello World!");

            }
            Console.WriteLine("Hello World!");
            var data = new PeopleReponse<People>()
            {
                ID = "98765",
                Data = new People
                {
                    Name = new Name
                    {
                        FirstName = "Yulong",
                        LastName = "Lu"
                    },
                    Age = 18
                }
            };

            var jsonStr = JsonSerializer.Serialize(data);
            var obj = JsonSerializer.Deserialize<PeopleReponse<JsonElement>>(jsonStr);
            foreach (var i in obj.Data.EnumerateObject())
            {
                System.Console.WriteLine(i.Value);
            }
        }
    }

    class People
    {
        public Name Name { get; set; }
        public int Age { get; set; }
    }

    class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class PeopleReponse<T>
    {
        public T Data { get; set; }
        public string ID { get; set; }
    }

}
