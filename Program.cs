using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using tiefebot.Data.Command;
using tiefebot.Data.Function;

namespace tiefebot;

class Program
{
    public static DiscordClient Client { get; private set; }
    
    static async Task Main()
    {
        var config = new DiscordConfiguration()
        {
            Intents = DiscordIntents.All,
            Token = "MTMxODgyNTc3MDI5NDE4NjAyNA.G0to9j.Ft4ukd1J_5dMf837_tcCWp7YEKTZyvzYsQNqzU",
            TokenType = TokenType.Bot,
            AutoReconnect = true,
        };
        Client = new DiscordClient(config);
        Client.Ready += OnClientReady;

        var slashCommandsConfiguration = Client.UseSlashCommands();
        slashCommandsConfiguration.RegisterCommands<TestSc>();
        
        SchedulerService schedulerService = new SchedulerService();
        await schedulerService.StartScheduler();
        
        
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
    {
        return Task.CompletedTask;
    }
}

