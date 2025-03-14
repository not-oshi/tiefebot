using Quartz;
using Quartz.Impl;

namespace tiefebot.Data.Function;

public class SchedulerService
{
    public async Task StartScheduler()
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<Events>()
            .WithIdentity("Events", "general")
            .Build();
        
        // To make interval - .WithIntervalInHours(4).RepeatForever() // Interval 4 hours
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("EventsTrigger", "general")
            .StartNow()
            .WithDailyTimeIntervalSchedule(x => x
                .InTimeZone(TimeZoneInfo.Utc)    
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 0)))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

}