using System.Xml.Schema;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tiefebot.Data.Entity;

namespace tiefebot.Data;

public class DataBase : DbContext
{
    public DbSet<Character> Characters { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<InvItem> InvItems { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CharacterConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryConfiguration());
        modelBuilder.ApplyConfiguration(new InvItemConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=tiefebot.db");
        optionsBuilder.LogTo(Console.WriteLine);
    }
}

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Inventory)
            .WithOne(x => x.Character)
            .HasForeignKey<Inventory>(x => x.CharacterId);
    }
}

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.InvItems)
            .WithOne(x => x.Inventory)
            .HasForeignKey(x => x.InventoryId);
    }
}

public class InvItemConfiguration : IEntityTypeConfiguration<InvItem>
{
    public void Configure(EntityTypeBuilder<InvItem> builder)
    {
        builder.HasKey(xx => new{xx.InventoryId, xx.ItemId});
        builder.HasOne(x => x.Inventory)
            .WithMany(x => x.InvItems)
            .HasForeignKey(x => x.InventoryId);
        builder.HasOne(x => x.Item)
            .WithMany(x => x.InvItems)
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasDiscriminator<Enums.ItemType>(nameof(Item.ItemType))
            .HasValue<Tool>(Enums.ItemType.Tool)
            .HasValue<Weapon>(Enums.ItemType.Weapon)
            .HasValue<Ammo>(Enums.ItemType.Ammo)
            .HasValue<Magazine>(Enums.ItemType.Magazine)
            .HasValue<Defence>(Enums.ItemType.Defence)
            .HasValue<Medication>(Enums.ItemType.Medication);
    }
}