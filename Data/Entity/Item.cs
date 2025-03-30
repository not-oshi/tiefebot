namespace tiefebot.Data.Entity;

public abstract class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Enums.ItemType ItemType { get; set; }
    public string Description { get; set; } 
    public int Damage { get; set; }
    public int MaxStack { get; set; }
    public bool IsDisposable { get; set; }
    public ICollection<InvItem> InvItems { get; set; }
}

public class Tool : Item
{
    public string Usage { get; set; }
}

public class Weapon : Item
{
    public Enums.WeaponType WeaponType { get; set; }
    public int ROF { get; set; } // Rate Of Fire
}

public class Ammo : Item
{
    public Enums.WeaponType AmmoType { get; set; }
    public int Capacity { get; set; } //For guns and magazines
    public bool IsMagazine { get; set; }
}

public class Defence : Item
{
    public int Protection { get; set; }
    public bool IsWearable { get; set; } 
}

public class Medication : Item
{
    public int Heal { get; set; }
}
