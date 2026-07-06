using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using System.Data.SqlClient;

namespace DAL
{
    public class clsUsuarioDAL
    {
        public bool Insert(clsUsuarioBE usuario)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = "INSERT INTO Usuario(NombreUsuario, PasswordHash) VALUES (@NombreUsuario,@PasswordHash)";
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);

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
        public bool Delete(int idUsuario)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Usuario SET Activo = 0 WHERE IdUsuario = @IdUsuario", con, tran);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
        public bool Restaurar(int idUsuario)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Usuario SET Activo = 1 WHERE IdUsuario = @IdUsuario", con, tran);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
        public clsUsuarioBE GetByUsername(string NombreUsuario)
        {
            clsUsuarioBE Usuario = null;

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @NombreUsuario AND Activo = 1", con);
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Usuario = Mapear(dr);
                }
            }
            return Usuario;
        }
        public List<clsUsuarioBE> GetAll()
        {
            List<clsUsuarioBE> lista = new List<clsUsuarioBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Activo = 1", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        public List<clsUsuarioBE> GetEliminados()
        {
            List<clsUsuarioBE> lista = new List<clsUsuarioBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Activo = 0", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        public bool QuitarRolesUsuario(int idUsuario)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM UsuarioRol WHERE IdUsuario = @IdUsuario", con, tran);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
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
        private clsUsuarioBE Mapear(SqlDataReader dr)
        {
            return new clsUsuarioBE
            {
                IdUsuario = (int)dr["IdUsuario"],
                NombreUsuario = dr["NombreUsuario"].ToString(),
                PasswordHash = dr["PasswordHash"].ToString(),
                Activo = (bool)dr["Activo"]
            };
        }
    }
}