using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using tiefebot.Data.Entity;
using tiefebot.Data.Function;

namespace tiefebot.Data.Command;

public class TestSc : ApplicationCommandModule
{
    
    [SlashCommand("whoareyou", "Will Send a full name of this Replica")]
    public async Task WhoAreYou(InteractionContext ctx)
    {
        await ctx.Interaction.CreateResponseAsync
        (InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .WithContent("Ich bin Verwaltung-, Interaktion-, Daten-Replica"));

    }
    
    [SlashCommand("checktest", "1")]
    public async Task checktest(InteractionContext ctx,
        [Autocomplete(typeof(CharacterCheck)), Option("test", "test", true)] string id)
    {
        await ctx.Interaction.CreateResponseAsync
        (InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .WithContent("showing"));

    }
    
    [SlashCommand("dice_test", "Check dice system")]
    public async Task CheckTest(InteractionContext ctx,
        [Option("Stat_Num", "Just a test")] long num)
    {
        await ctx.DeferAsync();
        DiscordEmbed embed = await DiceCheck.CheckDice(num);
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
    }    
    
    
    [SlashCommand("test2", "test command")]
    public async Task Test2(InteractionContext ctx)
    {

        await using DataBase db = new DataBase();

        Item item = new()
        {
            Name = "TestItem"
        };
        
        await db.Items.AddAsync(item);
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
    
    [SlashCommand("test3", "test command")]
    public async Task Test3(InteractionContext ctx)
    {

        await using DataBase db = new DataBase();

        var character = await db.Characters
            .Include(x => x.Inventory)
                .ThenInclude(x => x.InvItems)
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        if (character == null)
        {
            Console.WriteLine("Игрок не найден!");
            return;
        }

        var item = await db.Items.FirstOrDefaultAsync(x => x.Name == "TestItem");
        
        if (item == null)
        {
            Console.WriteLine("Предмет не найден!");
            return;
        }
        
        var inventoryItem = character.Inventory.InvItems
            .FirstOrDefault(x => x.ItemId == item.Id);
        
        if (inventoryItem != null)
        {
            // Увеличить количество, если предмет уже есть
            inventoryItem.Quantity += 1;
        }
        else
        {
            // Добавить новый предмет
            var newInvItem = new InvItem
            {
                InventoryId = character.Inventory.Id,
                ItemId = item.Id,
                Quantity = 1 //for test
            };

            db.InvItems.Add(newInvItem);
        }
        
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
}