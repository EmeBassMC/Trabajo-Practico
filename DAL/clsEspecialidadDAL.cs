using BE;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsEspecialidadDAL
    {

        #region Escritura
        public bool Insert(clsEspecialidadBE Especialidad)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = "INSERT INTO Especialidad (Nombre) VALUES (@Nombre)";
                    SqlCommand cmd = new SqlCommand(sql, con,tran);
                    
                    cmd.Parameters.AddWithValue("@Nombre", Especialidad.Nombre);
                    
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
        public bool Update(clsEspecialidadBE EspecialidadUpdate)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"UPDATE Especialidad 
                    SET Nombre = @Nombre
                    WHERE IdEspecialidad = @IdEspecialidad";

                    SqlCommand cmd = new SqlCommand(sql,con,tran);                   
                    cmd.Parameters.AddWithValue("@Nombre", EspecialidadUpdate.Nombre);
                    cmd.Parameters.AddWithValue("@IdEspecialidad", EspecialidadUpdate.IdEspecialidad); 

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
        public bool Delete(int IdEspecialidad)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Especialidad SET Activo = 0 WHERE IdEspecialidad = @IdEspecialidad", con, tran);
                    cmd.Parameters.AddWithValue("@IdEspecialidad", IdEspecialidad);
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
        public bool Restaurar(int IdEspecialidad)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE Especialidad SET Activo = 1 WHERE IdEspecialidad = @IdEspecialidad", con, tran);
                    cmd.Parameters.AddWithValue("@IdEspecialidad", IdEspecialidad);
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
        #endregion
        #region Lectura
        public clsEspecialidadBE GetByID(int IdEspecialidad) 
        {
            clsEspecialidadBE Especialidad = null;
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Especialidad WHERE IdEspecialidad = @IdEspecialidad", con);
                cmd.Parameters.AddWithValue("@IdEspecialidad", IdEspecialidad);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Especialidad = Mapear(dr);
                }
                return Especialidad;
            }
        }
        public List<clsEspecialidadBE> GetAll()
        {
            List<clsEspecialidadBE> listaEspecialidades = new List<clsEspecialidadBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Especialidad WHERE Activo = 1", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listaEspecialidades.Add(Mapear(dr));
                }
                return listaEspecialidades;
            }
        }
        public List<clsEspecialidadBE> GetEliminadas()
        {
            List<clsEspecialidadBE> lista = new List<clsEspecialidadBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Especialidad WHERE Activo = 0", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        #endregion

        #region Mapper
        private clsEspecialidadBE Mapear(SqlDataReader dr)
        {
            return new clsEspecialidadBE
            {
                IdEspecialidad = (int)dr["IdEspecialidad"],
                Nombre = dr["Nombre"].ToString(),
                Activo = (bool)dr["Activo"]
            };
        }
        #endregion
    }
}
