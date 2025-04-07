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

    public enum ItemType
    {
        [ChoiceName("Tool")]
        Tool,
        [ChoiceName("Weapon")]
        Weapon,
        [ChoiceName("Ammo")]
        Ammo,
        [ChoiceName("Magazine")]
        Magazine,
        [ChoiceName("Defence")]
        Defence,
        [ChoiceName("Medication")]
        Medication
    }

    public enum WeaponType
    {
        [ChoiceName("Very_Lightweight_Melee")]
        VeryLightweightMelee,
        [ChoiceName("Lightweight_Melee")]
        LightweightMelee,
        [ChoiceName("Medium_Melee")]
        MediumMelee,
        [ChoiceName("Pistol")]
        Pistol,
        [ChoiceName("Revolver")]
        Revolver,
        [ChoiceName("Shotgun")]
        Shotgun,
        [ChoiceName("SMG")]
        Smg,
        [ChoiceName("Rifle")]
        Rifle,
        [ChoiceName("Mining_Laser")]
        MiningLaser
        //TODO: need to finish this
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