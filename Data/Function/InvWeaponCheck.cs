using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace tiefebot.Data.Function;

public class InvWeaponCheck : IAutocompleteProvider
{
    public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
    {
        await using DataBase db = new DataBase();

        var invWeapons = db.Characters
            .Where(x => x.MemberDiscordId == ctx.User.Id)
            .SelectMany(x => x.Inventory.InvItems)
            .Where(x => x.Item.ItemType == Enums.ItemType.Weapon)
            .Select(x => new DiscordAutoCompleteChoice(x.Item.Name, x.Item.Name))
            .ToList();
        
        return invWeapons;
    }
}