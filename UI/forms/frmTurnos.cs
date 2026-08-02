using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Utilidades;

namespace UI
{
    public partial class frmTurnos : Form, IObservadorIdioma
    {

        clsTurnosBLL bllTurnos = new clsTurnosBLL();
        clsPacienteBLL bllPaciente = new clsPacienteBLL();
        clsProfesionalBLL bllProfesional = new clsProfesionalBLL();
        clsRolBLL bllRol = new clsRolBLL();
        bool puedeAgregar, puedeConfirmar, puedeCancelar;
        bool modoEdicion = false;
        int idSeleccionado = 0;
        public frmTurnos()
        {
            InitializeComponent();
        }

        private void frmTurnos_Load(object sender, EventArgs e)
        {
            int idUsuario = clsSesionActual.GetInstancia().IdUsuario;
            puedeAgregar = bllRol.TienePermiso(idUsuario, "Turnos.Agregar");
            puedeConfirmar = bllRol.TienePermiso(idUsuario, "Turnos.Confirmar");
            puedeCancelar = bllRol.TienePermiso(idUsuario, "Turnos.Cancelar");

            cargarComboEstado();
            cargarComboPacientes();
            cargarComboProfesionales();
            cargarGrilla();
            bloquearCampos();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
            clsEstiloUI.PersonalizarForm(this);
            clsEstiloUI.EstilizarGrilla(dgvTurnos);
        }



        private void cmbPaciente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbmProfesional_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpFechaTurno_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtObservaciones_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            limpiarCampos();
            habilitarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var g = clsGestorIdioma.GetInstancia();

            if (modoEdicion == false)
            {
                // Sin esto, si no hay pacientes o profesionales cargados todavía,
                // SelectedValue es null y el (int) de abajo tira NullReferenceException.
                if (cmbPaciente.SelectedValue == null)
                {
                    MessageBox.Show("Tenés que cargar al menos un Paciente antes de crear un Turno.");
                    return;
                }
                if (cbmProfesional.SelectedValue == null)
                {
                    MessageBox.Show("Tenés que cargar al menos un Profesional antes de crear un Turno.");
                    return;
                }
                if (dtpFechaTurno.Value <= DateTime.Now)
                {
                    MessageBox.Show("La fecha y hora del turno tiene que ser futura.");
                    return;
                }

                clsTurnoBE turno = new clsTurnoBE();
                turno.IDPaciente = (int)cmbPaciente.SelectedValue;
                turno.IDProfesional = (int)cbmProfesional.SelectedValue;
                turno.FechaHora = dtpFechaTurno.Value;
                turno.Estado = EstadoTurnosBE.Pendiente;
                turno.Observaciones = txtObservaciones.Text;
                bool resultado = bllTurnos.Insert(turno);
                if (resultado == true)
                {
                    MessageBox.Show(g.Traducir("msgGuardadoExito"));
                }
                else
                {
                    MessageBox.Show("No se pudo guardar. Puede que el profesional ya tenga un turno en ese horario.");
                }
            }
            cargarGrilla();
            bloquearCampos();
            limpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0) return;

            var g = clsGestorIdioma.GetInstancia();
            DialogResult confirm = MessageBox.Show(
                g.Traducir("msgConfirmarEliminar"),
                g.Traducir("msgConfirmar"),
                MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                bllTurnos.Cancelar(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            var g = clsGestorIdioma.GetInstancia();

            if (idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un turno primero");
                return;
            }
            bool resultado = bllTurnos.Confirmar(idSeleccionado);
            if (resultado)
                MessageBox.Show(g.Traducir("msgTurnoConfirmado"));
            else
                MessageBox.Show(g.Traducir("msgErrorConfirmar"));

            cargarGrilla();
            bloquearCampos();
            limpiarCampos();
        }

        private void btnCancelarForm_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            bloquearCampos();
            modoEdicion = false;
        }

