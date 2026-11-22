using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LogiMarket.Infrastructure.Persistencia;

// Solo la usa dotnet ef para generar migraciones
public sealed class FabricaContextoDiseno : IDesignTimeDbContextFactory<LogiMarketDbContext>
{
    public LogiMarketDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<LogiMarketDbContext>().UseSqlite("Data Source=logimarket_diseno.db").Options);
}
