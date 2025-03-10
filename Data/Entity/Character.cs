namespace tiefebot.Data.Entity;

public class Character
{
    public Guid Id { get; set; }
    public ulong PlayerDiscordId { get; set; }
    public string Name { get; set; }
    //public string Type { get; set; } //Gestalt or Replika
    //public int Level { get; set; } //Story-tale Level
    
    //public int Personality { get; set; }
    //public int Empathy { get; set; }
    //public int Inteligent { get; set; }
    //public int Armor { get; set; }
    //public int Combat { get; set; }
    //public int Dexterity { get; set; }
    
    public CharacterInventory CharacterInventory { get; set; }
}