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

    [SlashCommand("Get_Item", "A command to get the items")]
    public async Task GetItem(InteractionContext ctx,
        [Autocomplete(typeof(ItemsCheck)), 
         Option("Item", "Chose the Item", true)] string choosedItem)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());
        
        await using DataBase db = new DataBase();
        
        // Searching Player Character
        var character = await db.Characters
            .Include(x => x.Inventory)
            .ThenInclude(x => x.InvItems)
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        if (character.MemberDiscordId == null) 
        {
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Character not found"));
            return;
        }
        
        // Searching choosed item
        var item = await db.Items.FirstOrDefaultAsync(x => x.Name == choosedItem);
        
        // Searching InventoryItem
        var inventoryItem = character.Inventory.InvItems
            .FirstOrDefault(x => x.ItemId == item.Id);
        
        if (inventoryItem != null) 
        {
            var inventoryCounts = inventoryItem.Inventory.InvItems.Count;
            if (inventoryCounts >= 6)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("Rule of six!"));
                return;
            }
        }
        
        if (inventoryItem == null)
        {
            // Add item if it not exist in inventory
            var newInvItem = new InvItem
            {
                InventoryId = character.Inventory.Id,
                ItemId = item.Id,
                
                Quantity = 1
            };
            db.InvItems.Add(newInvItem);
        }
        else
        {
            // Increase the quantity if the item already exists
            if (inventoryItem.Quantity < item.MaxStack)
                inventoryItem.Quantity += 1;
            else
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                    .WithContent("You can't carry any more"));
                return;
            }
        }
        
        await db.SaveChangesAsync();
        
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("You got the item"));
    }

    [SlashCommand("Use_Item", "A command to use the Items")]
    public async Task UseItem(InteractionContext ctx,
        [Autocomplete(typeof(InvItemsCheck)), Option("Invetory_Item", "Select the item you want to use", true)] string itemName)
    {
        
    }
    
    
    [SlashCommand("checktest", "1")]
    public async Task checktest(InteractionContext ctx,
        [Autocomplete(typeof(CharactersCheck)), Option("test", "test", true)] string charName)
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
}