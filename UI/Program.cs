using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool integridadOk = BLL.clsDigitoVerificador.VerificarIntegridad();

            // Antes esto cortaba la app entera con return si integridadOk era false,
            // dejando afuera hasta al propio administrador. Ahora siempre se muestra
            // el login, y es frmLogin quien decide si te deja pasar o no segun tu permiso.
            frmLogin login = new frmLogin(integridadOk);
            login.ShowDialog();

            if (clsSesionActual.GetInstancia().IdUsuario > 0)
                Application.Run(new frmPrincipal());
        }
    }
}
