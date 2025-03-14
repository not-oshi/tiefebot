using DSharpPlus;
using Quartz;

namespace tiefebot.Data.Function;

public class Events : IJob
{
    public int MalfunctionCounter = 0;

    private readonly string[] _rooms = { };
    private readonly string[] _malfunctions = { };
    private readonly Random _random = new Random();
    
    //DO: make events or at least example
    
    public async Task Execute(IJobExecutionContext context)
    {
        int index1 = _random.Next(_rooms.Length);
        int index2 = _random.Next(_malfunctions.Length);

        var channel = Program.Client.GetChannelAsync(333333333);
        await Program.Client.SendMessageAsync(channel.Result, "test");
        
        Console.WriteLine("Задача выполнена " + DateTime.Now);
    }
}