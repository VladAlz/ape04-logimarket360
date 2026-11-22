using LogiMarket.Domain.Catalogo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> b)
    {
        b.ToTable("Producto", t =>
        {
            t.HasCheckConstraint("CK_Producto_Stock", "Stock >= 0");
            t.HasCheckConstraint("CK_Producto_Precio", "CAST(Precio AS REAL) > 0");
            t.HasCheckConstraint("CK_Producto_Peso", "CAST(PesoKg AS REAL) >= 0");
        });
        b.HasKey(p => p.Id);
        b.Property(p => p.Sku).HasMaxLength(30).IsRequired();
        b.HasIndex(p => p.Sku).IsUnique();
        b.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
        b.HasIndex(p => p.Nombre);
        b.Property(p => p.Categoria).HasMaxLength(40);
        b.Property(p => p.Descripcion).HasMaxLength(300);
        b.Property(p => p.Precio).HasPrecision(12, 2);
        b.Property(p => p.PesoKg).HasPrecision(10, 3);
        // Dos ventas sobre el mismo producto: la segunda falla al guardar
        b.Property(p => p.Version).IsConcurrencyToken();
    }
}
