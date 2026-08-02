
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
using UI.forms;
using UI.Utilidades;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI
{
    public partial class frmPacientes : Form, IObservadorIdioma
    {

        clsPacienteBLL bllPaciente = new clsPacienteBLL();
        clsRolBLL bllRol = new clsRolBLL();
        bool puedeAgregar, puedeModificar, puedeEliminar;
        bool modoEdicion = false;
        int idSeleccionado = 0;

        public frmPacientes()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            bloquearCampos();
            modoEdicion = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0) return;
            var g = clsGestorIdioma.GetInstancia();
            DialogResult confirm = MessageBox.Show(
                g.Traducir("msgConfirmarEliminar"),
                g.Traducir("msgConfirmar"),
                MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bllPaciente.Delete(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }
        //metodos del formulario
        public void cargarGrilla()
        {
            var lista = chkVerEliminados.Checked ? bllPaciente.GetEliminados() : bllPaciente.GetAll();
            dgvPacientes.DataSource = lista;
            if (dgvPacientes.Columns["DVH"] != null)
                dgvPacientes.Columns["DVH"].Visible = false;
            if (dgvPacientes.Columns["Activo"] != null)
                dgvPacientes.Columns["Activo"].Visible = false;
        }
        public void bloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDNI.Enabled = false;
            txtTelefono.Enabled = false;
            txtMail.Enabled = false;
            txtObraSocial.Enabled = false;
            dtpFechaNacimiento.Enabled = false;
            btnNuevo.Enabled = puedeAgregar;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
        }
        public void habilitarCampos()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtDNI.Enabled = true;
            txtTelefono.Enabled = true;
            txtMail.Enabled = true;
            txtObraSocial.Enabled = true;
            dtpFechaNacimiento.Enabled = true;
            btnNuevo.Enabled = false;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
        }
        public void limpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDNI.Text = "";
            txtTelefono.Text = "";
            txtMail.Text = "";
            txtObraSocial.Text = "";
            dtpFechaNacimiento.Value = DateTime.Now;
            idSeleccionado = 0;

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var g = clsGestorIdioma.GetInstancia();

            if (!txtDNI.Text.All(char.IsDigit))
            {
                MessageBox.Show("El DNI debe contener solo números.");
                return;
            }
            if (!txtTelefono.Text.All(char.IsDigit))
            {
                MessageBox.Show("El telefono debe contener solo números.");
                return;
            }
            if (modoEdicion == false)
            {
                clsPacienteBE paciente = new clsPacienteBE();
                paciente.Nombre = txtNombre.Text;
                paciente.Apellido = txtApellido.Text;
                paciente.DNI = txtDNI.Text;
                paciente.Telefono = txtTelefono.Text;
                paciente.Email = txtMail.Text;
                paciente.ObraSocial = txtObraSocial.Text;
                paciente.FechaNacimiento = dtpFechaNacimiento.Value;
                bool resultado = bllPaciente.Insert(paciente);
                if (resultado == true)
                {
                    MessageBox.Show(g.Traducir("msgGuardadoExito"));
                }
                else
                {
                    MessageBox.Show(g.Traducir("msgErrorGuardar"));
                }
            }
            else if (modoEdicion == true)
            {
                clsPacienteBE paciente = new clsPacienteBE();
                paciente.Nombre = txtNombre.Text;
                paciente.Apellido = txtApellido.Text;
                paciente.DNI = txtDNI.Text;
                paciente.Telefono = txtTelefono.Text;
                paciente.Email = txtMail.Text;
                paciente.ObraSocial = txtObraSocial.Text;
                paciente.FechaNacimiento = dtpFechaNacimiento.Value;
                paciente.IdPersona = idSeleccionado;
                bool resultado = bllPaciente.Update(paciente);
                if (resultado == true)
                {
                    MessageBox.Show(g.Traducir("msgActualizadoExito"));
                }
                else
                {
                    MessageBox.Show(g.Traducir("msgErrorActualizar"));
                }
            }
            cargarGrilla();
            bloquearCampos();
            limpiarCampos();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            limpiarCampos();
            habilitarCampos();
        }
        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvPacientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvPacientes.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(fila.Cells["IdPersona"].Value);
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                txtDNI.Text = fila.Cells["DNI"].Value.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value.ToString();
                txtMail.Text = fila.Cells["Email"].Value.ToString();
                txtObraSocial.Text = fila.Cells["ObraSocial"].Value.ToString();
                dtpFechaNacimiento.Value = Convert.ToDateTime(fila.Cells["FechaNacimiento"].Value);
                modoEdicion = true;
                habilitarCampos();
                btnGuardar.Enabled = puedeModificar;
                btnEliminar.Enabled = puedeEliminar;
                btnRestaurar.Enabled = chkVerEliminados.Checked && puedeEliminar;

            }
        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // bloquea el caracter
            }
        }

        private void frmPacientes_Load(object sender, EventArgs e)
        {
            int idUsuario = clsSesionActual.GetInstancia().IdUsuario;
            puedeAgregar = bllRol.TienePermiso(idUsuario, "Pacientes.Agregar");
            puedeModificar = bllRol.TienePermiso(idUsuario, "Pacientes.Modificar");
            puedeEliminar = bllRol.TienePermiso(idUsuario, "Pacientes.Eliminar");

            cargarGrilla();
            bloquearCampos();
            clsEstiloUI.PersonalizarForm(this);
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
            clsEstiloUI.EstilizarGrilla(dgvPacientes);
        }

        private void frmPacientes_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            groupBox1.Text = g.Traducir("grpPaciente");
            lblNombre.Text = g.Traducir("lblNombre");
            lblApellido.Text = g.Traducir("lblApellido");
            lblDNI.Text = g.Traducir("lblDNI");
            lblTelefono.Text = g.Traducir("lblTelefono");
            lblEmail.Text = g.Traducir("lblEmail");
            lblObraSocial.Text = g.Traducir("lblObraSocial");
            label7.Text = g.Traducir("lblFechaNac");
            btnGuardar.Text = g.Traducir("btnGuardar");
            btnNuevo.Text = g.Traducir("btnNuevo");
            btnEliminar.Text = g.Traducir("btnEliminar");
            btnCancelar.Text = g.Traducir("btnCancelar");
            btnHistorial.Text = g.Traducir("btnHistorial");
            btnRestaurar.Text = g.Traducir("btnRestaurar");
            lblPacientesRegistrados.Text = g.Traducir("lblPacientesRegistrados");
            this.Text = g.Traducir("titlePacientes");

            if (dgvPacientes.Columns.Count > 0)
            {
                dgvPacientes.Columns["IdPersona"].HeaderText = g.Traducir("colID");
                dgvPacientes.Columns["Nombre"].HeaderText = g.Traducir("colNombre");
                dgvPacientes.Columns["Apellido"].HeaderText = g.Traducir("colApellido");
                dgvPacientes.Columns["DNI"].HeaderText = g.Traducir("colDNI");
                dgvPacientes.Columns["Telefono"].HeaderText = g.Traducir("colTelefono");
                dgvPacientes.Columns["Email"].HeaderText = g.Traducir("colEmail");
                dgvPacientes.Columns["FechaNacimiento"].HeaderText = g.Traducir("colFechaNac");
                dgvPacientes.Columns["ObraSocial"].HeaderText = g.Traducir("colObraSocial");
            }
        }

        private void chkVerEliminados_CheckedChanged(object sender, EventArgs e)
        {
            bool viendoEliminados = chkVerEliminados.Checked;
            btnNuevo.Enabled = !viendoEliminados && puedeAgregar;
            btnRestaurar.Enabled = viendoEliminados && puedeEliminar;
            limpiarCampos();
            bloquearCampos();
            cargarGrilla();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0) return;
            DialogResult confirm = MessageBox.Show(
                "¿Restaurar este paciente?", "Confirmar restauración", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bllPaciente.Restaurar(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0)
            {
                MessageBox.Show("Seleccioná un paciente primero.");
                return;
            }
            frmHistorialCambios frm = new frmHistorialCambios(idSeleccionado);
            frm.ShowDialog();
            cargarGrilla();
            bloquearCampos();
            limpiarCampos();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // bloquea el caracter
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }      
    }
}