        private void dgvTurnos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvTurnos.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(fila.Cells["IdTurno"].Value);
                cmbPaciente.SelectedValue = Convert.ToInt32(fila.Cells["IdPaciente"].Value);
                cbmProfesional.SelectedValue = Convert.ToInt32(fila.Cells["IdProfesional"].Value);
                dtpFechaTurno.Value = Convert.ToDateTime(fila.Cells["FechaHora"].Value);
                txtObservaciones.Text = fila.Cells["Observaciones"].Value.ToString();
                modoEdicion = true;
                habilitarCampos();
                btnConfirmar.Enabled = puedeConfirmar;
                btnCancelar.Enabled = puedeCancelar;
            }
        }

        //metodos para el formulario, carga de grilla, combos,etc.
        public void cargarComboEstado()
        {
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Confirmado");
            cmbEstado.Items.Add("Cancelado");
            cmbEstado.SelectedIndex = 0;
        }
        public void cargarComboPacientes()
        {
            cmbPaciente.DataSource = bllPaciente.GetAll();
            cmbPaciente.DisplayMember = "Nombre";
            cmbPaciente.ValueMember = "IdPersona";
        }
        public void cargarComboProfesionales()
        {
            cbmProfesional.DataSource = bllProfesional.GetAll();
            cbmProfesional.DisplayMember = "Nombre";
            cbmProfesional.ValueMember = "IdPersona";

        }
        public void cargarGrilla()
        {
            dgvTurnos.DataSource = bllTurnos.GetAll();
        }
        public void bloquearCampos()
        {
            cmbPaciente.Enabled = false;
            cbmProfesional.Enabled = false;
            cmbEstado.Enabled = false;
            dtpFechaTurno.Enabled = false;
            txtObservaciones.Enabled = false;
            btnNuevo.Enabled = puedeAgregar;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnConfirmar.Enabled = false;
        }
        public void habilitarCampos()
        {
            cmbPaciente.Enabled = true;
            cbmProfesional.Enabled = true;
            cmbEstado.Enabled = true;
            dtpFechaTurno.Enabled = true;
            txtObservaciones.Enabled = true;
            btnNuevo.Enabled = false;
            btnGuardar.Enabled = puedeAgregar;     
            btnCancelar.Enabled = puedeCancelar;   
            btnConfirmar.Enabled = puedeConfirmar; 
        }
        public void limpiarCampos()
        {
            txtObservaciones.Text = "";
            cargarComboPacientes();    
            cargarComboProfesionales();
            cmbEstado.SelectedIndex = 0;
            dtpFechaTurno.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        private void Turnos_Enter(object sender, EventArgs e)
        {

        }

        private void frmTurnos_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            Turnos.Text = g.Traducir("grpTurnos");
            label1.Text = g.Traducir("lblPaciente");
            lblNombreProfesional.Text = g.Traducir("lblProfesional");
            label7.Text = g.Traducir("lblFechaTurno");
            label2.Text = g.Traducir("lblObservaciones");
            btnNuevo.Text = g.Traducir("btnNuevo");
            btnGuardar.Text = g.Traducir("btnGuardar");
            btnConfirmar.Text = g.Traducir("btnConfirmar");
            btnCancelar.Text = g.Traducir("btnCancelar");
            btnCancelarForm.Text = g.Traducir("btnCancelarForm");
            this.Text = g.Traducir("titleTurnos");

            if (dgvTurnos.Columns.Count > 0)
            {
                dgvTurnos.Columns["IDTurno"].HeaderText = g.Traducir("colID");
                dgvTurnos.Columns["IDPaciente"].HeaderText = g.Traducir("colPaciente");
                dgvTurnos.Columns["IDProfesional"].HeaderText = g.Traducir("colProfesional");
                dgvTurnos.Columns["FechaHora"].HeaderText = g.Traducir("colFechaHora");
                dgvTurnos.Columns["Estado"].HeaderText = g.Traducir("colEstado");
                dgvTurnos.Columns["Observaciones"].HeaderText = g.Traducir("colObservaciones");
            }
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

