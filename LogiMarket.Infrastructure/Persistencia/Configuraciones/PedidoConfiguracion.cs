using LogiMarket.Domain.Clientes;
using LogiMarket.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class PedidoConfiguracion : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> b)
    {
        b.ToTable("Pedido");
        b.HasKey(p => p.Id);
        b.HasOne<Cliente>().WithMany().HasForeignKey(p => p.ClienteId).OnDelete(DeleteBehavior.Restrict);
        b.Property(p => p.ClienteNombre).HasMaxLength(120).IsRequired();
        b.Property(p => p.CategoriaCliente).HasConversion<string>().HasMaxLength(20);
        b.Property(p => p.Entrega).HasConversion<string>().HasMaxLength(20);
        b.Property(p => p.EstadoCodigo).HasColumnName("Estado").HasMaxLength(20).IsRequired();
        b.HasIndex(p => p.EstadoCodigo);
        b.Property(p => p.Politica).HasMaxLength(20);
        b.Property(p => p.Subtotal).HasPrecision(12, 2);
        b.Property(p => p.Descuento).HasPrecision(12, 2);
        b.Property(p => p.Tarifa).HasPrecision(12, 2);
        b.Property(p => p.Total).HasPrecision(12, 2);
        b.Property(p => p.Version).IsConcurrencyToken();

        b.Ignore(p => p.Estado);
        b.Ignore(p => p.UnidadesFisicas);
        b.Ignore(p => p.ImporteProductos);

        b.HasMany(p => p.Lineas).WithOne().HasForeignKey(l => l.PedidoId).OnDelete(DeleteBehavior.Cascade);
        b.Navigation(p => p.Lineas).HasField("_lineas").UsePropertyAccessMode(PropertyAccessMode.Field);
        b.HasMany(p => p.Historial).WithOne().HasForeignKey(h => h.PedidoId).OnDelete(DeleteBehavior.Cascade);
        b.Navigation(p => p.Historial).HasField("_historial").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class PedidoDetalleConfiguracion : IEntityTypeConfiguration<PedidoDetalle>
{
    public void Configure(EntityTypeBuilder<PedidoDetalle> b)
    {
        b.ToTable("PedidoDetalle", t => t.HasCheckConstraint("CK_PedidoDetalle_Cantidad", "Cantidad > 0"));
        b.HasKey(l => l.Id);
        b.Property(l => l.Codigo).HasMaxLength(30).IsRequired();
        b.HasIndex(l => l.Codigo);
        b.Property(l => l.Nombre).HasMaxLength(100).IsRequired();
        b.Property(l => l.PrecioUnitario).HasPrecision(12, 2);
        b.Ignore(l => l.Subtotal);
        b.Ignore(l => l.UnidadesFisicas);
        b.HasMany(l => l.Componentes).WithOne().HasForeignKey(c => c.PedidoDetalleId).OnDelete(DeleteBehavior.Cascade);
        b.Navigation(l => l.Componentes).HasField("_componentes").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public sealed class PedidoDetalleComponenteConfiguracion : IEntityTypeConfiguration<PedidoDetalleComponente>
{
    public void Configure(EntityTypeBuilder<PedidoDetalleComponente> b)
    {
        b.ToTable("PedidoDetalleComponente");
        b.HasKey(c => c.Id);
        b.Property(c => c.Sku).HasMaxLength(30).IsRequired();
        b.HasIndex(c => c.Sku);
        b.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        b.Property(c => c.PrecioUnitario).HasPrecision(12, 2);
        b.Property(c => c.PesoKg).HasPrecision(10, 3);
    }
}

public sealed class PedidoHistorialConfiguracion : IEntityTypeConfiguration<PedidoHistorial>
{
    public void Configure(EntityTypeBuilder<PedidoHistorial> b)
    {
        b.ToTable("PedidoHistorial");
        b.HasKey(h => h.Id);
        b.Property(h => h.EstadoAnterior).HasMaxLength(20);
        b.Property(h => h.EstadoNuevo).HasMaxLength(20);
        b.Property(h => h.Usuario).HasMaxLength(40);
        b.Property(h => h.Detalle).HasMaxLength(300);
    }
}
