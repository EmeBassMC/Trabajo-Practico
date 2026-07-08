using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class clsControlCambiosBLL
    {
        public void RegistrarCambioPaciente(clsPacienteBE anterior)
        {
            try
            {
                clsControlCambiosDAL dal = new clsControlCambiosDAL();
                clsControlCambioBE cambio = new clsControlCambioBE
                {
                    Tabla = "Paciente",
                    IdRegistro = anterior.IdPersona,
                    UsuarioId = clsSesionActual.GetInstancia().IdUsuario,
                    DatosAnteriores = clsSerializador.Serializar(anterior)
                };
                dal.Insert(cambio);
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsControlCambiosBLL", ex);
            }
        }

        public List<clsControlCambioBE> GetHistorialPaciente(int idPaciente)
        {
            clsControlCambiosDAL dal = new clsControlCambiosDAL();
            return dal.GetHistorial("Paciente", idPaciente);
        }

        public bool RestaurarPaciente(int idControlCambio)
        {
            try
            {
                clsControlCambiosDAL dal = new clsControlCambiosDAL();
                clsControlCambioBE cambio = dal.GetById(idControlCambio);
                if (cambio == null) return false;

                // El snapshot se tomó ANTES de que el BLL desencriptara/re-encriptara nada,
                // así que ya tiene el Email tal cual estaba en la base (encriptado) y el DVH correcto.
                clsPacienteBE pacienteAnterior = clsSerializador.Deserializar<clsPacienteBE>(cambio.DatosAnteriores);

                clsPacienteDAL pacienteDal = new clsPacienteDAL();
                bool resultado = pacienteDal.Update(pacienteAnterior);

                if (resultado)
                {
                    dal.MarcarRestaurado(idControlCambio);

                    List<clsPacienteBE> todos = pacienteDal.GetAll();
                    int dvv = clsDigitoVerificador.CalcularDVV(todos);
                    new clsDigitoVerificadorDAL().GuardarDVV("Paciente", dvv);

                    clsBitacoraBE b = new clsBitacoraBE();
                    b.UsuarioId = clsSesionActual.GetInstancia().IdUsuario;
                    b.Actividad = "Restauración de Cambio (Paciente)";
                    b.Informacion = "OK - Id Paciente: " + pacienteAnterior.IdPersona + " - ControlCambio: " + idControlCambio;
                    clsBitacoraBLL.Registrar(b);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                clsBitacoraBLL.RegistrarError("clsControlCambiosBLL", ex);
                return false;
            }
        }
        public List<clsCambioResumenBE> GetHistorialPacienteResumen(int idPaciente)
        {
            clsControlCambiosDAL dal = new clsControlCambiosDAL();
            List<clsControlCambioBE> historial = dal.GetHistorial("Paciente", idPaciente);

            clsPacienteDAL pacienteDal = new clsPacienteDAL();
            clsPacienteBE actual = pacienteDal.GetByID(idPaciente); // estado actual, en vivo, no guardado

            List<clsCambioResumenBE> resultado = new List<clsCambioResumenBE>();
            foreach (clsControlCambioBE c in historial)
            {
                clsPacienteBE antes = clsSerializador.Deserializar<clsPacienteBE>(c.DatosAnteriores);

                resultado.Add(new clsCambioResumenBE
                {
                    IdControlCambio = c.IdControlCambio,
                    FechaCambio = c.FechaCambio,
                    UsuarioId = c.UsuarioId,
                    Resumen = ArmarResumenCambios(antes, actual),
                    Restaurado = c.Restaurado
                });
            }
            return resultado;
        }
        private string ArmarResumenCambios(clsPacienteBE antes, clsPacienteBE actual)
        {
            List<string> cambios = new List<string>();
            if (antes.Nombre != actual.Nombre) cambios.Add($"Nombre: \"{antes.Nombre}\" → \"{actual.Nombre}\"");
            if (antes.Apellido != actual.Apellido) cambios.Add($"Apellido: \"{antes.Apellido}\" → \"{actual.Apellido}\"");
            if (antes.DNI != actual.DNI) cambios.Add($"DNI: \"{antes.DNI}\" → \"{actual.DNI}\"");
            if (antes.Telefono != actual.Telefono) cambios.Add($"Teléfono: \"{antes.Telefono}\" → \"{actual.Telefono}\"");
            if (antes.ObraSocial != actual.ObraSocial) cambios.Add($"Obra Social: \"{antes.ObraSocial}\" → \"{actual.ObraSocial}\"");
            if (antes.FechaNacimiento != actual.FechaNacimiento)
                cambios.Add($"Fecha Nac.: {antes.FechaNacimiento:dd/MM/yyyy} → {actual.FechaNacimiento:dd/MM/yyyy}");
            // Email queda afuera: está encriptado, comparar el texto cifrado no dice nada útil

            return cambios.Count > 0 ? string.Join("  |  ", cambios) : "(sin cambios en los campos visibles)";
        }

    }
}