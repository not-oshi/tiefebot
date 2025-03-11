namespace tiefebot.Data.Entity;

public class Inventory
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; }
    public ICollection<InvItem> InvItems { get; set; }
}