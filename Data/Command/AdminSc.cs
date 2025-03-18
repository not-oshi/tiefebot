using System.ComponentModel.DataAnnotations;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Command;

public class AdminSc : ApplicationCommandModule
{
    [SlashCommand("New_Character", "Creates a new character - Attention! Misspellings are not allowed.")]
    [SlashRequireOwner]
    public async Task NewCharacter(InteractionContext ctx,
        [Option("Character", "The name of the Character.")] string charName,
        [Option("Character_type", "Gestalt or Replika?")] Enums.CharType charType,
        [Option("Level", "Story-Tale level")] long level,
        [Option("Personality", ".")] long personality,
        [Option("Empathy", ".")] long empathy,
        [Option("Intelligent", ".")] long intelligent,
        [Option("Armor", "tip: Gestalt can't have more than 2 points")] long armor,
        [Option("Combat", ".")] long combat,
        [Option("Dexterity", ".")] long dexterity)
    {
        //Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());
        
        // Retrieve character data from the database
        await using DataBase db = new DataBase();
        var tryCharacter = await db.Characters
            .FirstOrDefaultAsync(x => x.MemberDiscordId == ctx.User.Id);
        
        // Check if the character already exists
        if (tryCharacter != null)
        {
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Character already exists"));
            return;
        }
        
        // Create a new character
        Character character = new()
        { 
            MemberDiscordId = ctx.User.Id,
            Name = charName,
            Type = charType,
            Level = (int)level,
            Personality = (int)personality,
            Empathy = (int)empathy,
            Intelligent = (int)intelligent,
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
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("Creating done"));
    }
    
    [SlashCommand("New_Item", "Creates a new Item - Attention! Misspellings are not allowed.")]
    [SlashRequireOwner]
    public async Task NewItem(InteractionContext ctx,
        [Option("Name", "The name of the Item")] string itemName,
        [Option("Item_Type", ".")] Enums.ItemType itemType,
        [Option("Description", "Item description")] string itemDesc,
        [Option("Damage_n_Heal", "Damage or recovery points")] long itemDNH,
        [Option("Protection", "Item protection points")] long itemProtection,
        [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack)
    {
        //Making delay
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
            new DiscordInteractionResponseBuilder().AsEphemeral());
        
        await using DataBase db = new DataBase();
        
        // Create a new Item
        Item item = new()
        {
            Name = itemName,
            Type = itemType,
            Description = itemDesc,
            DNH = (int)itemDNH,
            Protection = (int)itemProtection,
            MaxStack = (int)itemMaxStack
        };
        
        await db.Items.AddAsync(item);
        await db.SaveChangesAsync();
        
        // Confirming
        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent("Creating done"));
    }
}