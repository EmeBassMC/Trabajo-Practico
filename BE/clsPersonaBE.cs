using System;

namespace BE
{
    public abstract class clsPersonaBE
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Activo { get; set; }

        public clsPersonaBE()
        {
            Activo = true;
        }
    }
}