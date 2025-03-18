using DSharpPlus.SlashCommands;

namespace tiefebot.Data;

public class Enums
{
    public enum CharType
    {
        [ChoiceName("Gestalt")]
        //Gestalt,
        Option1,
        [ChoiceName("Replika")]
        //Replika
        Option2
    }

    public enum ItemType
    {
        [ChoiceName("Tool")]
        Tool,
        [ChoiceName("Weapon")]
        Weapon,
        [ChoiceName("Ammo")]
        Ammo,
        [ChoiceName("Defence")]
        Defence,
        [ChoiceName("Medication")]
        Medication
    }
    
    public enum Stats
    {
        [ChoiceName("Personality")]
        Personality,
        [ChoiceName("Empathy")]
        Empathy,
        [ChoiceName("Intelligent")]
        Intelligent,
        [ChoiceName("Armor")]
        Armor,
        [ChoiceName("Combat")]
        Combat,
        [ChoiceName("Dexterity")]
        Dexterity
    }
}