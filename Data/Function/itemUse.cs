using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class itemUse
{
    public static async Task<DiscordEmbed> CheckDice(InteractionContext ctx, string itemName, bool isLearnOnly)
    {
        DiscordEmbedBuilder embed;
        
        await using DataBase db = new DataBase();

        var invItem = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invItem => invItem.Item)
            .FirstAsync(x => x.Item.Name == itemName);

        switch (invItem.Item.Type)
        {
            case Enums.ItemType.Tool:
                embed = new DiscordEmbedBuilder
                {
                    Color = new Optional<DiscordColor>(961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    }
                };
                break;
            case Enums.ItemType.Weapon:
                embed = new DiscordEmbedBuilder
                {
                    Color = new Optional<DiscordColor>(961515),
                    Title = invItem.Item.Name,
                    Description = invItem.Item.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                };
                embed.AddField("Damage", $"{invItem.Item.DNH}", inline: true);
                embed.AddField("Rate Of Fire", $"{invItem.Item.ROF}", inline: true);
                break;
        }
        
        return NotImplementedException;
    }

    public static DiscordEmbed NotImplementedException { get; set; }
}