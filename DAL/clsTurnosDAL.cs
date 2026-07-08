using System;
using System.Collections.Generic;
using System.Data;
using BE;
using System.Data.SqlClient;

namespace DAL
{
    public class clsTurnosDAL
    {
        #region metodos de escritura
        public bool Insert(clsTurnoBE turno)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"INSERT INTO Turno 
                                   (IdPaciente, IdProfesional, FechaHora, Estado, Observaciones)
                                   VALUES 
                                   (@IdPaciente, @IdProfesional, @FechaHora, @Estado, @Observaciones)";

                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@IdPaciente", turno.IDPaciente);
                    cmd.Parameters.AddWithValue("@IdProfesional", turno.IDProfesional);
                    cmd.Parameters.AddWithValue("@FechaHora", turno.FechaHora);
                    cmd.Parameters.AddWithValue("@Estado", (int)turno.Estado);
                    cmd.Parameters.AddWithValue("@Observaciones", turno.Observaciones ?? "");

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
        public bool Update(clsTurnoBE turno)
        {
            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    string sql = @"UPDATE Turno 
                                   SET IdPaciente    = @IdPaciente,
                                       IdProfesional = @IdProfesional,
                                       FechaHora     = @FechaHora,
                                       Estado        = @Estado,
                                       Observaciones = @Observaciones
                                   WHERE IdTurno = @IdTurno";

                    SqlCommand cmd = new SqlCommand(sql, con, tran);
                    cmd.Parameters.AddWithValue("@IdTurno", turno.IDTurno);
                    cmd.Parameters.AddWithValue("@IdPaciente", turno.IDPaciente);
                    cmd.Parameters.AddWithValue("@IdProfesional", turno.IDProfesional);
                    cmd.Parameters.AddWithValue("@FechaHora", turno.FechaHora);
                    cmd.Parameters.AddWithValue("@Estado", (int)turno.Estado);
                    cmd.Parameters.AddWithValue("@Observaciones", turno.Observaciones ?? "");

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
        //se deja comentado porque nose utiliza borrado fisico solo logico con deshabilitacion en el sistema
        //public bool Delete(int idTurno)
        //{
        //    using (SqlConnection con = clsConexionDAL.GetConnection())
        //    {
        //        con.Open();
        //        SqlTransaction tran = con.BeginTransaction();
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(
        //                "DELETE FROM Turno WHERE IdTurno = @IdTurno", con, tran);
        //            cmd.Parameters.AddWithValue("@IdTurno", idTurno);

        //            cmd.ExecuteNonQuery();
        //            tran.Commit();
        //            return true;
        //        }
        //        catch
        //        {
        //            tran.Rollback();
        //            return false;
        //        }
        //    }
        //}
        #endregion
        #region metodos de lectura
        public clsTurnoBE GetById(int idTurno)
        {
            clsTurnoBE turno = null;

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Turno WHERE IdTurno = @IdTurno", con);
                cmd.Parameters.AddWithValue("@IdTurno", idTurno);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    turno = Mapear(dr);
                }
            }
            return turno;
        }
        public List<clsTurnoBE> GetAll()
        {
            List<clsTurnoBE> lista = new List<clsTurnoBE>();

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Turno", con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }      
        public List<clsTurnoBE> GetByPaciente(int idPaciente)
        {
            List<clsTurnoBE> lista = new List<clsTurnoBE>();

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Turno WHERE IdPaciente = @IdPaciente", con);
                cmd.Parameters.AddWithValue("@IdPaciente", idPaciente);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        public List<clsTurnoBE> GetByProfesional(int idProfesional)
        {
            List<clsTurnoBE> lista = new List<clsTurnoBE>();

            using (SqlConnection con = clsConexionDAL.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Turno WHERE IdProfesional = @IdProfesional", con);
                cmd.Parameters.AddWithValue("@idProfesional", idProfesional);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(Mapear(dr));
                }
            }
            return lista;
        }
        #endregion
        #region MAPPER
        private clsTurnoBE Mapear(SqlDataReader dr)
        {
            return new clsTurnoBE
            {
                IDTurno = (int)dr["IdTurno"],
                IDPaciente = (int)dr["IdPaciente"],
                IDProfesional = (int)dr["IdProfesional"],
                FechaHora = (DateTime)dr["FechaHora"],
                Estado = (EstadoTurnosBE)(int)dr["Estado"],
                Observaciones = dr["Observaciones"].ToString()
            };
        }
        #endregion
    }
}