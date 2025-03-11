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
        var tryCharacter = db.Characters.FirstOrDefault(x => x.MemberDiscordId == ctx.User.Id);

        if (tryCharacter == null)
        {
            //Creating Character
            Character character = new()
            { 
                MemberDiscordId = ctx.User.Id,
                Name = "Test",
            };
        
            await db.Characters.AddAsync(character);
            await db.SaveChangesAsync();
        
            //Creating Inventory for Character
            Inventory inventory = new()
            {
                CharacterId = character.Id,
            };
            
            await db.Inventories.AddAsync(inventory);
            await db.SaveChangesAsync();
        }
        else
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent("Already exists"));
            return;
        }
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
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