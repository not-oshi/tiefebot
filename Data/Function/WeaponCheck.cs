using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public class WeaponCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        
        var inventoryItems = db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Where(x => x.Item is Weapon)
            .Include(invItem => invItem.Item)
            .ToList();
        
        var invWeapons = inventoryItems
            .Select(x => new DiscordAutoCompleteChoice(x.Item.Name, x.ItemId.ToString()))
            .ToList();
        
        return invWeapons;
    }
}