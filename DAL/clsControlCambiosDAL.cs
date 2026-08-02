using BE;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace DAL
{
    public class clsControlCambiosDAL
    {
        public bool Insert(clsControlCambioBE cambio)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO ControlCambios (Tabla, IdRegistro, FechaCambio, UsuarioId, DatosAnteriores, Restaurado) " +
                        "VALUES (@Tabla, @IdRegistro, @FechaCambio, @UsuarioId, @DatosAnteriores, 0)", con, tran);
                    cmd.Parameters.AddWithValue("@Tabla", cambio.Tabla);
                    cmd.Parameters.AddWithValue("@IdRegistro", cambio.IdRegistro);
                    cmd.Parameters.AddWithValue("@FechaCambio", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioId", cambio.UsuarioId);
                    cmd.Parameters.AddWithValue("@DatosAnteriores", cambio.DatosAnteriores);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        public List<clsControlCambioBE> GetHistorial(string tabla, int idRegistro)
        {
            List<clsControlCambioBE> lista = new List<clsControlCambioBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM ControlCambios WHERE Tabla = @Tabla AND IdRegistro = @IdRegistro AND Restaurado = 0 ORDER BY FechaCambio DESC, IdControlCambio DESC", con);
                cmd.Parameters.AddWithValue("@Tabla", tabla);
                cmd.Parameters.AddWithValue("@IdRegistro", idRegistro);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) lista.Add(Mapear(dr));
            }
            return lista;
        }

        public clsControlCambioBE GetById(int id)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM ControlCambios WHERE IdControlCambio = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) return Mapear(dr);
            }
            return null;
        }

        public bool MarcarRestaurado(int id)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE ControlCambios SET Restaurado = 1 WHERE IdControlCambio = @Id", con, tran);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        // Marca como restaurados TODOS los cambios pendientes de un objeto (no solo el elegido).
        // Al restaurar, se resuelve toda la cola de cambios pendientes de ese registro,
        // así los "anteriores" no siguen figurando en la lista.
        public bool MarcarTodosRestaurados(string tabla, int idRegistro)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE ControlCambios SET Restaurado = 1 WHERE Tabla = @Tabla AND IdRegistro = @IdRegistro AND Restaurado = 0", con, tran);
                    cmd.Parameters.AddWithValue("@Tabla", tabla);
                    cmd.Parameters.AddWithValue("@IdRegistro", idRegistro);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        private clsControlCambioBE Mapear(SqlDataReader dr)
        {
            return new clsControlCambioBE
            {
                IdControlCambio = (int)dr["IdControlCambio"],
                Tabla = dr["Tabla"].ToString(),
                IdRegistro = (int)dr["IdRegistro"],
                FechaCambio = (DateTime)dr["FechaCambio"],
                UsuarioId = (int)dr["UsuarioId"],
                DatosAnteriores = dr["DatosAnteriores"].ToString(),
                Restaurado = (bool)dr["Restaurado"]
            };
        }
    }
}