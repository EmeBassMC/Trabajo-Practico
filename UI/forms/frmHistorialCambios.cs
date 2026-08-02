using BE;
using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;
using UI.Utilidades;
namespace UI.forms
{
    public partial class frmHistorialCambios : Form, IObservadorIdioma
    {
        private int idPaciente;
        private clsControlCambiosBLL bll = new clsControlCambiosBLL();
        public frmHistorialCambios(int idPaciente)
        {
            InitializeComponent();
            this.idPaciente = idPaciente;
            this.FormClosed += frmHistorialCambios_FormClosed;
        }
        private void frmHistorialCambios_Load(object sender, EventArgs e)
        {
            dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorial.MultiSelect = false;
            dgvHistorial.ReadOnly = true;
            CargarHistorial();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            clsEstiloUI.PersonalizarForm(this);
            clsEstiloUI.EstilizarGrilla(dgvHistorial);

        }
        private void frmHistorialCambios_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();
            this.Text = "TurnoSync | " + g.Traducir("titleHistorial") + " |";
            btnRestaurar.Text = g.Traducir("btnRestaurar");
            btnCerrar.Text = g.Traducir("btnCerrar");

            if (dgvHistorial.Columns.Count > 0)
            {
                dgvHistorial.Columns["IdControlCambio"].HeaderText = g.Traducir("colIdControlCambio");
                dgvHistorial.Columns["FechaCambio"].HeaderText = g.Traducir("colFechaCambio");
                dgvHistorial.Columns["UsuarioId"].HeaderText = g.Traducir("colUsuarioId");
                dgvHistorial.Columns["Resumen"].HeaderText = g.Traducir("colResumen");
            }
        }
        private void CargarHistorial()
        {
            dgvHistorial.DataSource = null;
            dgvHistorial.DataSource = bll.GetHistorialPacienteResumen(idPaciente);
            if (dgvHistorial.Columns["Resumen"] != null)
                dgvHistorial.Columns["Resumen"].Width = 500;
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
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
        private void EstilizarGrilla(DataGridView grilla)
        {
            grilla.EnableHeadersVisualStyles = false;
            grilla.BorderStyle = BorderStyle.None;
            grilla.BackgroundColor = Color.White;
            grilla.GridColor = Color.FromArgb(225, 225, 225);
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 62, 80);
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grilla.ColumnHeadersHeight = 32;

            grilla.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grilla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(93, 135, 173);
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
            grilla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 248);

            grilla.RowTemplate.Height = 26;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}