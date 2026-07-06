using System;
using System.Collections.Generic;
using BE;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;


namespace DAL
{
    public class clsPacienteDAL
    {
        //Generamos el INSTER, lo que haremos con esto es poder tener el comando SQL para insertar pacientes.
        //vamos a utilizar de referencia a la clase Paciente de la capa de negocio en la cual estan las propiedades del paciente.
        public bool Insert(clsPacienteBE paciente)
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
                        @"INSERT INTO Paciente(Nombre,Apellido,DNI,Telefono,Email,FechaNacimiento,ObraSocial,DVH) 
                        VALUES 
                        (@Nombre,@Apellido,@DNI,@Telefono,@Email,@FechaNacimiento,@ObraSocial,@DVH)";
                    //ahora ejecutamos el comando que hara que los datos de la clase Paciente de la capa de negocio se "peguen" a las tablas de la DB.
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", paciente.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", paciente.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                    cmd.Parameters.AddWithValue("@Email", paciente.Email);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@ObraSocial", paciente.ObraSocial);
                    cmd.Parameters.AddWithValue("@DVH", paciente.DVH);

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

        public bool Update(clsPacienteBE paciente)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    //aca va el UPDATE que se hace a la DB recordar siempre que como el IDPaciente es la FK se tomara este para buscarlo con el where
                    string sql = @"UPDATE Paciente
                            SET 
                            Nombre = @Nombre,
                            Apellido = @Apellido,
                            DNI = @DNI,
                            Telefono = @Telefono,
                            Email = @Email,
                            FechaNacimiento = @FechaNacimiento,
                            ObraSocial = @ObraSocial,
                            DVH = @DVH
                            WHERE IdPaciente = @IdPaciente";
                    //mismo sector de code que el INSERT
                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", paciente.Apellido);
                    cmd.Parameters.AddWithValue("@DNI", paciente.DNI);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                    cmd.Parameters.AddWithValue("@Email", paciente.Email);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@ObraSocial", paciente.ObraSocial);
                    cmd.Parameters.AddWithValue("@IdPaciente", paciente.IdPersona);
                    cmd.Parameters.AddWithValue("@DVH", paciente.DVH);
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
        public bool Delete(int idPaciente)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Paciente SET Activo = 0 WHERE IdPaciente = @IdPaciente", con, tran);
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
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
        public bool Restaurar(int idPaciente)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand
                        ("UPDATE Paciente SET Activo = 1 WHERE IdPaciente = @IdPaciente", con, tran);
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);
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
        public List<clsPacienteBE> GetEliminados()
        {
            List<clsPacienteBE> lista = new List<clsPacienteBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Paciente WHERE Activo = 0", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        //metodos de solo lectura para mostrar información de la DB, GetById, GetAll, GetByDNI
        public clsPacienteBE GetByID(int IdPaciente)
        {
            clsPacienteBE paciente = null; //si el paciente esta vacio va a devolver siempre null
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Paciente WHERE IdPaciente = @IdPaciente", con);
                cmd.Parameters.AddWithValue("@IdPaciente", IdPaciente);
                SqlDataReader dr = cmd.ExecuteReader();
                //hacemos un if para pasar el dr.read porque la idea es que nos devuelva una sola fila
                if (dr.Read())
                {
                    paciente = Mapear(dr);
                }
                return paciente;
            }
        }
        public List<clsPacienteBE> GetAll()
        {
            List<clsPacienteBE> listaPaciente = new List<clsPacienteBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Paciente WHERE Activo = 1", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listaPaciente.Add(Mapear(dr));
                }
                return listaPaciente;
            }
        }
        public clsPacienteBE GetByDNI(string dni)
        {
            clsPacienteBE paciente = null;

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Paciente WHERE DNI = @DNI", con);
                cmd.Parameters.AddWithValue("@DNI", dni);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    paciente = Mapear(dr);
                }
            }
            return paciente;
        }
        private clsPacienteBE Mapear(SqlDataReader dr)
        {
            return new clsPacienteBE
            {
                IdPersona = (int)dr["IdPaciente"],
                Nombre = dr["Nombre"].ToString(),
                Apellido = dr["Apellido"].ToString(),
                DNI = dr["DNI"].ToString(),
                Telefono = dr["Telefono"].ToString(),
                Email = dr["Email"].ToString(),
                FechaNacimiento = (DateTime)dr["FechaNacimiento"],
                ObraSocial = dr["ObraSocial"].ToString(),
                DVH = (int)dr["DVH"],
                Activo = (bool)dr["Activo"]
            };
        }

    }
} 
