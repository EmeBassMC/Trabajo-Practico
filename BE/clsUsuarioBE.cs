using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class clsUsuarioBE
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordHash { get; set; }
        public bool Activo { get; set; }
        public List<clsRolBE> Roles { get; set; }

        public clsUsuarioBE()
        {
            Roles = new List<clsRolBE>();
            Activo = true;
        }

    }
}