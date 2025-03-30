using Microsoft.VisualBasic.CompilerServices;

namespace tiefebot.Data.Entity;

public class Character
{
    public Guid Id { get; set; }
    public ulong MemberDiscordId { get; set; }
    public string Name { get; set; }
    public Enums.CharType Type { get; set; }
    public int Level { get; set; } //Story-tale Level
    public int Personality { get; set; }
    public int PersonalityLeft { get; set; }
    public int Empathy { get; set; }
    public int Intelligent { get; set; }
    public int Armor { get; set; }
    public int Combat { get; set; }
    public int Dexterity { get; set; }
    
    public Inventory Inventory { get; set; }
}