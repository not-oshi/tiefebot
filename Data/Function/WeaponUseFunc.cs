using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public class WeaponUseFunc
{
    public static async Task<DiscordEmbed> WeaponUseFuncTask(InteractionContext ctx, string weaponName)
    {
        await using DataBase db = new DataBase();
        DiscordEmbedBuilder embed = null;

        var invWeapon = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invWeapon => invWeapon.Item)
            .FirstAsync(x => x.Item.Name == weaponName);

        var weapon = invWeapon.Item as Weapon;

        switch (weapon.WeaponType)
        {
            case Enums.WeaponType.VeryLightweightMelee:
                embed = new DiscordEmbedBuilder 
                {
                    Color = new DiscordColor(0x961515),
                    Title = weapon.Name,
                    Description = weapon.Description,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://i.imgur.com/AfFp7pu.png"
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = weapon.ItemType.GetName() + $" | Is Disposable - {weapon.IsDisposable}"
                    }
                };
                embed.AddField
                ("Stats", 
                    $"**Damage** - [{weapon.Name}]" +
                    $"\n**Maximum quantity** - [{weapon.MaxStack}]",
                    inline: true);
        
                embed.AddField
                ("Usage", 
                    $"[]", 
                    inline: true);
                break;
        }
        
        //TODO : finish this
        return null;
    }
}