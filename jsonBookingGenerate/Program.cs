using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace jsonBookingGenerate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime DATETIME_1970 = new DateTime(1970, 1, 1);
            List<IModule> modules = Assembly.GetEntryAssembly().GetTypes().
                Where(i => !i.IsAbstract && typeof(AbstractModule).IsAssignableFrom(i)).
                Select(i => (IModule)Activator.CreateInstance(i)).ToList();
            modules.Sort((x, y) => x.ModuleType.CompareTo(y.ModuleType));
            while (true)
            {
                Console.Write(new string('-', 10));
                Console.Write($"Input Booking Type: {modules.Select(m => $"[{m.Index}->{m.Name}]").Aggregate((s1, s2) => $"{s1} | {s2}")}");
                Console.WriteLine(new string('-', 10));

                var inputData = Helper.GetInput();
                var module = modules.FirstOrDefault(m => m.Index.ToString() == inputData);
                if (module != null)
                {
                    Console.WriteLine(module.Name);
                    Console.Write(new string('-', 10));
                    Console.Write($"Input Operation Type: [0->Add]  [1->Delete]  [2->Update]");
                    Console.WriteLine(new string('-', 10));
                    inputData = Helper.GetInput();
                    var operate = inputData switch
                    {
                        "0" => EnumOperationType.Add,
                        "1" => EnumOperationType.Delete,
                        "2" => EnumOperationType.Update,
                        _ => EnumOperationType.Unkow
                    };
                    if (operate != EnumOperationType.Unkow)
                    {
                        Console.WriteLine(operate.ToString());
                        Console.Write(new string('-', 10));
                        Console.Write("Output result:");
                        Console.WriteLine(new string('-', 10));

                        try
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(new
                            {
                                OperationType = 10005,
                                CategoryId = 0x80524900,
                                ContentType = "application/json",
                                ClientRequestId = Guid.NewGuid().ToString(),
                                Data = JsonConvert.SerializeObject(new
                                {
                                    operation = (int)operate,
                                    eventId = Guid.NewGuid().ToString(),
                                    title = $"Operate {module.Name}",
                                    parentEventId = (string)null,
                                    parameters = module.Excute(operate),
                                    startTime = $"{(ulong)DateTime.UtcNow.AddSeconds(20).Subtract(DATETIME_1970).TotalMilliseconds}",
                                    endTime = $"{(ulong)DateTime.UtcNow.AddMinutes(30).Subtract(DATETIME_1970).TotalMilliseconds}",
                                    scheduleType = module.Index
                                })
                            }, Formatting.Indented));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Typed operation type incorrect.");
                    }
                }
                else
                {
                    Console.WriteLine("Typed Booking type incorrect.");
                }
            }
        }
    }
}