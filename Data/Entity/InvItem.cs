namespace tiefebot.Data.Entity;

public class InvItem
{
    public Guid InventoryId  { get; set; }
    public Inventory Inventory { get; set; }
    
    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    public int Ammunition { get; set; }
    public int Durability { get; set; }
    public bool IsEquipped { get; set; } // It's for "IsWearable"
    public bool IsLoaded { get; set; }
    public Guid LoadedMagGuid { get; set; }
    public int Quantity { get; set; }
}