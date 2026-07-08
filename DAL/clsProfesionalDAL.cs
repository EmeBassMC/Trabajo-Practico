using System;
using System.Collections.Generic;
using BE;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;


namespace DAL
{
    public class clsProfesionalDAL
    {
        //Generamos el INSTER, lo que haremos con esto es poder tener el comando SQL para insertar Profesionals.
        //vamos a utilizar de referencia a la clase Profesional de la capa de negocio en la cual estan las propiedades del Profesional.
        public bool Insert(clsProfesionalBE profesional)
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
                        @"INSERT INTO Profesional(Nombre,Apellido,DNI,Telefono,Email,FechaNacimiento,Matricula,IdEspecialidad) 
                        VALUES 
                        (@Nombre,@Apellido,@DNI,@Telefono,@Email,@FechaNacimiento,@Matricula,@IdEspecialidad)";
                    //ahora ejecutamos el comando que hara que los datos de la clase Profesional de la capa de negocio se "peguen" a las tablas de la DB.
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@Nombre", profesional.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", profesional.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", profesional.DNI);
                    cmd.Parameters.AddWithValue("@Telefono",
                        (object)profesional.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email",
                        (object)profesional.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaNacimiento",
                        profesional.FechaNacimiento == DateTime.MinValue
                            ? (object)DBNull.Value
                            : profesional.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Matricula", profesional.Matricula);
                    cmd.Parameters.AddWithValue("@IdEspecialidad", profesional.IdEspecialidad);

                    cmd.ExecuteNonQuery();

                    //si llegamos hasta este punto confirmamos la transaccion y devolvemos un true, si no tiramos rollback en el catch
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

        public bool Update(clsProfesionalBE Profesional)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //aca va el UPDATE que se hace a la DB recordar siempre que como el IDProfesional es la FK se tomara este para buscarlo con el where
                    string sql = @"UPDATE Profesional
                        SET Nombre = @Nombre,
                            Apellido = @Apellido,
                            DNI = @DNI,
                            Telefono = @Telefono,
                            Email = @Email,
                            FechaNacimiento = @FechaNacimiento,
                            Matricula = @Matricula,
                            IdEspecialidad = @IdEspecialidad
                            WHERE IdProfesional = @IdProfesional";
                    //mismo sector de code que el INSERT
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@Nombre", Profesional.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", Profesional.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", Profesional.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", Profesional.Telefono);
                    cmd.Parameters.AddWithValue("@Email", Profesional.Email);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", Profesional.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Matricula", Profesional.Matricula);
                    cmd.Parameters.AddWithValue("@IdEspecialidad", Profesional.IdEspecialidad);
                    cmd.Parameters.AddWithValue("@IdProfesional", Profesional.IdPersona);
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
        public bool Delete(int idProfesional)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Profesional SET Activo = 0 WHERE IdProfesional = @IdProfesional", con, tran);
                    cmd.Parameters.AddWithValue("@IdProfesional", idProfesional);
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
        public bool Restaurar(int idProfesional)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Profesional SET Activo = 1 WHERE IdProfesional = @IdProfesional", con, tran);
                    cmd.Parameters.AddWithValue("@IdProfesional", idProfesional);
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
        public List<clsProfesionalBE> GetAll()
        {
            List<clsProfesionalBE> listaProfesional = new List<clsProfesionalBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.*, e.Nombre AS NombreEspecialidad
              FROM Profesional p
              INNER JOIN Especialidad e ON e.IdEspecialidad = p.IdEspecialidad
              WHERE p.Activo = 1", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listaProfesional.Add(Mapear(dr));
                }
                return listaProfesional;
            }
        }

        public List<clsProfesionalBE> GetEliminados()
        {
            List<clsProfesionalBE> lista = new List<clsProfesionalBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.*, e.Nombre AS NombreEspecialidad
              FROM Profesional p
              INNER JOIN Especialidad e ON e.IdEspecialidad = p.IdEspecialidad
              WHERE p.Activo = 0", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }

        public clsProfesionalBE GetByID(int id)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.*, e.Nombre AS NombreEspecialidad
              FROM Profesional p
              INNER JOIN Especialidad e ON e.IdEspecialidad = p.IdEspecialidad
              WHERE p.IdProfesional = @Id", con);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return Mapear(dr);
                }
            }
            return null;
        }

        public clsProfesionalBE GetByDNI(string dni)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.*, e.Nombre AS NombreEspecialidad
              FROM Profesional p
              INNER JOIN Especialidad e ON e.IdEspecialidad = p.IdEspecialidad
              WHERE p.DNI = @DNI", con);
                cmd.Parameters.AddWithValue("@DNI", dni);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return Mapear(dr);
                }
            }
            return null;
        }
        private clsProfesionalBE Mapear(SqlDataReader dr)
        {
            return new clsProfesionalBE
            {
                IdPersona = (int)dr["IdProfesional"],
                Nombre = dr["Nombre"].ToString(),
                Apellido = dr["Apellido"].ToString(),
                DNI = dr["DNI"].ToString(),
                Telefono = dr["Telefono"].ToString(),
                FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? DateTime.MinValue : (DateTime)dr["FechaNacimiento"],
                Email = dr["Email"] == DBNull.Value ? "" : dr["Email"].ToString(),
                Matricula = dr["Matricula"].ToString(),
                IdEspecialidad = (int)dr["IdEspecialidad"],
                Activo = (bool)dr["Activo"],
                NombreEspecialidad = dr["NombreEspecialidad"].ToString()  
            };
        }

    }
} 
