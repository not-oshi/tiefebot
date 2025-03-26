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
    
    
    [SlashCommandGroup("Create", "Create an item - Attention! Misspellings are not allowed.")]
    public class CreateContainer : ApplicationCommandModule
    {
        [SlashCommand("Tool", "Create a Tool")]
        [SlashRequireOwner]
        public async Task CreateTool(InteractionContext ctx,
            [Option("Name", "The name of the Item")] string itemName,
            [Option("Item_Type", ".")] Enums.ItemType itemType,
            [Option("Description", "Item description")] string itemDesc,
            [Option("Usage", "What this tool is for")] string itemUsage,
            [Option("Damage", "Damage points")] long itemDamage,
            [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack,
            [Option("Disposable", "Is the item disposable")] bool isDisposable
            )
        {
            // Making delay
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral());
            
            // Creating a database connection
            await using DataBase db = new DataBase();
            
            // Applying stats
            var tool = new Tool()
            {
                Name = itemName,
                ItemType = itemType,
                Description = itemDesc,
                Usage = itemUsage,
                Damage = (int)itemDamage,
                MaxStack = (int)itemMaxStack,
                IsDisposable = isDisposable
            };
            
            // Adding item and saving db
            await db.Items.AddAsync(tool);
            await db.SaveChangesAsync();
            // Confirming
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Creating done"));
        }

        [SlashCommand("Weapon", "Create a Weapon")]
        [SlashRequireOwner]
        public async Task CreateWeapon(InteractionContext ctx, 
            [Option("Name", "The name of the Item")] string itemName,
            [Option("Item_Type", ".")] Enums.ItemType itemType,
            [Option("Description", "Item description")] string itemDesc,
            [Option("Weapon_Type", "What kind of weapon you want to create")] Enums.WeaponType itemWeaponType,
            [Option("Damage", "Damage points")] long itemDamage,
            [Option("Rate_Of_Fire", "Rate of Fire")] long itemROF,
            [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack,
            [Option("Disposable", "Is the item disposable")] bool isDisposable
            )
        {
            // Making delay
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral());
            
            // Creating a database connection
            await using DataBase db = new DataBase();
            
            // Applying stats
            var weapon = new Weapon()
            {
                Name = itemName,
                ItemType = itemType,
                Description = itemDesc,
                WeaponType = itemWeaponType,
                Damage = (int)itemDamage,
                ROF = (int)itemROF,
                MaxStack = (int)itemMaxStack,
                IsDisposable = isDisposable
            };
            
            // Adding item and saving db
            await db.Items.AddAsync(weapon);
            await db.SaveChangesAsync();
            // Confirming
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Creating done"));
        }
        
        [SlashCommand("Ammo", "Create a Ammo")]
        [SlashRequireOwner]
        public async Task CreateAmmo(InteractionContext ctx, 
            [Option("Name", "The name of the Item")] string itemName,
            [Option("Item_Type", ".")] Enums.ItemType itemType,
            [Option("Description", "Item description")] string itemDesc,
            [Option("Weapon_Type", "What kind of weapon you want to create")] Enums.WeaponType itemWeaponType,
            [Option("Damage", "Damage points")] long itemDamage,
            [Option("Magazine", "Is the item is magazine?")] bool isMagazine,
            [Option("Capacity", "(For Magazines) How much ammo it can store?")] long itemCapacity,
            [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack,
            [Option("Disposable", "Is the item disposable")] bool isDisposable
        )
        {
            // Making delay
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral());
            
            // Creating a database connection
            await using DataBase db = new DataBase();
            
            // Applying stats
            var ammo = new Ammo()
            {
                Name = itemName,
                ItemType = itemType,
                Description = itemDesc,
                WeaponType = itemWeaponType,
                IsMagazine = isMagazine,
                Capacity = (int)itemCapacity,
                Damage = (int)itemDamage,
                MaxStack = (int)itemMaxStack,
                IsDisposable = isDisposable
            };
            
            // Adding item and saving db
            await db.Items.AddAsync(ammo);
            await db.SaveChangesAsync();
            // Confirming
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Creating done"));
        }
        
        [SlashCommand("Defence", "Create a Defence")]
        [SlashRequireOwner]
        public async Task CreateDefence(InteractionContext ctx, 
            [Option("Name", "The name of the Item")] string itemName,
            [Option("Item_Type", ".")] Enums.ItemType itemType,
            [Option("Description", "Item description")] string itemDesc,
            [Option("Damage", "Damage points")] long itemDamage,
            [Option("Protection", "Protection points")] long itemProtection,
            [Option("Wearable", "Is the item Wearable")] bool isWearable,
            [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack,
            [Option("Disposable", "Is the item disposable")] bool isDisposable
        )
        {
            // Making delay
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral());
            
            // Creating a database connection
            await using DataBase db = new DataBase();
            
            // Applying stats
            var defence = new Defence()
            {
                Name = itemName,
                ItemType = itemType,
                Description = itemDesc,
                Protection = (int)itemProtection,
                IsWearable = isWearable,
                Damage = (int)itemDamage,
                MaxStack = (int)itemMaxStack,
                IsDisposable = isDisposable
            };
            
            // Adding item and saving db
            await db.Items.AddAsync(defence);
            await db.SaveChangesAsync();
            // Confirming
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Creating done"));
        }
        
        [SlashCommand("Medication", "Create a Medication")]
        [SlashRequireOwner]
        public async Task CreateMedication(InteractionContext ctx, 
            [Option("Name", "The name of the Item")] string itemName,
            [Option("Item_Type", ".")] Enums.ItemType itemType,
            [Option("Description", "Item description")] string itemDesc,
            [Option("Damage", "Damage points")] long itemDamage,
            [Option("Heal", "Heal points")] long itemHeal,
            [Option("Max_Stack", "Maximum number of items in one stack")] long itemMaxStack,
            [Option("Disposable", "Is the item disposable")] bool isDisposable
        )
        {
            // Making delay
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral());
            
            // Creating a database connection
            await using DataBase db = new DataBase();
            
            // Applying stats
            var medication = new Medication()
            {
                Name = itemName,
                ItemType = itemType,
                Description = itemDesc,
                Heal = (int)itemHeal,
                Damage = (int)itemDamage,
                MaxStack = (int)itemMaxStack,
                IsDisposable = isDisposable
            };
            
            // Adding item and saving db
            await db.Items.AddAsync(medication);
            await db.SaveChangesAsync();
            // Confirming
            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent("Creating done"));
        }
    }
}