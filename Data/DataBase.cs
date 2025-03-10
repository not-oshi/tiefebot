using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using tiefebot.Data.Entity;

namespace tiefebot.Data;

public class DataBase : DbContext
{
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterInventory> CharacterInventories { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<noeItem> noeItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CharacterConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterInventoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=tiefebot.db");
}

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.CharacterInventory)
            .WithOne(x => x.Character)
            .HasForeignKey<CharacterInventory>(x => x.CharacterId);
    }
}

public class CharacterInventoryConfiguration : IEntityTypeConfiguration<CharacterInventory>
{
    public void Configure(EntityTypeBuilder<CharacterInventory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Character)
            .WithOne(x => x.CharacterInventory)
            .HasForeignKey<CharacterInventory>(x => x.CharacterId);
        builder.HasMany(x => x.Items)
            .WithOne(x => x.CharacterInventory)
            .HasForeignKey(x => x.CharacterInventoryId);
    }
}

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.CharacterInventory)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CharacterInventoryId);
    }
}