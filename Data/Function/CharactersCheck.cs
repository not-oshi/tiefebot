using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.EntityFrameworkCore;

namespace tiefebot.Data.Function;

public class CharactersCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();
        var choices = await db.Characters
            .Select(x => new DiscordAutoCompleteChoice(x.Name.ToString(), x.Name.ToString()))
            .ToListAsync();
        return choices;
    }
}