using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public static class ItemUseFunc
{
    public static async Task<DiscordEmbed> ItemUseFuncTask(InteractionContext ctx, string itemName, bool isLearnOnly)
    {
        await using DataBase db = new DataBase();

        var invItem = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invItem => invItem.Item)
            .FirstAsync(x => x.Item.Name == itemName);
        
        var embed = new DiscordEmbedBuilder
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
                Text = invItem.Item.Type.GetName() + $" | Is Disposable - {invItem.Item.IsDisposable}"
            }
        };
        embed.AddField
        ("Stats", 
            $"**Damage** - [{invItem.Item.Damage}]" +
            $"\n**Heal** - [{invItem.Item.Heal}]" +
            $"\n**Rate Of Fire** - [{invItem.Item.ROF}]",
            inline: true);
        
        embed.AddField
        (".", 
            $"**Protection** - [{invItem.Item.Protection}]" +
            $"\n**Maximum capacity** - [{invItem.Item.Capacity}]" +
            $"\n**Maximum quantity** - [{invItem.Item.MaxStack}]", 
            inline: true);

        if (invItem.Item.Type == Enums.ItemType.Weapon | invItem.Item.Type == Enums.ItemType.Ammo)
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