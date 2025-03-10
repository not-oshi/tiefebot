namespace tiefebot.Data.Entity;

public class CharacterInventory
{
    public Guid Id { get; set; }
    public Character Character { get; set; }
    public Guid CharacterId { get; set; }
    public ICollection<Item> Items { get; set; }
    //public int Count { get; set; }
    //public bool IsEquipped { get; set; } 
}