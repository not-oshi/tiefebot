using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public class WeaponUseFunc
{
    public static async Task<DiscordEmbed> WeaponUseFuncTask(InteractionContext ctx, string weaponName)
    {
        Random random = new Random();
        await using DataBase db = new DataBase();
        DiscordEmbedBuilder embed = null;

        var invWeapon = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invWeapon => invWeapon.Item)
            .FirstAsync(x => x.Item.Name == weaponName);

        var weapon = invWeapon.Item as Weapon;

        var numberOfDice = weapon.Damage / 10;
        var sidesOfDice = weapon.Damage % 10;

        int[] diceRolls = new int[numberOfDice];
        string damageString = ""; // Строка для записи урона
        int totalDamage = 0;

        for (int rof = 1; rof <= weapon.ROF; rof++)
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                diceRolls[i] = random.Next(1, sidesOfDice + 1);
                totalDamage += diceRolls[i];
                damageString += (i == 0) ? $"{diceRolls[i]}" : $" + {diceRolls[i]}";
            }
            // todo - remove this
            Console.WriteLine($"{damageString} = {totalDamage}");
        }
        
        embed = new DiscordEmbedBuilder 
        {
            Color = new DiscordColor(0x961515),
            Title = weapon.Name,
            Description = $"Damage - [{numberOfDice}d{sidesOfDice}] | Rate Of Fire - [{weapon.ROF}]" +
                          $"\n {damageString} = {totalDamage}",
            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            {
                Url = "https://i.imgur.com/AfFp7pu.png"
            }
        };
        
        //TODO : finish this
        return embed;
    }
}