using AppFinanzas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppFinanzas.Infrastructure
{ 
    public sealed class AssetTypeConfig : IEntityTypeConfiguration<TipoActivo>
    {
        public void Configure(EntityTypeBuilder<TipoActivo> b)
        {
            b.ToTable("TiposActivos");
            b.HasKey(x => x.Id);

            b.Property(x => x.Id).ValueGeneratedNever();
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
}