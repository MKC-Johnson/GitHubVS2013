using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString("r"));

            //1.首先创建一个作业调度池
            ISchedulerFactory schedf = new StdSchedulerFactory();
            IScheduler sched = schedf.GetScheduler();

            //2.创建出来一个具体的作业
            IJobDetail job = JobBuilder.Create<JobDemo>().Build();

            //NextGivenSecondDate：如果第一个参数为null则表名当前时间往后推迟2秒的时间点。
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(DateTime.Now.AddSeconds(1), 2);
            DateTimeOffset endTime = DateBuilder.NextGivenSecondDate(DateTime.Now.AddHours(2), 3);



            //3.创建并配置一个触发器
            //ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(3).WithRepeatCount(int.MaxValue)).Build();
            //ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create().StartAt(startTime).EndAt(endTime)
            //                                                                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).WithRepeatCount(100))
            //                                                                .Build();
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create().StartAt(startTime).EndAt(endTime)
                                                     .WithCronSchedule("1,10,14 10,20,25,26,33,54 * * * ? ")
                                                     .Build();


            //4.加入作业调度池中
            sched.ScheduleJob(job, trigger);

            //5.开始运行
            sched.Start();
            Console.ReadKey();
            //Add some comments.
        }
    }

    public class JobDemo: IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine(DateTime.Now.ToString("r"));
        }
    }
}
