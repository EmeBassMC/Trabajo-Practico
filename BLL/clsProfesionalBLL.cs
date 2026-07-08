using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class clsProfesionalBLL
    {
        #region metodos de escritura
        public bool Insert(clsProfesionalBE profesional)
        {
            try
            {
                if (string.IsNullOrEmpty(profesional.Matricula)) return false;
                if (profesional.IdEspecialidad <= 0) return false;
                if (string.IsNullOrEmpty(profesional.Nombre)) return false;
                if (string.IsNullOrEmpty(profesional.Apellido)) return false;
                if (string.IsNullOrEmpty(profesional.DNI)) return false;
                if (profesional.DNI.Length != 8) return false;

                clsProfesionalDAL dal = new clsProfesionalDAL();
                profesional.Email = clsEncriptacion.Encriptar(profesional.Email); // ← nuevo
                bool resultado = dal.Insert(profesional);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Alta de Profesional";
                b.Informacion = resultado ? "OK - Id: " + profesional.IdPersona : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return false;
            }
        }

        public bool Update(clsProfesionalBE profesional)
        {
            try
            {
                if (profesional.IdPersona <= 0) return false;
                if (string.IsNullOrEmpty(profesional.Matricula)) return false;
                if (profesional.IdEspecialidad <= 0) return false;
                if (string.IsNullOrEmpty(profesional.Nombre)) return false;
                if (string.IsNullOrEmpty(profesional.Apellido)) return false;
                if (string.IsNullOrEmpty(profesional.DNI)) return false;
                if (profesional.DNI.Length != 8) return false;

                clsProfesionalDAL dal = new clsProfesionalDAL();
                clsProfesionalBE anterior = dal.GetByID(profesional.IdPersona);
                profesional.Email = clsEncriptacion.Encriptar(profesional.Email); // ← nuevo

                bool resultado = dal.Update(profesional);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Modificación de Profesional";
                b.Informacion = resultado ?
           "OK - ANTES: ID:" + anterior.IdPersona + " " + anterior.Nombre + " " + anterior.Apellido + " DNI:" + anterior.DNI + " Mat:" + anterior.Matricula +
           " | DESPUÉS: ID:" + profesional.IdPersona + " " + profesional.Nombre + " " + profesional.Apellido + " DNI:" + profesional.DNI + " Mat:" + profesional.Matricula
           : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {

                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsProfesionalDAL dal = new clsProfesionalDAL();
                bool resultado = dal.Delete(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Baja de Profesional";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return false;
            }
        }
        public bool Restaurar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsProfesionalDAL dal = new clsProfesionalDAL();
                bool resultado = dal.Restaurar(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Restauración de Profesional";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return false;
            }
        }

        public List<clsProfesionalBE> GetEliminados()
        {
            clsProfesionalDAL dal = new clsProfesionalDAL();
            return dal.GetEliminados();
        }
        #endregion
        #region METODOS DE LECTURA
        public clsProfesionalBE GetById(int id)
        {
            try
            {
                if (id <= 0) return null;
                clsProfesionalDAL dal = new clsProfesionalDAL();
                clsProfesionalBE p = dal.GetByID(id);
                if (p != null) p.Email = clsEncriptacion.Desencriptar(p.Email);
                return p;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return null;
            }
        }
        public List<clsProfesionalBE> GetAll()
        {
            try
            {
                clsProfesionalDAL dal = new clsProfesionalDAL();
                List<clsProfesionalBE> lista = dal.GetAll();
                foreach (clsProfesionalBE p in lista)
                    p.Email = clsEncriptacion.Desencriptar(p.Email);
                return lista;
            }
            catch (Exception ex)
            {

                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return null;
            }
        }

        public clsProfesionalBE GetByDni(string dni)
        {
            try
            {
                if (string.IsNullOrEmpty(dni)) return null;
                clsProfesionalDAL dal = new clsProfesionalDAL();
                clsProfesionalBE p = dal.GetByDNI(dni);
                if (p != null) p.Email = clsEncriptacion.Desencriptar(p.Email);
                return p;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsProfesionalBLL", ex);
                return null;
            }
        }
        #endregion
    }
}