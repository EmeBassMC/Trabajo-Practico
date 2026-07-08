using BE;
using DAL;
using System.Diagnostics.CodeAnalysis;

namespace BLL
{
    public class clsPacienteBLL
    {
        #region metodos de escritura
        public bool Insert(clsPacienteBE paciente)
        {
            try
            {
                if (string.IsNullOrEmpty(paciente.Nombre)) return false;
                if (string.IsNullOrEmpty(paciente.Apellido)) return false;
                if (string.IsNullOrEmpty(paciente.DNI)) return false;
                if (paciente.DNI.Length != 8) return false;

                clsPacienteDAL dal = new clsPacienteDAL();
                paciente.DVH = clsDigitoVerificador.CalcularDVH(paciente);
                paciente.Email = clsEncriptacion.Encriptar(paciente.Email);
                bool resultado = dal.Insert(paciente);

                // Recalcular DVV — esto era lo que faltaba
                List<clsPacienteBE> todos = dal.GetAll();
                foreach (clsPacienteBE p in todos)
                    // por si CalcularDVV llegara a necesitarlo (no lo usa, pero mantiene consistencia)
                    p.Email = clsEncriptacion.Desencriptar(p.Email); 
                int dvv = clsDigitoVerificador.CalcularDVV(todos);
                new clsDigitoVerificadorDAL().GuardarDVV("Paciente", dvv);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Alta de Paciente";
                b.Informacion = resultado ? "OK - DNI: " + paciente.DNI : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return false;
            }
        }
        public bool Update(clsPacienteBE pacienteUpdate)
        {
            try
            {
                if (pacienteUpdate.IdPersona <= 0) return false;
                if (string.IsNullOrEmpty(pacienteUpdate.Nombre)) return false;
                if (string.IsNullOrEmpty(pacienteUpdate.Apellido)) return false;
                if (string.IsNullOrEmpty(pacienteUpdate.DNI) || (pacienteUpdate.DNI.Length != 8)) return false;

                clsPacienteDAL dal = new clsPacienteDAL();
                clsPacienteBE pacienteAnterior = dal.GetByID(pacienteUpdate.IdPersona);
                new clsControlCambiosBLL().RegistrarCambioPaciente(pacienteAnterior);   // guardamos el paciente anterior para un restore si hace falta
                pacienteUpdate.DVH = clsDigitoVerificador.CalcularDVH(pacienteUpdate);
                pacienteUpdate.Email = clsEncriptacion.Encriptar(pacienteUpdate.Email); // ← generamos el paciente nuevo

                bool resultado = dal.Update(pacienteUpdate);

                List<clsPacienteBE> todos = dal.GetAll();
                int dvv = clsDigitoVerificador.CalcularDVV(todos);
                new clsDigitoVerificadorDAL().GuardarDVV("Paciente", dvv);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Modificación de Paciente";
                b.Informacion = resultado ?
                    "ANTES: ID:" + pacienteAnterior.IdPersona + " " + pacienteAnterior.Nombre + " " + pacienteAnterior.Apellido + " DNI:" + pacienteAnterior.DNI +
                    " | DESPUÉS: ID:" + pacienteUpdate.IdPersona + " " + pacienteUpdate.Nombre + " " + pacienteUpdate.Apellido + " DNI:" + pacienteUpdate.DNI
                    : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsPacienteDAL dal = new clsPacienteDAL();
                bool resultado = dal.Delete(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Baja de Paciente";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return false;
            }
        }
        public bool Restaurar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsPacienteDAL dal = new clsPacienteDAL();
                bool resultado = dal.Restaurar(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Restauración de Paciente";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return false;
            }
        }

        public List<clsPacienteBE> GetEliminados()
        {
            clsPacienteDAL dal = new clsPacienteDAL();
            return dal.GetEliminados();
        }
        #endregion

        #region metodos de lectura
        public clsPacienteBE GetById(int id)
        {
            try
            {
                if (id <= 0) return null;
                clsPacienteDAL dal = new clsPacienteDAL();
                clsPacienteBE p = dal.GetByID(id);
                if (p != null) p.Email = clsEncriptacion.Desencriptar(p.Email);
                return p;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return null;
            }
        }
        public List<clsPacienteBE> GetAll()
        {
            try
            {
                clsPacienteDAL dal = new clsPacienteDAL();
                List<clsPacienteBE> lista = dal.GetAll();
                foreach (clsPacienteBE p in lista)
                    p.Email = clsEncriptacion.Desencriptar(p.Email);
                return lista;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                return null;
            }
        }
        public clsPacienteBE GetByDni(string dni)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(dni)) return null;
                        clsPacienteDAL dal = new clsPacienteDAL();
                        clsPacienteBE p = dal.GetByDNI(dni);
                        if (p != null) p.Email = clsEncriptacion.Desencriptar(p.Email);
                        return p;
                    }
                    catch (Exception ex)
                    {
                        clsBitacoraBLL.RegistrarError("clsPacienteBLL", ex);
                        return null;
                    }
                }

        #endregion
    }
}