using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public static class ItemUseFunc
{
    public static async Task<DiscordEmbed> ItemUseFuncTask(InteractionContext ctx, string itemId, bool isLearnOnly)
    {
        // Initialize the database context for querying data.
        await using DataBase db = new DataBase();
        
        // DiscordEmbedBuilder object to structure the embed response.
        DiscordEmbedBuilder embed = null;

        // Query the database to find the specified item in the user's inventory.
        var invItem = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invItem => invItem.Item)
            .FirstAsync(x => x.Item.Id.ToString() == itemId);
        
        // Handle different item types using a switch statement.
        switch (invItem.Item.ItemType)
        { 
            case Enums.ItemType.Tool:
                var tool = invItem.Item as Tool;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Damage** - [{tool.Name}]" +
                    $"\n**Maximum quantity** - [{tool.MaxStack}]",
                    inline: true);
        
                embed.AddField
                ("Usage", 
                    $"[{tool.Usage}]", 
                    inline: true);
                break;
            
            case Enums.ItemType.Weapon:
                var weapon = invItem.Item as Weapon;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Weapon Type** - [{weapon.WeaponType}]" +
                    $"\n**Damage** - [{weapon.Damage}]" +
                    $"\n**Rate of Fire** - [{weapon.ROF}]",
                    inline: true);
                break;
            
            case Enums.ItemType.Ammo:
                var ammo = invItem.Item as Ammo;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Weapon Type** - [{ammo.AmmoType}]" +
                    $"\n**Damage** - [{ammo.Damage}]",
                    inline: true);
                embed.AddField
                ("Stats", 
                    $"**Maximum quantity** - [{ammo.MaxStack}]",
                    inline: true);
                break;
            
            case Enums.ItemType.Magazine:
                var magazine = invItem.Item as Magazine;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Weapon Type** - [{magazine.MagazineType}]" +
                    $"\n**Damage** - [{magazine.Damage}]" +
                    $"\n**Capacity** - [{magazine.Capacity}]",
                    inline: true);
                embed.AddField
                ("Stats", 
                    $"\n**Maximum quantity** - [{magazine.MaxStack}]",
                    inline: true);
                break;
            
            case Enums.ItemType.Defence:
                var defence = invItem.Item as Defence;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Damage** - [{defence.Damage}]" +
                    $"\n**Protection** - [{defence.Protection}]" +
                    $"\nIs it wearable? - [{defence.IsWearable}]",
                    inline: true);
                break;
            
            case Enums.ItemType.Medication:
                var medication = invItem.Item as Medication;
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = invItem.Item.ItemType.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Heal** - [{medication.Heal}]" +
                    $"\n**Damage** - [{medication.Damage}]" +
                    $"\n**Maximum quantity** - [{medication.MaxStack}]",
                    inline: true);
                break;
        }

        // Adjust logic if the item is a Weapon or Ammo type.
        if (invItem.Item.ItemType == Enums.ItemType.Weapon | invItem.Item.ItemType == Enums.ItemType.Ammo)
        {
            isLearnOnly = true; // Set the flag to indicate learning only.
        }
        
        // If the item is for learning only, return the embed without disposal logic.
        if (isLearnOnly)
        {
            return embed;
        }
        
        // Handle item disposal logic if the item is disposable.
        if (invItem.Item.IsDisposable)
        {
            if (invItem.Quantity > 1)
            {
                invItem.Quantity -= 1;
                return embed;
            }
            // Remove the item from the inventory if quantity is 1.
            db.InvItems.Remove(invItem);
            await db.SaveChangesAsync();
        }
        
        // Return the final embed with item details.
        return embed;
    }
}