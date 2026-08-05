using System;
using System.IO;
using BE;
using DAL;

namespace BLL
{
    public class clsBackupBLL
    {
        private clsBackupDAL dal = new clsBackupDAL();

        public bool HacerBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
                throw new ArgumentException("Debe indicar una ruta de archivo para el backup.");

            string carpeta = Path.GetDirectoryName(rutaArchivo);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
                throw new ArgumentException("La carpeta de destino no existe: " + carpeta);

            bool resultado;
            try
            {
                resultado = dal.HacerBackup(rutaArchivo);
            }
            catch (Exception ex)
            {
                RegistrarEnBitacora("Backup de Base de Datos", "ERROR - " + ex.Message);
                throw;
            }

            RegistrarEnBitacora("Backup de Base de Datos", resultado ? ("OK - " + rutaArchivo) : "ERROR");
            return resultado;
        }

        public bool RestaurarBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                throw new ArgumentException("No se encontró el archivo de backup indicado.");

            bool resultado;
            try
            {
                resultado = dal.RestaurarBackup(rutaArchivo);
            }
            catch (Exception ex)
            {
                RegistrarEnBitacora("Restauracion de Base de Datos", "ERROR - " + ex.Message);
                throw;
            }

            RegistrarEnBitacora("Restauracion de Base de Datos", resultado ? ("OK - " + rutaArchivo) : "ERROR");

            // Después de restaurar, la base pudo haber vuelto a un estado distinto
            // (otro usuario, otra contraseña, o directamente sin el usuario actual).
            // La sesión en memoria ya no es confiable: se fuerza el logout.
            clsSesionActual.GetInstancia().IdUsuario = 0;
            clsSesionActual.GetInstancia().NombreUsuario = null;

            return resultado;
        }

        private void RegistrarEnBitacora(string actividad, string informacion)
        {
            var bitacora = new clsBitacoraBE
            {
                UsuarioId = clsSesionActual.GetInstancia().IdUsuario,
                Actividad = actividad,
                Informacion = informacion
            };
            clsBitacoraBLL.Registrar(bitacora);
        }
    }
}