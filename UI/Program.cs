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

            bool seguirMostrandoLogin = true;
            while (seguirMostrandoLogin)
            {
                frmLogin login = new frmLogin(integridadOk);
                login.ShowDialog();

                if (clsSesionActual.GetInstancia().IdUsuario > 0)
                {
                    Application.Run(new frmPrincipal());

                    // Al volver aca, frmPrincipal ya se cerro.
                    // Si la sesion quedo en 0, fue un Logout desde "Salir" -> volvemos a mostrar el login.
                    // Si la sesion sigue con datos, se cerro la ventana con la X -> se termina la app.
                    seguirMostrandoLogin = clsSesionActual.GetInstancia().IdUsuario == 0;
                }
                else
                {
                    seguirMostrandoLogin = false; // cancelo o cerro el login sin loguearse: se termina la app
                }
            }
        }
    }
}
