using BE;
using BLL;
using System;
using System.Windows.Forms;
namespace UI.forms
{
    public partial class frmHistorialCambios : Form
    {
        private int idPaciente;
        private clsControlCambiosBLL bll = new clsControlCambiosBLL();
        public frmHistorialCambios(int idPaciente)
        {
            InitializeComponent();
            this.idPaciente = idPaciente;
        }
        private void frmHistorialCambios_Load(object sender, EventArgs e)
        {
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.ReadOnly = true;
            CargarHistorial();
        }
        private void CargarHistorial()
        {
            dgvHistorial.DataSource = null;
            dgvHistorial.DataSource = bll.GetHistorialPacienteResumen(idPaciente);
            if (dgvHistorial.Columns["Resumen"] != null)
                dgvHistorial.Columns["Resumen"].Width = 500;
        }
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (dgvHistorial.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un cambio de la lista primero.");
                return;
            }
            int idControlCambio = Convert.ToInt32(dgvHistorial.SelectedRows[0].Cells["IdControlCambio"].Value);
            DialogResult confirm = MessageBox.Show(
                "Esto va a revertir el paciente al estado que tenía en ese momento.\n¿Confirmás?",
                "Confirmar restauración",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;
            bool resultado = bll.RestaurarPaciente(idControlCambio);
            if (resultado)
            {
                MessageBox.Show("Restaurado correctamente.");
                CargarHistorial();
            }
            else
            {
                MessageBox.Show("No se pudo restaurar. Revisá la Bitácora para más detalle.");
            }
        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dgvHistorial_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}