using System;
using System.Threading.Tasks;
using Quartz;
using static quartzLearning.Program;

namespace quartzLearning
{
    public class TestJob : IJob
    {
        public int Count { get; set; }
        public string TestName { get; set; }
        public object Obj{ get; set; }

        public Task Execute(IJobExecutionContext context)
        {
            if (Obj  is TestObj obj)
            {
                Console.WriteLine($"Name:[{obj?.Name}]");
                Console.WriteLine($"Count:[{Count}]");
                Console.WriteLine($"Count:[{TestName}]");
                obj.Name = "this is first calling";
                Count = 100;
                TestName = Guid.NewGuid().ToString();
            }
            return Task.CompletedTask;
        }
    }
}