using System;

namespace BE
{
    public class clsCambioResumenBE
    {
        public int IdControlCambio { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioId { get; set; }
        public string Resumen { get; set; }
        public bool Restaurado { get; set; }
    }
}