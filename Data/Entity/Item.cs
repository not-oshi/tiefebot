namespace tiefebot.Data.Entity;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Enums.ItemType Type { get; set; }
    
    public string Description { get; set; } 
    public int DNH { get; set; } // Damage & Heal
    public int ROF { get; set; } // Rate Of Fire
    public int Protection { get; set; }
    public int MaxStack { get; set; }
    public bool IsDisposable { get; set; }
    public ICollection<InvItem> InvItems { get; set; }
}