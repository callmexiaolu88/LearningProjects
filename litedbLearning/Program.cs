using System;
using LiteDB;
using Newtonsoft.Json;

namespace litedbLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var db = new LiteDatabase("test.db", null);
            var collection = db.GetCollection<MasterClass>();
            for (int i = 10; i >= 0 ; i--)
            {
                var obj = new MasterClass
                {
                    Id=Guid.NewGuid(),
                    Name = "master" + i,
                    Slave = new SlaveClass
                    {
                        Name = "slave" + i
                    }
                };
                collection.Insert(obj);
            }

            Console.WriteLine(JsonConvert.SerializeObject(collection.FindAll()));
        }
    }

    class MasterClass
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public SlaveClass Slave{ get; set; }
    }

    class SlaveClass
    {
        public string Name { get; set; }
    }

}
