using System;
using System.Data.SqlClient;

namespace DAL
{
    // Backup/Restore de la base completa, para poder sacar una copia de TurnoSync
    // y poder levantarla en otra instalación (otra PC, otro servidor).
    public class clsBackupDAL
    {
        // Nombre de la base tal cual figura en el connection string de clsConexionDAL 
        private const string NombreBaseDeDatos = "TurnoSync";

        // No se puede hacer BACKUP ni RESTORE de una base mientras hay una conexión abierta contra ella
        // (y la conexión "normal" de la app siempre apunta a TurnoSync). Por eso acá usamos una conexión
        // aparte, contra la base master, igual que hace clsConexionDAL pero cambiando el Initial Catalog.
        private static string ConnectionStringMaster =>
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;";

        public bool HacerBackup(string rutaArchivo)
        {
            using (SqlConnection con = new SqlConnection(ConnectionStringMaster))
            {
                con.Open();

                string sql = $@"BACKUP DATABASE [{NombreBaseDeDatos}] 
                                 TO DISK = @Ruta 
                                 WITH FORMAT, INIT, NAME = 'Backup completo de TurnoSync';";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandTimeout = 120; // un backup puede tardar mas que el timeout default de 30s
                    cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public bool RestaurarBackup(string rutaArchivo)
        {
            using (SqlConnection con = new SqlConnection(ConnectionStringMaster))
            {
                con.Open();

                // Paso 1: pasar la base a SINGLE_USER para poder restaurarla.
                // ROLLBACK IMMEDIATE corta cualquier otra conexión activa contra TurnoSync
                // (incluida la que la propia aplicación tenía abierta hasta hace un segundo).
                EjecutarComando(con, $"ALTER DATABASE [{NombreBaseDeDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");

                try
                {
                    string sqlRestore = $@"RESTORE DATABASE [{NombreBaseDeDatos}] 
                                            FROM DISK = @Ruta 
                                            WITH REPLACE;";

                    using (SqlCommand cmd = new SqlCommand(sqlRestore, con))
                    {
                        cmd.CommandTimeout = 120;
                        cmd.Parameters.AddWithValue("@Ruta", rutaArchivo);
                        cmd.ExecuteNonQuery();
                    }

                    return true;
                }
                finally
                {
                    // Paso 2: pase lo que pase (haya funcionado el restore o haya tirado excepción),
                    // siempre hay que devolver la base a MULTI_USER, si no queda inutilizable.
                    EjecutarComando(con, $"ALTER DATABASE [{NombreBaseDeDatos}] SET MULTI_USER;");
                }
            }
        }

        private void EjecutarComando(SqlConnection con, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}