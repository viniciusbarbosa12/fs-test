using Lib.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lib.Repository.Mappings;

public class MonsterExtendedMapping: IEntityTypeConfiguration<Monster>
{
    public void Configure(EntityTypeBuilder<Monster> builder)
    {
        builder.ToTable("Monsters");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Attack).IsRequired();
        builder.Property(m => m.Defense).IsRequired();
        builder.Property(m => m.Speed).IsRequired();
        builder.Property(m => m.Hp).IsRequired();
        builder.Property(m => m.ImageUrl).HasMaxLength(300);

        builder.HasMany<Battle>()
            .WithOne(b => b.MonsterARelation!)
            .HasForeignKey(b => b.MonsterA)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany<Battle>()
            .WithOne(b => b.MonsterBRelation!)
            .HasForeignKey(b => b.MonsterB)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany<Battle>()
            .WithOne(b => b.WinnerRelation!)
            .HasForeignKey(b => b.Winner)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
