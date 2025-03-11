using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Command;

public class TestSc : ApplicationCommandModule
{
    [SlashCommand("test", "test command")]
    public async Task Test(InteractionContext ctx)
    {

        await using DataBase db = new DataBase();

        Character character = new()
        {
            MemberDiscordId = ctx.User.Id,
            Name = "Test"

        };
        
        await db.Characters.AddAsync(character);
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
}