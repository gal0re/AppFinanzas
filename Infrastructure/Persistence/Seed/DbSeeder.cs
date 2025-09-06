using AppFinanzas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppFinanzas.Infrastructure.Persistence.Seed
{ 
    public static class DbSeeder
    {
        public static async Task RunAsync(FinanzasContext db)
        {
            if (await db.Activos.AnyAsync()) return;

            db.Activos.AddRange(
                new Activo { Id = 1, Ticker = "AAPL", Nombre = "Apple", TipoActivoId = 1, PrecioUnitario = 177.97m },
                new Activo { Id = 2, Ticker = "GOOGL", Nombre = "Alphabet Inc", TipoActivoId = 1, PrecioUnitario = 138.21m },
                new Activo { Id = 3, Ticker = "MSFT", Nombre = "Microsoft", TipoActivoId = 1, PrecioUnitario = 329.04m },
                new Activo { Id = 4, Ticker = "KO", Nombre = "Coca Cola", TipoActivoId = 1, PrecioUnitario = 58.30m },
                new Activo { Id = 5, Ticker = "WMT", Nombre = "Walmart", TipoActivoId = 1, PrecioUnitario = 163.42m },
                new Activo { Id = 6, Ticker = "AL30", Nombre = "BONOS ARGENTINA USD 2030 L.A", TipoActivoId = 2, PrecioUnitario = 307.4m },
                new Activo { Id = 7, Ticker = "GD30", Nombre = "Bonos Globales Argentina USD Step Up 2030", TipoActivoId = 2, PrecioUnitario = 336.1m },
                new Activo { Id = 8, Ticker = "Delta.Pesos", Nombre = "Delta Pesos Clase A", TipoActivoId = 3, PrecioUnitario = 0.0181m },
                new Activo { Id = 9, Ticker = "Fima.Premium", Nombre = "Fima Premium Clase A", TipoActivoId = 3, PrecioUnitario = 0.0317m }
            );

            await db.SaveChangesAsync();
        }
    }
}