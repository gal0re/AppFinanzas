using System;

namespace AppFinanzas.Domain.Entities
{
    public class TipoActivo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;


        // Entidades relacionadas
        public ICollection<Activo> Activos { get; set; }
    }

}
