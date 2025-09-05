using AppFinanzas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ActivoConfig : IEntityTypeConfiguration<Activo>
{
    public void Configure(EntityTypeBuilder<Activo> b)
    {
        b.ToTable("Activos");
        b.HasKey(x => x.Id);

        b.Property(x => x.Ticker)
            .IsRequired()
            .HasMaxLength(16);

        b.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(64);

        b.Property(x => x.PrecioUnitario)
            .HasColumnType("decimal(18,4)");

        b.HasOne(x => x.TipoActivo)
            .WithMany(t => t.Activos)
            .HasForeignKey(x => x.TipoActivoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
