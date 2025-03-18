using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class InvItemsCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        
        var character = await db.Characters
            .Include(x => x.Inventory)
            .ThenInclude(x => x.InvItems).ThenInclude(invItem => invItem.Item)
            .FirstAsync(x => x.MemberDiscordId == ctx.User.Id);

        var invItems = character.Inventory.InvItems
            .Select(x => new DiscordAutoCompleteChoice(x.Item.Name.ToString(), x.Item.Name.ToString()))
            .ToList();
        
        return invItems;
    }
}