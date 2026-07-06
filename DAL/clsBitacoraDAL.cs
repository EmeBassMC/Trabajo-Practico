using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DAL
{
    /*
    Insert(int usuarioId, string actividad, string informacion): bool
    GetAll(): List<clsBitacoraBE>
    */
    public class clsBitacoraDAL
    {
        public bool Insert(clsBitacoraBE bitacora)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"INSERT INTO Bitacora 
                                   (UsuarioId,Actividad,Informacion)
                                   VALUES 
                                   (@UsuarioID,@Actividad,@Informacion)";

                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@UsuarioId", bitacora.UsuarioId);
                    cmd.Parameters.AddWithValue("@Actividad", bitacora.Actividad);
                    cmd.Parameters.AddWithValue("@Informacion", bitacora.Informacion);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }           
        }
        public List<clsBitacoraBE> GetAll()
        {
            List<clsBitacoraBE> lista = new List<clsBitacoraBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Bitacora", con);
                SqlDataReader dr = cmd.ExecuteReader(); // ← falta esto
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        public List<clsBitacoraBE> GetFiltrado(clsBitacoraFiltroBE filtro)
        {
            List<clsBitacoraBE> lista = new List<clsBitacoraBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                string sql = @"SELECT * FROM Bitacora
                       WHERE (@FechaDesde IS NULL OR Fecha >= @FechaDesde)
                         AND (@FechaHasta IS NULL OR Fecha <= @FechaHasta)
                         AND (@UsuarioId IS NULL OR UsuarioId = @UsuarioId)
                         AND (@Actividad IS NULL OR Actividad = @Actividad)
                       ORDER BY Fecha DESC";

                // Si hay "hasta", lo llevamos al final de ese día (23:59:59) para que
                // el filtro incluya todos los registros de esa fecha, no solo hasta
                // las 00:00:00 (que es lo que trae un DateTimePicker por defecto).
                DateTime? fechaHastaFinDia = filtro.FechaHasta?.Date.AddDays(1).AddSeconds(-1);

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@FechaDesde", (object)filtro.FechaDesde ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaHasta", (object)fechaHastaFinDia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UsuarioId", (object)filtro.UsuarioId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Actividad", (object)filtro.Actividad ?? DBNull.Value);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(Mapear(dr));
            }
            return lista;
        }

        public List<string> GetActividadesDistintas()
        {
            List<string> lista = new List<string>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT DISTINCT Actividad FROM Bitacora ORDER BY Actividad", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(dr["Actividad"].ToString());
            }
            return lista;
        }
        #region Mapper
        private clsBitacoraBE Mapear(SqlDataReader dr)
        {
            return new clsBitacoraBE
            {
                Id = (int)dr["Id"],
                Fecha = (DateTime)dr["Fecha"],
                UsuarioId = dr["UsuarioId"] == DBNull.Value ? 0 : (int)dr["UsuarioId"],
                Actividad = dr["Actividad"].ToString(),
                Informacion = dr["Informacion"] == DBNull.Value ? "" : dr["Informacion"].ToString()
            };
        }
        #endregion
    }
}
