using DSharpPlus.SlashCommands;

namespace tiefebot.Data;

public class Enums
{
    //To use this - [Option("Character_type", "Gestalt or Replica?")] Enums.CharType charType
    public enum CharType
    {
        [ChoiceName("Gestalt")]
        Gestalt,
        [ChoiceName("Replica")]
        Replica
    }
}