using System.Linq;
using System;
using System.Text.Json;
using Nett;

namespace tomlSerilizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceList = LoadServiceList("cloudconfig.tml");
            Console.WriteLine(JsonSerializer.Serialize(serviceList));
        }

        public static void SaveServiceList(TVUCloudServiceList cloudServiceList, string filePath)
        {
            try
            {
                Toml.WriteFile<TVUCloudServiceList>(cloudServiceList, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SaveServiceList() Exception {ex}");
            }
        }

        public static TVUCloudServiceList LoadServiceList(string filePath)
        {
            TVUCloudServiceList ret = null;
            try
            {
                ret = Toml.ReadFile<TVUCloudServiceList>(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadServiceList() message {ex}");
            }

            if (ret == null)
                ret = new TVUCloudServiceList();
            ret.Adjust();
            return ret;
        }
    }
}
