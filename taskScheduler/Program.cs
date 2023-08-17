// See https://aka.ms/new-console-template for more information
using taskScheduler;

Console.WriteLine("Hello, World!");

var taskScheduler = new CustomTaskScheduler();
var tasks = new List<Task>();
for (int i = 0; i < 500; i++)
{
    var index = i;
    tasks.Add(taskScheduler.Run(() =>
      {
          Thread.Sleep(10000);
          System.Console.WriteLine($"This is Thread [{index}]");
      }));
}

Console.Read();