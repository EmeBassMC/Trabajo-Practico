using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BLL
{
    public abstract class clsComponenteRol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public abstract bool EsGrupo { get; }
        public abstract List<string> ObtenerPermisos();

        public virtual void Agregar(clsComponenteRol componente)
        {
            throw new NotSupportedException($"{GetType().Name} no admite hijos.");
        }

        public virtual void Quitar(clsComponenteRol componente)
        {
            throw new NotSupportedException($"{GetType().Name} no admite hijos.");
        }
    }
}