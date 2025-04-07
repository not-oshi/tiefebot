using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class ItemsCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        var items = await db.Items
            .Select(x => new DiscordAutoCompleteChoice(x.Name, x.Id.ToString()))
            .ToListAsync();
        return items;
    }
}