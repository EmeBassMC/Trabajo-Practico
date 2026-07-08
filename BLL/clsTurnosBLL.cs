using BE;
using DAL;

namespace BLL
{
    public class clsTurnosBLL
    {
        #region metodos de escritura
        public bool Insert(clsTurnoBE turno)
        {
            try
            {
                if (turno.IDPaciente <= 0) return false;
                if (turno.IDProfesional <= 0) return false;
                if (turno.FechaHora <= DateTime.Now) return false;
                if (EstaDisponbile(turno.IDProfesional, turno.FechaHora) == false) return false;

                clsTurnosDAL dal = new clsTurnosDAL();
                bool resultado = dal.Insert(turno);
                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Alta de Turno";
                b.Informacion = resultado ? "OK - Id: " + turno.IDTurno : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return false;
            }
        }
        public bool Cancelar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsTurnosDAL dal = new clsTurnosDAL();
                clsTurnoBE turno = dal.GetById(id);
                if (turno == null) return false;
                if (turno.Estado == EstadoTurnosBE.Cancelado) return false;
                turno.Estado = EstadoTurnosBE.Cancelado;
                bool resultado = dal.Update(turno);
                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Cancelación de Turno";
                b.Informacion = resultado ? "OK - Id: " + turno.IDTurno : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return false;
            }
        }
        public bool Confirmar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsTurnosDAL dal = new clsTurnosDAL();
                clsTurnoBE turno = dal.GetById(id);
                if (turno == null) return false;
                if (turno.Estado == EstadoTurnosBE.Confirmado) return false;
                turno.Estado = EstadoTurnosBE.Confirmado;
                bool resultado = dal.Update(turno);
                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Confirmación de Turno";
                b.Informacion = resultado ? "OK - Id: " + turno.IDTurno : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return false;
            }
        }
        public bool Update(clsTurnoBE turno)
        {
            try
            {
                if (turno.IDTurno <= 0) return false;
                if (turno.IDPaciente <= 0) return false;
                if (turno.IDProfesional <= 0) return false;

                clsTurnosDAL dal = new clsTurnosDAL();
                clsTurnoBE anterior = dal.GetById(turno.IDTurno);

                bool resultado = dal.Update(turno);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Modificación de Turno";
                b.Informacion = resultado ?
                    "ANTES - ID:" + anterior.IDTurno + " Pac:" + anterior.IDPaciente + " Prof:" + anterior.IDProfesional + " Fecha:" + anterior.FechaHora + " Estado:" + anterior.Estado +
                    " | DESPUÉS - ID:" + turno.IDTurno + " Pac:" + turno.IDPaciente + " Prof:" + turno.IDProfesional + " Fecha:" + turno.FechaHora + " Estado:" + turno.Estado
                    : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return false;
            }
        }
        #endregion

        #region metodos lectura
        public clsTurnoBE GetByID(int id)
        {
            try
            {
                if (id <= 0) return null;
                clsTurnosDAL dal = new clsTurnosDAL();
                return dal.GetById(id);
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return null;
            }
        }

        public List<clsTurnoBE> GetAll()
        {
            try
            {
                clsTurnosDAL dal = new clsTurnosDAL();
                return dal.GetAll();
            }
            catch (Exception ex)
            {

                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return null;
            }
        }
        public List<clsTurnoBE> GetByPaciente(int idPaciente)
        {
            try
            {
                if (idPaciente <= 0) return null;
                clsTurnosDAL dal = new clsTurnosDAL();
                return dal.GetByPaciente(idPaciente);
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsTurnosBLL", ex);
                return null;
            }
        }


        private bool EstaDisponbile(int IdProfesional, DateTime fechaHora)
        {
            clsTurnosDAL dal = new clsTurnosDAL();
            List<clsTurnoBE> listaTurnos = dal.GetByProfesional(IdProfesional);
            foreach (clsTurnoBE x in listaTurnos)
            {
                if (x.FechaHora == fechaHora) return false;
            }
            return true;
        }
        #endregion
    }
}