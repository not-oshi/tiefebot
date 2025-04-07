using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;
using tiefebot.Data.Entity;

namespace tiefebot.Data.Function;

public class MagazineCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        
        var inventoryItems = db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Where(x => x.Item is Magazine)
            .Include(invItem => invItem.Item)
            .ToList();
        
        var invAmmos = inventoryItems
            .Select(x =>
                new DiscordAutoCompleteChoice(
                    $"{x.Item.Name} [{x.Ammunition}/{(x.Item is Magazine magazine ? magazine.Capacity : 0)}]",
                    x.Item.Id.ToString()))
            .ToList();
        
        return invAmmos;
    }
}