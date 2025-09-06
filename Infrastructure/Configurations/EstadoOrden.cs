using AppFinanzas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppFinanzas.Infrastructure
{
    public sealed class OrderStatusConfig : IEntityTypeConfiguration<EstadoOrden>
    {
        public void Configure(EntityTypeBuilder<EstadoOrden> b)
        {
            b.ToTable("EstadosOrden");
            b.HasKey(x => x.Id);

            b.Property(x => x.Id).ValueGeneratedNever();
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
}
