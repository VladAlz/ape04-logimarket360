using LogiMarket.Domain.Catalogo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class KitConfiguracion : IEntityTypeConfiguration<Kit>
{
    public void Configure(EntityTypeBuilder<Kit> b)
    {
        b.ToTable("Kit");
        b.HasKey(k => k.Id);
        b.Property(k => k.Codigo).HasMaxLength(30).IsRequired();
        b.HasIndex(k => k.Codigo).IsUnique();
        b.Property(k => k.Nombre).HasMaxLength(100).IsRequired();
        b.HasMany(k => k.Componentes).WithOne().HasForeignKey(c => c.KitId).OnDelete(DeleteBehavior.Cascade);
        b.Navigation(k => k.Componentes).HasField("_componentes").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class KitComponenteConfiguracion : IEntityTypeConfiguration<KitComponente>
{
    public void Configure(EntityTypeBuilder<KitComponente> b)
    {
        b.ToTable("KitComponente", t => t.HasCheckConstraint("CK_KitComponente_Cantidad", "Cantidad > 0"));
        b.HasKey(c => c.Id);
        b.Property(c => c.CodigoComponente).HasMaxLength(30).IsRequired();
        b.HasIndex(c => new { c.KitId, c.CodigoComponente }).IsUnique();
    }
}
