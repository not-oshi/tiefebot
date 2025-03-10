namespace tiefebot.Data.Entity;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CharacterInventory CharacterInventory { get; set; }
    public Guid CharacterInventoryId { get; set; }
}