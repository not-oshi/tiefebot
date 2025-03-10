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
            PlayerDiscordId = ctx.User.Id,
            Name = "Test"

        };
        
        await db.Characters.AddAsync(character);
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
    
    [SlashCommand("test2", "test command")]
    public async Task Test2(InteractionContext ctx)
    {

        await using DataBase db = new DataBase();

        noeItem noeItem = new()
        {
            Name = "TestItem"
        };
        
        await db.noeItems.AddAsync(noeItem);
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
    
    [SlashCommand("test3", "test command")]
    public async Task Test3(InteractionContext ctx)
    {

        await using DataBase db = new DataBase();
        
        Character character = db.Characters.First(character => character.PlayerDiscordId == ctx.User.Id);
        CharacterInventory inventory = db.CharacterInventories.First(characterInventory => characterInventory.CharacterId == character.Id);
        noeItem noeItem = db.noeItems.First(noeItem =>  noeItem.Equals("TestItem"));
        Item item = new()
        {
            Name = noeItem.Name,
            CharacterInventoryId = inventory.Id
        };
        
        await db.Items.AddAsync(item);
        await db.SaveChangesAsync();
        
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().WithContent("Done"));
    }
}