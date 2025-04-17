using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public class WeaponUseFunc
{
    public static async Task<DiscordEmbed> WeaponUseFuncTask(InteractionContext ctx, string weaponId, bool singleShotMode)
    {
        // Create a random number generator.
        Random random = new Random();
        
        // Initialize the database context for querying data.
        await using DataBase db = new DataBase();
        
        // Initialize a DiscordEmbedBuilder object to create the response embed.
        DiscordEmbedBuilder embed = null;
        
        // Local function to determine if a weapon is a firearm.
        bool IsFirearm(Enums.WeaponType weaponType)
        {
            return weaponType == Enums.WeaponType.Pistol ||
                   weaponType == Enums.WeaponType.Revolver ||
                   weaponType == Enums.WeaponType.Shotgun ||
                   weaponType == Enums.WeaponType.Smg ||
                   weaponType == Enums.WeaponType.Rifle;
        }

        Guid.TryParse(weaponId, out Guid weaponGuid);
        
        // Query the database to find the weapon in the user's inventory by name.
        var invWeapon = await db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Include(invWeapon => invWeapon.Item)
            .FirstAsync(x => x.Item.Id == weaponGuid);

        // Cast the retrieved item to the Weapon type.
        var weapon = invWeapon.Item as Weapon;
        var firingMode = singleShotMode ? 1 : weapon.ROF;
        
        // Calculate the number of dice and sides of the dice based on weapon damage.
        var numberOfDice = weapon.Damage / 10;
        var sidesOfDice = weapon.Damage % 10;

        // Create an array to store dice roll results and variables for damage calculation.
        int[] diceRolls = new int[numberOfDice];
        string damageString = "";
        int totalDamage = 0;

        // Loop through the weapon's rate of fire (ROF) to calculate damage per shot.
        for (int rof = 1; rof <= firingMode; rof++)
        {
            for (int i = 0; i < numberOfDice; i++)
            {
                // Roll the dice and add the result to the total damage.
                diceRolls[i] = random.Next(1, sidesOfDice + 1);
                totalDamage += diceRolls[i];
                // Build the damage calculation string.
                damageString += $"{diceRolls[i]}";
                if (i < numberOfDice - 1) 
                {
                    damageString += " + "; // Add "+" between dice results.
                }
            }
            damageString += " + "; // Add "+" between rate of fire rounds.
        }
        // Remove trailing " +" from the damage string.
        damageString = damageString.TrimEnd(' ', '+');
        
        // Create a DiscordEmbedBuilder object to display weapon damage and stats.
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

        // If the weapon is a firearm, reduce its ammunition by the rate of fire.
        if (IsFirearm(weapon.WeaponType))
        {
            invWeapon.Ammunition -= firingMode;
            await db.SaveChangesAsync();
        }
        
        // Return the created embed as the method result.
        //TODO : finish this
        return embed;
    }
}