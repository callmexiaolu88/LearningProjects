using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace quartzLearning
{
    class Program
    {
        private static IScheduler _scheduler { get; set; }
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await InitAndStartScheduler();
            var jobDetail = JobBuilder.
                            Create<TestJob>().
                            UsingJobData("TestName", Guid.NewGuid()).
                            UsingJobData("Count", 1).
                            WithIdentity("t1").
                            WithDescription("this is first name").Build();
            jobDetail.JobDataMap.Add("obj", new TestObj() { Name = "main" });
            var jobStartTrigger = TriggerBuilder.
                            Create().
                            ForJob(jobDetail).
                            WithIdentity("t1").
                            WithDescription("this is first trigger").
                            StartNow().EndAt(DateTimeOffset.Now.AddSeconds(30)).Build();
            var jobEndTrigger = TriggerBuilder.
                            Create().
                            ForJob(jobDetail).
                            WithIdentity("triggerEnd").
                            WithDescription("this is second trigger").
                            StartAt(DateTimeOffset.UtcNow.AddSeconds(10)).Build();
            //await _scheduler.AddJob(jobDetail, false, true);
            var dt = await _scheduler.ScheduleJob(jobDetail, jobStartTrigger);
            var existed = await _scheduler.CheckExists(jobDetail.Key);
            System.Console.WriteLine($"Job before exist: [{existed}]");
            existed = await _scheduler.CheckExists(jobStartTrigger.Key);
            System.Console.WriteLine($"Trigger before exist: [{existed}]");
            Console.WriteLine($"Start Date:{dt.ToLocalTime()}");
            await Task.Delay(10000);
            existed = await _scheduler.CheckExists(jobDetail.Key);
            System.Console.WriteLine($"Job after exist: [{existed}]");
            existed = await _scheduler.CheckExists(jobStartTrigger.Key);
            System.Console.WriteLine($"Trigger after exist: [{existed}]");
            var dtnull = await _scheduler.RescheduleJob(jobEndTrigger.Key, jobEndTrigger);
            Console.WriteLine($"End Date:{dtnull.Value.ToLocalTime()}");
            Console.Read();
        }

        static async Task InitAndStartScheduler()
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await _scheduler.Start();
            return;
        }



    }

    public class TestObj
    {
        public string Name { get; set; }
    }
}
