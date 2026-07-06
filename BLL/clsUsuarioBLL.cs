using BE;
using DAL;

namespace BLL
{
    public class clsUsuarioBLL
    {
        clsUsuarioDAL dal = new clsUsuarioDAL();
        clsSeguridadBLL seg = new clsSeguridadBLL();

        public bool Login(string nombreUsuario, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreUsuario)) return false;
                if (string.IsNullOrEmpty(password)) return false;

                clsUsuarioBE usuario = dal.GetByUsername(nombreUsuario);

                if (usuario == null) return false;
                bool resultado = seg.VerificarPassword(password, usuario.PasswordHash);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = usuario.IdUsuario;
                b.Actividad = "Inicio de Sesión";
                b.Informacion = resultado ? "OK - " + nombreUsuario : "ERROR - " + nombreUsuario;
                clsBitacoraBLL.Registrar(b);


                return resultado;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }

        public bool Registrar(clsUsuarioBE usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.NombreUsuario)) return false;
                if (string.IsNullOrEmpty(usuario.PasswordHash)) return false;

                usuario.PasswordHash = seg.HashPassword(usuario.PasswordHash);
                bool resultado = dal.Insert(usuario);
                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Alta de Usuario";
                b.Informacion = resultado ? "OK - Id: " + usuario.IdUsuario : "ERROR";
                clsBitacoraBLL.Registrar(b);
                return resultado;
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                return false;
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsUsuarioDAL dal = new clsUsuarioDAL();
                bool resultado = dal.Delete(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Baja de Usuario";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                string v = ex.ToString();
                return false;
            }
        }

        public bool Restaurar(int id)
        {
            try
            {
                if (id <= 0) return false;
                clsUsuarioDAL dal = new clsUsuarioDAL();
                bool resultado = dal.Restaurar(id);

                clsBitacoraBE b = new clsBitacoraBE();
                b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                b.Actividad = "Restauración de Usuario";
                b.Informacion = resultado ? "OK - Id: " + id : "ERROR";
                clsBitacoraBLL.Registrar(b);

                return resultado;
            }
            catch (Exception ex)
            {
                string v = ex.ToString();
                return false;
            }
        }

        #region metodos lectura
        public clsUsuarioBE GetByUsername(string username)
        {
            try
            {
                clsUsuarioBE u = dal.GetByUsername(username);
                if (u != null)
                {
                    clsRolBLL rolBll = new clsRolBLL();
                    u.Roles = rolBll.GetRolesUsuario(u.IdUsuario);
                }
                return u;
            }
            catch
            {
                return null;
            }
        }


        public List<clsUsuarioBE> GetAll()
        {
            clsUsuarioDAL dal = new clsUsuarioDAL();
            return dal.GetAll();
        }

        public List<clsUsuarioBE> GetEliminados()
        {
            clsUsuarioDAL dal = new clsUsuarioDAL();
            return dal.GetEliminados();
        }
        #endregion
    }
}