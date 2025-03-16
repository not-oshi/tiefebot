using System.ComponentModel.DataAnnotations;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Command;

public class AdminSc
{
    [SlashCommand("New_Character", "Creates a new character - Attention! Misspellings are not allowed.")]
    [SlashRequireOwner]
    public async Task NewCharacter(InteractionContext ctx,
        [Option("Character", "The name of the Character.")] string charName,
        [Option("Character_type", "Gestalt or Replika?")] Enums.CharType charType,
        [Option("Level", "Story-Tale level")] long level,
        [Option("Personality", ".")] long personality,
        [Option("Empathy", ".")] long empathy,
        [Option("Inteligent", ".")] long inteligent,
        [Option("Armor", "tip: Gestalt can't have more than 2 points")] long armor,
        [Option("Combat", ".")] long combat,
        [Option("Dexterity", ".")] long dexterity)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        // Retrieve character data from the database
        await using DataBase db = new DataBase();
        var tryCharacter = await db.Characters
            .FirstAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        // Check if the character already exists
        if (tryCharacter.MemberDiscordId == ctx.User.Id)
        {
            await ctx.Interaction.CreateResponseAsync(
                InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .WithContent("Character already exists")
                    .AsEphemeral(true));
            return;
        }

        // Create a new character
        Character character = new()
        { 
            MemberDiscordId = ctx.User.Id,
            Name = charName,
            Type = charType.ToString(),
            Level = (int)level,
            Personality = (int)personality,
            Empathy = (int)empathy,
            Inteligent = (int)inteligent,
            Armor = (int)armor,
            Combat = (int)combat,
            Dexterity = (int)dexterity
        };
        await db.Characters.AddAsync(character);
        await db.SaveChangesAsync();
        
        // Create inventory for the new character
        Inventory inventory = new()
        {
            CharacterId = character.Id,
        };
        await db.Inventories.AddAsync(inventory);
        await db.SaveChangesAsync();

        // Confirm the Character creation
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .WithContent("Creating done").AsEphemeral(true));
    }
    
    [SlashCommand("New_Item", "Creates a new Item - Attention! Misspellings are not allowed.")]
    [SlashRequireOwner]
    public async Task NewItem(InteractionContext ctx)
    {
        // Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        
        await using DataBase db = new DataBase();
        
        // Create a new Item
        Item item = new()
        {
            Name = "TestItem"
        };
        
        await db.Items.AddAsync(item);
        await db.SaveChangesAsync();
        
        // Confirm the Item creation
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .WithContent("Creating done").AsEphemeral(true));
    }
}