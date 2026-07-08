using System;

namespace BE
{
    public class clsControlCambioBE
    {
        public int IdControlCambio { get; set; }
        public string Tabla { get; set; }
        public int IdRegistro { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioId { get; set; }
        public string DatosAnteriores { get; set; }
        public bool Restaurado { get; set; }
    }
}