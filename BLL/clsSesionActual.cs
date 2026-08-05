using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsSesionActual
    {
        private static clsSesionActual _instancia;

        private clsSesionActual() { }

        public static clsSesionActual GetInstancia()
        {
            if (_instancia == null)
                _instancia = new clsSesionActual();
            return _instancia;
        }

        public static void CerrarSesion()
        {
            _instancia = null;
        }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
    }
}
