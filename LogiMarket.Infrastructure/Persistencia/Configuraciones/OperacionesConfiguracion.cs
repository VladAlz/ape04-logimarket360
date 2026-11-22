using LogiMarket.Domain.Auditoria;
using LogiMarket.Domain.Despachos;
using LogiMarket.Domain.Devoluciones;
using LogiMarket.Domain.Eventos;
using LogiMarket.Domain.Inventario;
using LogiMarket.Domain.Pagos;
using LogiMarket.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class PagoConfiguracion : IEntityTypeConfiguration<Pago>
{
    public void Configure(EntityTypeBuilder<Pago> b)
    {
        b.ToTable("Pago");
        b.HasKey(p => p.Id);
        b.HasOne<Pedido>().WithMany().HasForeignKey(p => p.PedidoId).OnDelete(DeleteBehavior.Restrict);
        b.Property(p => p.Monto).HasPrecision(12, 2);
        b.Property(p => p.ClaveIdempotencia).HasMaxLength(60).IsRequired();
        b.Property(p => p.Referencia).HasMaxLength(60).IsRequired();
        b.Property(p => p.Proveedor).HasMaxLength(30);
        // Una clave, una referencia y un solo pago aprobado por pedido
        b.HasIndex(p => p.ClaveIdempotencia).IsUnique();
        b.HasIndex(p => p.Referencia).IsUnique();
        b.HasIndex(p => p.PedidoId).IsUnique();
    }
}

public sealed class MovimientoInventarioConfiguracion : IEntityTypeConfiguration<MovimientoInventario>
{
    public void Configure(EntityTypeBuilder<MovimientoInventario> b)
    {
        b.ToTable("MovimientoInventario", t => t.HasCheckConstraint("CK_Movimiento_Stock", "StockResultante >= 0"));
        b.HasKey(m => m.Id);
        b.Property(m => m.Sku).HasMaxLength(30).IsRequired();
        b.HasIndex(m => m.Sku);
        b.Property(m => m.Motivo).HasMaxLength(20).IsRequired();
        b.Property(m => m.Referencia).HasMaxLength(60);
        b.HasIndex(m => m.Referencia);
        b.Property(m => m.Usuario).HasMaxLength(40);
    }
}

public sealed class DespachoConfiguracion : IEntityTypeConfiguration<Despacho>
{
    public void Configure(EntityTypeBuilder<Despacho> b)
    {
        b.ToTable("Despacho");
        b.HasKey(d => d.Id);
        b.HasOne<Pedido>().WithMany().HasForeignKey(d => d.PedidoId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(d => d.PedidoId).IsUnique();
        b.Property(d => d.NumeroGuia).HasMaxLength(40).IsRequired();
        b.HasIndex(d => d.NumeroGuia).IsUnique();
        b.Property(d => d.Familia).HasMaxLength(20);
        b.Property(d => d.Etiqueta).HasMaxLength(200);
    }
}

public sealed class DevolucionConfiguracion : IEntityTypeConfiguration<Devolucion>
{
    public void Configure(EntityTypeBuilder<Devolucion> b)
    {
        b.ToTable("Devolucion");
        b.HasKey(d => d.Id);
        b.HasOne<Pedido>().WithMany().HasForeignKey(d => d.PedidoId).OnDelete(DeleteBehavior.Restrict);
        // Devolucion unica por pedido
        b.HasIndex(d => d.PedidoId).IsUnique();
        b.Property(d => d.Motivo).HasConversion<string>().HasMaxLength(20);
        b.Property(d => d.Reembolso).HasPrecision(12, 2);
        b.Property(d => d.ReferenciaReembolso).HasMaxLength(40).IsRequired();
        b.HasIndex(d => d.ReferenciaReembolso).IsUnique();
        b.Property(d => d.Usuario).HasMaxLength(40);
    }
}

public sealed class EventoOutboxConfiguracion : IEntityTypeConfiguration<EventoOutbox>
{
    public void Configure(EntityTypeBuilder<EventoOutbox> b)
    {
        b.ToTable("EventoOutbox");
        b.HasKey(e => e.Id);
        b.Property(e => e.Tipo).HasMaxLength(40).IsRequired();
        b.Property(e => e.Contenido).HasMaxLength(1000);
        b.Property(e => e.UltimoError).HasMaxLength(300);
        b.HasIndex(e => e.Procesado);
        b.HasIndex(e => new { e.Tipo, e.PedidoId });
    }
}

public sealed class RegistroAuditoriaConfiguracion : IEntityTypeConfiguration<RegistroAuditoria>
{
    public void Configure(EntityTypeBuilder<RegistroAuditoria> b)
    {
        b.ToTable("Auditoria");
        b.HasKey(r => r.Id);
        b.Property(r => r.Categoria).HasMaxLength(20).IsRequired();
        b.HasIndex(r => r.Categoria);
        b.Property(r => r.Usuario).HasMaxLength(40);
        b.Property(r => r.Mensaje).HasMaxLength(500).IsRequired();
        b.Property(r => r.Correlacion).HasMaxLength(60);
    }
}
