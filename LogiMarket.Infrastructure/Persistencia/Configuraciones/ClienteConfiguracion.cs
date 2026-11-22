using LogiMarket.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class ClienteConfiguracion : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> b)
    {
        b.ToTable("Cliente");
        b.HasKey(c => c.Id);
        b.Property(c => c.Nombre).HasMaxLength(120).IsRequired();
        b.Property(c => c.Identificacion).HasMaxLength(20).IsRequired();
        b.HasIndex(c => c.Identificacion).IsUnique();
        b.HasIndex(c => c.Nombre);
        b.Property(c => c.Email).HasMaxLength(120);
        b.Property(c => c.Telefono).HasMaxLength(20);
        b.Property(c => c.Categoria).HasConversion<string>().HasMaxLength(20);
        b.Property(c => c.Canal).HasConversion<string>().HasMaxLength(10);
        b.Ignore(c => c.Destino);
    }
}
