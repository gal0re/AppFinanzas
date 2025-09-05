using AppFinanzas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class InvestmentOrderConfig : IEntityTypeConfiguration<OrdenInversion>
{
    public void Configure(EntityTypeBuilder<OrdenInversion> b)
    {
        b.ToTable("Ordenes");
        b.HasKey(x => x.Id);

        b.Property(x => x.IdCuenta).IsRequired();
        b.Property(x => x.IdActivo).IsRequired();

        b.Property(x => x.NombreActivo)
            .IsRequired()
            .HasMaxLength(32);

        b.Property(x => x.Cantidad)
            .IsRequired();

        b.Property(x => x.Precio)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        b.Property(x => x.MontoTotal)
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        b.Property(x => x.Operacion)
            .IsRequired()
            .HasMaxLength(1);

        b.Property(x => x.EstadoId)
            .HasDefaultValue(0);

        // ForeingKey
        b.HasOne(x => x.Activo)
            .WithMany(x => x.Ordenes)
            .HasForeignKey(x => x.IdActivo)
            /* Debo restringir la eliminación de Activos. 
             * Aplico "Restrict" para que la BD no permita campos null de IdActivo en la tabla OrdenInversion */
            .OnDelete(DeleteBehavior.Restrict);
           

        b.HasOne(x => x.Estado)
            .WithMany(e => e.Ordenes)
            .HasForeignKey(x => x.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
