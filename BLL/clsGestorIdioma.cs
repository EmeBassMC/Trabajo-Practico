using BE;
using DAL;
using System.Collections.Generic;
namespace BLL
{
    public class clsGestorIdioma
    {
        private static clsGestorIdioma _instancia;
        private List<IObservadorIdioma> _suscriptores;
        private Dictionary<string, string> _traducciones;
        private Dictionary<string, string> _traduccionesFallback; // inglés, para cuando falta la del idioma actual
        private clsTraduccionDAL traduccionDAL;
        public string IdiomaActual { get; private set; }
        private clsGestorIdioma()
        {
            _suscriptores = new List<IObservadorIdioma>();
            _traducciones = new Dictionary<string, string>();
            traduccionDAL = new clsTraduccionDAL();
            IdiomaActual = "es";
            CargarTraducciones("es");
            _traduccionesFallback = traduccionDAL.GetDiccionarioPorCodigo("en");
        }
        public int GetTotalClaves()
        {
            return _traducciones.Count;
        }
        public static clsGestorIdioma GetInstancia()
        {
            if (_instancia == null)
                _instancia = new clsGestorIdioma();
            return _instancia;
        }
        public void Suscribir(IObservadorIdioma observador)
        {
            if (!_suscriptores.Contains(observador))
                _suscriptores.Add(observador);
        }
        public void Desuscribir(IObservadorIdioma observador)
        {
            if (_suscriptores.Contains(observador))
                _suscriptores.Remove(observador);
        }
        public void CambiarIdioma(string codigo)
        {
            IdiomaActual = codigo;
            CargarTraducciones(codigo);
            _traduccionesFallback = traduccionDAL.GetDiccionarioPorCodigo("en"); // refresca por si editaste inglés recién
            foreach (IObservadorIdioma obs in _suscriptores)
                obs.ActualizarIdioma(codigo);
        }
        // Carga todas las traducciones del idioma en el diccionario
        private void CargarTraducciones(string codigo)
        {
            _traducciones = traduccionDAL.GetDiccionarioPorCodigo(codigo);
        }
        // Devuelve el texto de una clave: 1) el del idioma actual si está completo,
        // 2) si no, el de inglés como respaldo, 3) si tampoco, la clave cruda.
        public string Traducir(string clave)
        {
            if (_traducciones.ContainsKey(clave) && !string.IsNullOrWhiteSpace(_traducciones[clave]))
                return _traducciones[clave];

            if (_traduccionesFallback != null && _traduccionesFallback.ContainsKey(clave) && !string.IsNullOrWhiteSpace(_traduccionesFallback[clave]))
                return _traduccionesFallback[clave];

            return clave;
        }
        public bool TieneClavesSinTraducir()
        {
            foreach (var kvp in _traducciones)
            {
                if (string.IsNullOrWhiteSpace(kvp.Value))
                    return true;
            }
            return false;
        }
    }
}