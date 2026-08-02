using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class clsTraduccionDAL
    {
        // Devuelve un diccionario Clave -> Texto para un idioma (por codigo: "es", "en")
        public Dictionary<string, string> GetDiccionarioPorCodigo(string codigo)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT tc.Clave, t.Texto 
                          FROM Traduccion t 
                          JOIN Idioma i ON t.IdIdioma = i.IdIdioma
                          JOIN TraduccionClave tc ON t.IdClave = tc.IdClave
                          WHERE i.Codigo = @Codigo", con);
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string clave = dr["Clave"].ToString();
                    if (!dic.ContainsKey(clave))
                        dic.Add(clave, dr["Texto"].ToString());
                }
            }
            return dic;
        }

        // Todas las traducciones de un idioma (por IdIdioma) para el ABM
        public List<clsTraduccionBE> GetByIdioma(int idIdioma)
        {
            List<clsTraduccionBE> lista = new List<clsTraduccionBE>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"
                    SELECT t.IdTraduccion, t.IdIdioma, t.Texto, t.IdClave, tc.Clave, tc.Formulario 
                    FROM Traduccion t
                    JOIN TraduccionClave tc ON t.IdClave = tc.IdCLAVE
                    WHERE t.IdIdioma = @IdIdioma
                    ORDER BY tc.Clave ASC", con);
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(Mapear(dr));
            }
            return lista;
        }

        // Todas las claves distintas que existen en el sistema
        public List<string> GetClaves()
        {
            List<string> claves = new List<string>();
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT IdClave, Clave FROM TraduccionClave ORDER BY Clave", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    claves.Add(dr["Clave"].ToString());
            }
            return claves;
        }
        public bool Update(clsTraduccionBE t)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Traduccion SET Texto = @Texto WHERE IdTraduccion = @IdTraduccion", con, tran);
                    cmd.Parameters.AddWithValue("@Texto", t.Texto);
                    cmd.Parameters.AddWithValue("@IdTraduccion", t.IdTraduccion);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }

        public bool Delete(int idTraduccion)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Traduccion WHERE IdTraduccion = @IdTraduccion", con, tran);
                    cmd.Parameters.AddWithValue("@IdTraduccion", idTraduccion);
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    return true;
                }
                catch { tran.Rollback(); return false; }
            }
        }
        public int EscanearYGenerarClaves(Dictionary<string, string> claves, Dictionary<string, string> formularios)
        {
            int count = 0;
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    // Traer todos los idiomas UNA sola vez, antes del loop
                    List<int> idsIdiomas = new List<int>();
                    SqlCommand cmdIdiomas = new SqlCommand("SELECT IdIdioma FROM Idioma", con, tran);
                    using (SqlDataReader drIdiomas = cmdIdiomas.ExecuteReader())
                    {
                        while (drIdiomas.Read())
                            idsIdiomas.Add((int)drIdiomas["IdIdioma"]);
                    }

                    foreach (KeyValuePair<string, string> kvp in claves)
                    {
                        string formulario = (formularios != null && formularios.ContainsKey(kvp.Key)) ? formularios[kvp.Key] : null;

                        SqlCommand cmdCheck = new SqlCommand(
                            "SELECT COUNT(*) FROM TraduccionClave WHERE Clave = @Clave", con, tran);
                        cmdCheck.Parameters.AddWithValue("@Clave", kvp.Key);
                        int existe = (int)cmdCheck.ExecuteScalar();

                        if (existe == 0)
                        {
                            SqlCommand cmdInsertClave = new SqlCommand(
                                "INSERT INTO TraduccionClave (Clave, Formulario) VALUES (@Clave, @Formulario); SELECT SCOPE_IDENTITY()", con, tran);
                            cmdInsertClave.Parameters.AddWithValue("@Clave", kvp.Key);
                            cmdInsertClave.Parameters.AddWithValue("@Formulario", (object)formulario ?? DBNull.Value);
                            int idClave = Convert.ToInt32(cmdInsertClave.ExecuteScalar());

                            // Insertar traducción para TODOS los idiomas — el base (1) con el texto detectado, el resto vacío
                            foreach (int idIdioma in idsIdiomas)
                            {
                                string texto = idIdioma == 1 ? kvp.Value : "";
                                SqlCommand cmdInsert = new SqlCommand(
                                    "INSERT INTO Traduccion (IdIdioma, IdClave, Texto) VALUES (@IdIdioma, @IdClave, @Texto)", con, tran);
                                cmdInsert.Parameters.AddWithValue("@IdIdioma", idIdioma);
                                cmdInsert.Parameters.AddWithValue("@IdClave", idClave);
                                cmdInsert.Parameters.AddWithValue("@Texto", texto);
                                cmdInsert.ExecuteNonQuery();
                            }
                            count++;
                        }
                        else
                        {
                            SqlCommand cmdGetId = new SqlCommand(
                                "SELECT IdClave FROM TraduccionClave WHERE Clave = @Clave", con, tran);
                            cmdGetId.Parameters.AddWithValue("@Clave", kvp.Key);
                            int idClave = Convert.ToInt32(cmdGetId.ExecuteScalar());

                            // Completar Formulario si todavía no estaba cargado, sin pisar lo que ya hubiera
                            if (!string.IsNullOrEmpty(formulario))
                            {
                                SqlCommand cmdUpdateForm = new SqlCommand(
                                    "UPDATE TraduccionClave SET Formulario = @Formulario WHERE IdClave = @IdClave AND (Formulario IS NULL OR Formulario = '')", con, tran);
                                cmdUpdateForm.Parameters.AddWithValue("@Formulario", formulario);
                                cmdUpdateForm.Parameters.AddWithValue("@IdClave", idClave);
                                cmdUpdateForm.ExecuteNonQuery();
                            }

                            if (!string.IsNullOrEmpty(kvp.Value) && kvp.Value != kvp.Key)
                            {
                                SqlCommand cmdUpdate = new SqlCommand(
                                    @"UPDATE Traduccion SET Texto = @Texto
                              WHERE IdClave = @IdClave AND IdIdioma = 1
                              AND (Texto = @Clave OR Texto = '')", con, tran);
                                cmdUpdate.Parameters.AddWithValue("@Texto", kvp.Value);
                                cmdUpdate.Parameters.AddWithValue("@IdClave", idClave);
                                cmdUpdate.Parameters.AddWithValue("@Clave", kvp.Key);
                                count += cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }
                    tran.Commit();
                }
                catch { tran.Rollback(); count = 0; }
            }
            return count;
        }

        private clsTraduccionBE Mapear(SqlDataReader dr)
        {
            return new clsTraduccionBE
            {
                IdTraduccion = (int)dr["IdTraduccion"],
                IdIdioma = (int)dr["IdIdioma"],
                Clave = dr["Clave"].ToString(),
                IdClave = (int)dr["IdClave"],
                Texto = dr["Texto"].ToString(),
                Formulario = dr["Formulario"] == DBNull.Value ? "" : dr["Formulario"].ToString()
            };
        }
    }
}