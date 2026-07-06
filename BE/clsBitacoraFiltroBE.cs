using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class clsBitacoraFiltroBE
    {
        public DateTime? FechaDesde {  get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? UsuarioId { get; set; }
        public string? Actividad {  get; set; }
    }
}
