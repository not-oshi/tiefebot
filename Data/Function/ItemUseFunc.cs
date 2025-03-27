using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public static class ItemUseFunc
{
    public static async Task<DiscordEmbed> ItemUseFuncTask(InteractionContext ctx, string itemName, bool isLearnOnly)
    {
        await using DataBase db = new DataBase();
        DiscordEmbedBuilder embed = null;

        var invItem = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invItem => invItem.Item)
            .FirstAsync(x => x.Item.Name == itemName);

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
                    $"**Weapon Type** - [{ammo.WeaponType}]" +
                    $"\n**Damage** - [{ammo.Damage}]" +
                    $"\n**Capacity** - [{ammo.Capacity}]",
                    inline: true);
                embed.AddField
                ("Stats", 
                    $"**Is it Magazine?** - [{ammo.IsMagazine}]" +
                    $"\n**Maximum quantity** - [{ammo.MaxStack}]",
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

        if (invItem.Item.ItemType == Enums.ItemType.Weapon | invItem.Item.ItemType == Enums.ItemType.Ammo)
        {
            isLearnOnly = true;
        }

        //Need to remake this for guns - no, need new func for guns
        if (isLearnOnly)
        {
            return embed;
        }
        if (invItem.Item.IsDisposable)
        {
            if (invItem.Quantity > 1)
            {
                invItem.Quantity -= 1;
                return embed;
            }
            db.InvItems.Remove(invItem);
            await db.SaveChangesAsync();
        }
        return embed;
    }
}