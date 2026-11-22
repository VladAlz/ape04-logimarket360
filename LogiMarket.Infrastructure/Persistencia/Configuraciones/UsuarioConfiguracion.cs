using LogiMarket.Domain.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogiMarket.Infrastructure.Persistencia.Configuraciones;

public sealed class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("Usuario");
        b.HasKey(u => u.Id);
        b.Property(u => u.NombreUsuario).HasMaxLength(40).IsRequired();
        b.HasIndex(u => u.NombreUsuario).IsUnique();
        b.Property(u => u.NombreCompleto).HasMaxLength(100).IsRequired();
        b.Property(u => u.Rol).HasConversion<string>().HasMaxLength(20);
        b.Property(u => u.HashContrasena).HasMaxLength(200).IsRequired();
    }
}
