using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class InvItemsCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        
        var invItems = db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Select(x => new DiscordAutoCompleteChoice(x.Item.Name, x.Item.Name))
            .ToList();

        return invItems;
    }
}