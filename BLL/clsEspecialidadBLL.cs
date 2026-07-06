using BE;
using DAL;
using System.Net;

namespace BLL
{
    public class clsEspecialidadBLL
    {

        public bool Insert(clsEspecialidadBE especialidad)
        {
            try
            {
                if (string.IsNullOrEmpty(especialidad.Nombre)) return false;
                clsEspecialidadDAL dal = new clsEspecialidadDAL();

                bool resultado = dal.Insert(especialidad);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Alta de Especialidad";
                b.Informacion = resultado ? "OK - Id: " + especialidad.IdEspecialidad : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }
        public bool Update(clsEspecialidadBE especialidad)
        {
            try
            {
                if ((especialidad.IdEspecialidad <= 0)) return false;
                if (string.IsNullOrEmpty(especialidad.Nombre)) return false;

                clsEspecialidadDAL dal = new clsEspecialidadDAL();
                clsEspecialidadBE anterior = dal.GetByID(especialidad.IdEspecialidad);
                bool resultado = dal.Update(especialidad);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Modificación de Especialidad";

                b.Informacion = resultado ?
                    "ANTES - ID: " + anterior.IdEspecialidad + " Nombre: " + anterior.Nombre +
                    " | DESPUÉS - ID: " + especialidad.IdEspecialidad + " Nombre: " + especialidad.Nombre
                    : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                if ((id <= 0)) return false;

                clsEspecialidadDAL dal = new clsEspecialidadDAL();

                bool resultado = dal.Delete(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Baja de Especialidad";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }
        public bool Restaurar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsEspecialidadDAL dal = new clsEspecialidadDAL();
                bool resultado = dal.Restaurar(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Restauración de Especialidad";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return false;
            }
        }

        public List<clsEspecialidadBE> GetEliminadas()
        {
            clsEspecialidadDAL dal = new clsEspecialidadDAL();
            return dal.GetEliminadas();
        }
        public List<clsEspecialidadBE> GetAll()
        {
            try
            {
                clsEspecialidadDAL dal = new clsEspecialidadDAL();
                return dal.GetAll();
            }
            catch (Exception ex)
            {
                string v = ex.ToString();
                return null;
            }
        }
        public clsEspecialidadBE GetById(int id)
        {
            try
            {
                if (id <= 0) return null;
                clsEspecialidadDAL dal = new clsEspecialidadDAL();
                return dal.GetByID(id);
            }
            catch (Exception ex)
            {
                string v = ex.ToString();
                return null;
            }
        }

    }
}