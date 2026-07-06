using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class clsRolDAL
    {          
        //Generamos el INSTER, lo que haremos con esto es poder tener el comando SQL para insertar Profesionals.
        //vamos a utilizar de referencia a la clase Profesional de la capa de negocio en la cual estan
        //las propiedades del Rol.
        public bool Insert(clsRolBE rol)
        {
            //utilizaremos el using para cerrar la conexion solo al termina, aunque el proceso se rompa

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                //abrimos la conexion a la db y luego abrimos la transacción.
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql =
                        @"INSERT INTO Rol(IdRolPadre,Nombre,EsGrupo) 
                        VALUES 
                        (@IdRolPadre,@Nombre,@EsGrupo)";
                    //ahora ejecutamos el comando que hara que los datos de la clase Profesional
                    //de la capa de negocio se "peguen" a las tablas de la DB.
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@IdRolPadre",
                        rol.IdRolPadre.HasValue ? (object)rol.IdRolPadre.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
                    cmd.Parameters.AddWithValue("@EsGrupo", rol.EsGrupo);

                    cmd.ExecuteNonQuery();

                    //si llegamos hasta este punto confirmamos la transaccion y devolvemos un true,
                    //si no tiramos rollback en el catch
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

        public bool Update(clsRolBE rol)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                //abrimos la conexion a la db y luego abrimos la transacción.
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"UPDATE Rol 
                     SET Nombre = @Nombre, 
                     EsGrupo = @EsGrupo, 
                     IdRolPadre = @IdRolPadre 
                     WHERE IdRol = @IdRol";

                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
                    cmd.Parameters.AddWithValue("@EsGrupo", rol.EsGrupo);
                    cmd.Parameters.AddWithValue("@IdRolPadre",
                        rol.IdRolPadre.HasValue ? (object)rol.IdRolPadre.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", rol.IdRol);

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
        public bool Delete(int idrol)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // 1. Borrar asignaciones de usuarios al rol
                    SqlCommand cmdUR = new SqlCommand(
                        "DELETE FROM UsuarioRol WHERE IdRol = @IdRol", con, tran);
                    cmdUR.Parameters.AddWithValue("@IdRol", idrol);
                    cmdUR.ExecuteNonQuery();

                    // 2. Borrar donde el rol es padre en RolPermiso
                    SqlCommand cmdRP1 = new SqlCommand(
                        "DELETE FROM RolPermiso WHERE IdRol = @IdRol", con, tran);
                    cmdRP1.Parameters.AddWithValue("@IdRol", idrol);
                    cmdRP1.ExecuteNonQuery();

                    // 3. Borrar donde el rol es hijo en RolPermiso
                    SqlCommand cmdRP2 = new SqlCommand(
                        "DELETE FROM RolPermiso WHERE IdPermiso = @IdRol", con, tran);
                    cmdRP2.Parameters.AddWithValue("@IdRol", idrol);
                    cmdRP2.ExecuteNonQuery();

                    // 4. Borrar hijos del rol (poner IdRolPadre en null)
                    SqlCommand cmdHijos = new SqlCommand(
                        "UPDATE Rol SET IdRolPadre = NULL WHERE IdRolPadre = @IdRol", con, tran);
                    cmdHijos.Parameters.AddWithValue("@IdRol", idrol);
                    cmdHijos.ExecuteNonQuery();

                    // 5. Borrar el rol
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Rol WHERE IdRol = @IdRol", con, tran);
                    cmd.Parameters.AddWithValue("@IdRol", idrol);
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
        public List<clsRolBE> GetAll()
        {
            List<clsRolBE> lstRol = new List<clsRolBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Rol", con);
                SqlDataReader dr = cmd.ExecuteReader();

                //while en el drread para traer toda las filas, a diferencia del if en el anterior aca traemos varios datos.
                while (dr.Read())
                {
                    lstRol.Add(Mapear(dr));
                }
                return lstRol;
            }
        }
        public bool AsignarRolUsuario(int idUsuario, int idRol)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection()) 
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction(); 
                try
                {
                    string sql = @"INSERT INTO UsuarioRol (IdUsuario, IdRol)
                           VALUES (@IdUsuario, @IdRol)";
                    SqlCommand cmd = new SqlCommand(sql, con, tran); 
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
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
        public bool QuitarRolUsuario(int idUsuario, int idRol)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"DELETE FROM UsuarioRol 
                                 WHERE IdUsuario = @IdUsuario AND IdRol = @IdRol";
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
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
        public List<clsRolBE> GetRolesPorUsuario(int idUsuario)
        {
            List<clsRolBE> lista = new List<clsRolBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT r.* FROM Rol r
              INNER JOIN UsuarioRol ur ON r.IdRol = ur.IdRol
              WHERE ur.IdUsuario = @IdUsuario", con);
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        public List<clsRolBE> GetPermisosPorRol(int idRol)
        {
            List<clsRolBE> lista = new List<clsRolBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT r.* FROM Rol r
              INNER JOIN RolPermiso rp ON r.IdRol = rp.IdPermiso
              WHERE rp.IdRol = @IdRol", con);
                cmd.Parameters.AddWithValue("@IdRol", idRol);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(Mapear(dr));
            }
            return lista;
        }

        public bool AsignarPermiso(int idRol, int idPermiso)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO RolPermiso (IdRol, IdPermiso) VALUES (@IdRol, @IdPermiso)", con, tran);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@IdPermiso", idPermiso);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
        //AGREGAMOS METODOS PARA QUE UN GRUPO TENGA LOS MISMOS ROLES QUE OTRO GRUPO
        public bool QuitarPermiso(int idRol, int idPermiso)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM RolPermiso WHERE IdRol = @IdRol AND IdPermiso = @IdPermiso", con, tran);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.Parameters.AddWithValue("@IdPermiso", idPermiso);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
        public bool QuitarTodosLosPermisosDeGrupo(int idGrupo)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM RolPermiso WHERE IdRol = @IdGrupo", con, tran);
                    cmd.Parameters.AddWithValue("@IdGrupo", idGrupo);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
        private clsRolBE Mapear(SqlDataReader dr)
        {
            return new clsRolBE
            {
                IdRol = (int)dr["IdRol"],
                Nombre = dr["Nombre"].ToString(),
                IdRolPadre = dr["IdRolPadre"] == DBNull.Value ? (int?)null : (int)dr["IdRolPadre"],
                EsGrupo = (bool)dr["EsGrupo"]
            };
        }
    }
}

