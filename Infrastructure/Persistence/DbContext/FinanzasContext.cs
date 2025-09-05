using AppFinanzas.Models;
using Microsoft.EntityFrameworkCore;

public class FinanzasContext : DbContext
{
    public FinanzasContext(DbContextOptions<FinanzasContext> options) : base(options) { }

    public DbSet<Activo> Activos => Set<Activo>();
    public DbSet<TipoActivo> TipoActivo => Set<TipoActivo>();
    public DbSet<OrdenInversion> Ordenes => Set<OrdenInversion>();
    public DbSet<EstadoOrden> EstadoOrdenes => Set<EstadoOrden>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanzasContext).Assembly);
    }
}
