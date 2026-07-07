using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class clsIdiomaDAL
    {
        public List<clsIdiomaBE> GetAll()
        {
            List<clsIdiomaBE> lista = new List<clsIdiomaBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Idioma", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(Mapear(dr));
            }
            return lista;
        }

        public bool Insert(clsIdiomaBE idioma)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Idioma (Codigo, Nombre) VALUES (@Codigo, @Nombre)", con, tran);
                    cmd.Parameters.AddWithValue("@Codigo", idioma.Codigo);
                    cmd.Parameters.AddWithValue("@Nombre", idioma.Nombre);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        public bool Delete(int idIdioma)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // primero borrar las traducciones del idioma
                    SqlCommand cmdT = new SqlCommand(
                        "DELETE FROM Traduccion WHERE IdIdioma = @IdIdioma", con, tran);
                    cmdT.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmdT.ExecuteNonQuery();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Idioma WHERE IdIdioma = @IdIdioma", con, tran);
                    cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
        public bool Update(clsIdiomaBE idioma)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Idioma SET Codigo = @Codigo, Nombre = @Nombre WHERE IdIdioma = @IdIdioma", con, tran);
                    cmd.Parameters.AddWithValue("@Codigo", idioma.Codigo);
                    cmd.Parameters.AddWithValue("@Nombre", idioma.Nombre);
                    cmd.Parameters.AddWithValue("@IdIdioma", idioma.IdIdioma);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        public bool CrearClavesVaciasParaIdioma(int idIdiomaNuevo)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Traduccion (IdIdioma, IdClave, Texto)
                  SELECT @IdNuevo, tc.IdClave, ''
                  FROM TraduccionClave tc
                  WHERE tc.IdClave NOT IN (
                      SELECT IdClave FROM Traduccion WHERE IdIdioma = @IdNuevo
                  )", con, tran);
                    cmd.Parameters.AddWithValue("@IdNuevo", idIdiomaNuevo);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        private clsIdiomaBE Mapear(SqlDataReader dr)
        {
            return new clsIdiomaBE
            {
                IdIdioma = (int)dr["IdIdioma"],
                Codigo = dr["Codigo"].ToString(),
                Nombre = dr["Nombre"].ToString()
            };
        }
    }
}