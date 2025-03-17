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

    [SlashCommand("Get_Item", "A command to get an items")]
    public async Task GetItem(InteractionContext ctx,
        [Autocomplete(typeof(ItemsCheck)), 
         Option("Item", "Chose the Item", true)] string choosedItem)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        
        await using DataBase db = new DataBase();
        
        // Searching Player Character
        var character = await db.Characters
            .Include(x => x.Inventory)
            .ThenInclude(x => x.InvItems)
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        if (character == null) 
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .WithContent("Character not found").AsEphemeral(true));
            return;
        }
        
        // Searching available items
        var item = await db.Items.FirstOrDefaultAsync(x => x.Name == choosedItem);
        
        // Searching InventoryItem
        var inventoryItem = character.Inventory.InvItems
            .FirstOrDefault(x => x.ItemId == item.Id);
        
        // need to finish this
        
        
        
        
        
        
        
    }
    
    
    [SlashCommand("checktest", "1")]
    public async Task checktest(InteractionContext ctx,
        [Autocomplete(typeof(CharactersCheck)), Option("test", "test", true)] string id)
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