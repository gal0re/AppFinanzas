using AppFinanzas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AssetTypeConfig : IEntityTypeConfiguration<TipoActivo>
{
    public void Configure(EntityTypeBuilder<TipoActivo> b)
    {
        b.ToTable("TiposActivos");
        b.HasKey(x => x.Id);

        b.Property(x => x.Descripcion)
            .IsRequired()
            .HasMaxLength(32);

        // seed lookups
        b.HasData(
            new TipoActivo { Id = 1, Descripcion = "Acción" },
            new TipoActivo { Id = 2, Descripcion = "Bono" },
            new TipoActivo { Id = 3, Descripcion = "FCI" }
        );
    }
}
