using AppFinanzas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class OrderStatusConfig : IEntityTypeConfiguration<EstadoOrden>
{
    public void Configure(EntityTypeBuilder<EstadoOrden> b)
    {
        b.ToTable("EstadosOrden");
        b.HasKey(x => x.Id);

        b.Property(x => x.DescripcionEstado)
            .IsRequired()
            .HasMaxLength(32);

        // seed lookups
        b.HasData(
            new EstadoOrden { Id = 0, DescripcionEstado = "En proceso" },
            new EstadoOrden { Id = 1, DescripcionEstado = "Ejecutada" },
            new EstadoOrden { Id = 3, DescripcionEstado = "Cancelada" }
        );
    }
}
