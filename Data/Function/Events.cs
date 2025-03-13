using Quartz;

namespace tiefebot.Data.Function;

public class Events : IJob
{
    //DO: make events or at least example
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Задача выполнена " + DateTime.Now);
        return Task.CompletedTask;
    }
}