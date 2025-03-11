namespace tiefebot.Data.Entity;

public class InvItem
{
    public Guid InventoryId  { get; set; }
    public Inventory Inventory { get; set; }
    
    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    
    public int Quantity { get; set; }
}