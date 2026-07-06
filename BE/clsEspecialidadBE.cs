namespace BE
{
    public class clsEspecialidadBE
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public clsEspecialidadBE()
        {
            Activo = true;
        }
    }
}