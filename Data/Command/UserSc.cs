using System.Reflection;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using tiefebot.Data.Entity;
using tiefebot.Data.Function;

namespace tiefebot.Data.Command;

public class UserSc : ApplicationCommandModule
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
         Option("Item", "Chose the Item", true)] string choosedItemId)
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
        var item = await db.Items.FirstOrDefaultAsync(x => x.Id.ToString() == choosedItemId);
        
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
                    .WithContent("You can't  carry any more"));
                return;
            }
        }
        
        await db.SaveChangesAsync();
        
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("You got the item"));
    }

    [SlashCommand("Use_Item", "A command to use the Items")]
    public async Task UseItem(InteractionContext ctx,
        [Autocomplete(typeof(InvItemsCheck)), Option("Inventory_Item", "Select the item you want to use", true)] string itemId)
    {
        //Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder());
        
        var embed = await ItemUseFunc.ItemUseFuncTask(ctx, itemId, false);
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embed));
    }

    [SlashCommand("Use_Weapon", "A command to use the Weapon")]
    public async Task UseWeapon(InteractionContext ctx,
        [Autocomplete(typeof(InvWeaponCheck)), Option("Inventory_Item", "Select the item you want to use", true)] string weaponId,
        [Option("Single_shot_mode", "Do you want to take a single shot?")] bool singleShotMode)
    {
        //Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder());

        var embed = await WeaponUseFunc.WeaponUseFuncTask(ctx, weaponId, singleShotMode);
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embed));
    }

    [SlashCommand("Inspect_Item", "A command to inspect Items")]
    public async Task InspectItem(InteractionContext ctx,
        [Autocomplete(typeof(InvItemsCheck)), Option("Inventory_Item", "Select the item you want to use", true)] string itemId)
    {
        //Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());

        var embed = await ItemUseFunc.ItemUseFuncTask(ctx, itemId, true);
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(embed));
    }
    
    
    [SlashCommand("Transfer_Item", "A command to transfer Items")]
    public async Task TransferItem(InteractionContext ctx,
        [Autocomplete(typeof(CharactersCheck)), Option("Recipient", "The character you want to transfer the item to", true)] string charId,
        [Autocomplete(typeof(InvItemsCheck)), Option("Inventory_Item", "Select the item you want to use", true)] string itemId)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());
        
        await using DataBase db = new DataBase();
        
        // Searching Sender Character
        var sender = await db.Characters 
            .Include(x => x.Inventory)
            .ThenInclude(x => x.InvItems)
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        // Searching Recipient Character
        var recipient = await db.Characters
            .Include(x => x.Inventory)
            .ThenInclude(x => x.InvItems)
            .FirstOrDefaultAsync(x => x.Id.ToString() == charId);
        
        // Searching item in Sender inventory
        var invItem = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invItem => invItem.Item)
            .FirstAsync(x => x.Item.Id.ToString() == itemId);
        
        // Transfering item
        invItem.InventoryId = recipient.Inventory.Id;

        await db.SaveChangesAsync();
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent($"You gave item to {recipient.Name}"));
    }

    [SlashCommand("Load_Magazine", "A command to Load the magazine")]
    public async Task LoadMagazine(InteractionContext ctx,
        [Autocomplete(typeof(MagazineCheck)), Option("Recipient", "The character you want to transfer the item to", true)] string ammoName)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());
        // todo finish this command
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent(ammoName));
    }
    
    [SlashCommand("Stat_Check", "Check any of your STATs for actions")]
    public async Task CheckTest(InteractionContext ctx,
        [Option("STAT", "What stat you want to check?")] Enums.Stats statName)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder());
        
        await using DataBase db = new DataBase();
        var character = await db.Characters 
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);

        var stat = statName.GetName();
        if (statName.GetName() == "Personality")
            stat = "PersonalityLeft";
        
        var propertyInfo = typeof(Character).GetProperty(stat);
        var value = propertyInfo.GetValue(character);
        
        var embed = await DiceCheck.CheckDice(value is int ? (int)value : 0, statName);
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
    }    
}