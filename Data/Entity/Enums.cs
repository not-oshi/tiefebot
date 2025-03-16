using DSharpPlus.SlashCommands;

namespace tiefebot.Data;

public class Enums
{
    public enum CharType
    {
        [ChoiceName("Gestalt")]
        Gestalt,
        [ChoiceName("Replika")]
        Replika
    }
    
    public enum Stats
    {
        [ChoiceName("Personality")]
        Personality,
        [ChoiceName("Empathy")]
        Empathy,
        [ChoiceName("Inteligent")]
        Inteligent,
        [ChoiceName("Armor")]
        Armor,
        [ChoiceName("Combat")]
        Combat,
        [ChoiceName("Dexterity")]
        Dexterity
    }
}