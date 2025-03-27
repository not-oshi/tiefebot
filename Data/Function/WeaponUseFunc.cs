using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class WeaponUseFunc
{
    public static async Task<DiscordEmbed> WeaponUseFuncTask(InteractionContext ctx, string weaponName)
    {
        await using DataBase db = new DataBase();

        var invWeapon = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invWeapon => invWeapon.Item)
            .FirstAsync(x => x.Item.Name == weaponName);
        
        
        
        //TODO : finish this
        return null;
    }
}