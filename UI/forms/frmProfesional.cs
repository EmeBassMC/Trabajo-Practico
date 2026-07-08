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

namespace UI
{
    public partial class frmProfesional : Form, IObservadorIdioma
    {
        clsProfesionalBLL bllprofesional = new clsProfesionalBLL();
        clsRolBLL bllRol = new clsRolBLL();
        bool puedeAgregar, puedeModificar, puedeEliminar;
        bool modoEdicion = false;
        int idSeleccionado = 0;

        public frmProfesional()
        {
            InitializeComponent();
        }

        private void frmProfesional_Load(object sender, EventArgs e)
        {
            int idUsuario = clsSesionActual.GetInstancia().IdUsuario;
            puedeAgregar = bllRol.TienePermiso(idUsuario, "Profesionales.Agregar");
            puedeModificar = bllRol.TienePermiso(idUsuario, "Profesionales.Modificar");
            puedeEliminar = bllRol.TienePermiso(idUsuario, "Profesionales.Eliminar");

            cargarCombo();
            cargarGrilla();
            bloquearCampos();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
        }

        public void cargarGrilla()
        {
            dataGridView1.DataSource = null;
            var lista = chkVerEliminados.Checked ? bllprofesional.GetEliminados() : bllprofesional.GetAll();
            dataGridView1.DataSource = lista;
            if (dataGridView1.Columns["Activo"] != null)
                dataGridView1.Columns["Activo"].Visible = false;
            if (dataGridView1.Columns["IdEspecialidad"] != null)
                dataGridView1.Columns["IdEspecialidad"].Visible = false;
            if (dataGridView1.Columns["Telefono"] != null)
                dataGridView1.Columns["Telefono"].Visible = false;
            if (dataGridView1.Columns["Email"] != null)
                dataGridView1.Columns["Email"].Visible = false;
        }
        public void bloquearCampos()
        {
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDNI.Enabled = false;
            dtpFechaNacimiento.Enabled = false;
            btnNuevo.Enabled = true;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
            txtMatricula.Enabled = false;
            cmbEspecialidad.Enabled = false;
        }
        public void habilitarCampos()
        {
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtDNI.Enabled = true;
            dtpFechaNacimiento.Enabled = true;
            btnNuevo.Enabled = false;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            txtMatricula.Enabled = true;
            cmbEspecialidad.Enabled = true;
        }
        public void limpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDNI.Text = "";
            txtMatricula.Text = "";
            dtpFechaNacimiento.Value = DateTime.Now;
            idSeleccionado = 0;
        }
        public void cargarCombo()
        {
            clsEspecialidadBLL bllEsp = new clsEspecialidadBLL();
            var lista = bllEsp.GetAll();
            cmbEspecialidad.DataSource = lista;
            cmbEspecialidad.DisplayMember = "Nombre";
            cmbEspecialidad.ValueMember = "IdEspecialidad";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            var g = clsGestorIdioma.GetInstancia();
            int edad = DateTime.Now.Year - dtpFechaNacimiento.Value.Year;
            if (dtpFechaNacimiento.Value.Date > DateTime.Now.AddYears(-edad)) edad--;
            if (!txtDNI.Text.All(char.IsDigit))
            {
                MessageBox.Show("El DNI debe contener solo números.");
                return;
            }
            if (edad < 21)
            {
                MessageBox.Show("El profesional debe ser mayor de 21 años.");
                return;
            }
            
            if (modoEdicion == false)
            {


                if (modoEdicion == false)
                {
                    clsProfesionalBE profesional = new clsProfesionalBE();

                    //tiramos los valores de email y telefono a null ya que no aplican a esto.
                    profesional.Email = null;
                    profesional.Telefono = null;
                    profesional.Nombre = txtNombre.Text;
                    profesional.Apellido = txtApellido.Text;
                    profesional.DNI = txtDNI.Text;
                    profesional.Matricula = txtMatricula.Text;
                    profesional.IdEspecialidad = (int)cmbEspecialidad.SelectedValue;
                    profesional.FechaNacimiento = dtpFechaNacimiento.Value;

                    bool resultado = bllprofesional.Insert(profesional);
                    if (resultado == true)
                        MessageBox.Show(g.Traducir("msgGuardadoExito"));
                    else
                        MessageBox.Show(g.Traducir("msgErrorGuardar"));
                }
                else if (modoEdicion == true)
                {
                    clsProfesionalBE profesional = new clsProfesionalBE();
                    //tiramos los valores de email y telefono a null ya que no aplican a esto.
                    profesional.Email = null;
                    profesional.Telefono = null;
                    profesional.Nombre = txtNombre.Text;
                    profesional.Apellido = txtApellido.Text;
                    profesional.DNI = txtDNI.Text;
                    profesional.Matricula = txtMatricula.Text;
                    profesional.IdEspecialidad = (int)cmbEspecialidad.SelectedValue;
                    profesional.FechaNacimiento = dtpFechaNacimiento.Value;
                    profesional.IdPersona = idSeleccionado;
                    bool resultado = bllprofesional.Update(profesional);
                    if (resultado == true)
                        MessageBox.Show(g.Traducir("msgActualizadoExito"));
                    else
                        MessageBox.Show(g.Traducir("msgErrorActualizar"));
                }
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            limpiarCampos();
            habilitarCampos();
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
                bllprofesional.Delete(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                    idSeleccionado = Convert.ToInt32(fila.Cells["IdPersona"].Value);
                    txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                    txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                    txtDNI.Text = fila.Cells["DNI"].Value.ToString();
                    txtMatricula.Text = fila.Cells["Matricula"].Value.ToString();
                    cmbEspecialidad.SelectedValue = Convert.ToInt32(fila.Cells["IdEspecialidad"].Value);
                    dtpFechaNacimiento.Value = Convert.ToDateTime(fila.Cells["FechaNacimiento"].Value);
                    modoEdicion = true;
                    habilitarCampos();
                    btnGuardar.Enabled = puedeModificar;
                    btnEliminar.Enabled = puedeEliminar;
                    btnRestaurar.Enabled = chkVerEliminados.Checked && puedeEliminar;

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void grpProfesional_Enter(object sender, EventArgs e)
        {

        }

        private void lblEspecialidad_Click(object sender, EventArgs e)
        {

        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0) return;

            DialogResult confirm = MessageBox.Show(
                "¿Restaurar este profesional?", "Confirmar restauración", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                bllprofesional.Restaurar(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void chkVerEliminados_CheckedChanged(object sender, EventArgs e)
        {
            bool viendoEliminados = chkVerEliminados.Checked;

            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtDNI.Enabled = false;
            txtMatricula.Enabled = false;
            cmbEspecialidad.Enabled = false;
            dtpFechaNacimiento.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;

            btnNuevo.Enabled = !viendoEliminados && puedeAgregar;
            btnRestaurar.Enabled = false; // se habilita recién al seleccionar una fila

            idSeleccionado = 0;
            modoEdicion = false;

            cargarGrilla();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // bloquea el caracter
            }
        }

        private void txtMatricula_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // bloquea el caracter
            }
        }

        private void frmProfesional_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            this.Text = g.Traducir("titleProfesionales");
            btnNuevo.Text = g.Traducir("btnNuevo");
            btnGuardar.Text = g.Traducir("btnGuardar");
            btnEliminar.Text = g.Traducir("btnEliminar");
            btnCancelar.Text = g.Traducir("btnCancelar");
            lblNombre.Text = g.Traducir("lblNombre");
            lblApellido.Text = g.Traducir("lblApellido");
            lblDNI.Text = g.Traducir("lblDNI");
            lblMatricula.Text = g.Traducir("lblMatricula");
            lblEspecialidad.Text = g.Traducir("lblEspecialidad");
            lblFechaNac.Text = g.Traducir("lblFechaNac");
            grpProfesional.Text = g.Traducir("grpProfesional");


            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["IdPersona"].HeaderText = g.Traducir("colID");
                dataGridView1.Columns["Nombre"].HeaderText = g.Traducir("colNombre");
                dataGridView1.Columns["Apellido"].HeaderText = g.Traducir("colApellido");
                dataGridView1.Columns["DNI"].HeaderText = g.Traducir("colDNI");
                dataGridView1.Columns["Telefono"].HeaderText = g.Traducir("colTelefono");
                dataGridView1.Columns["Email"].HeaderText = g.Traducir("colEmail");
                dataGridView1.Columns["FechaNacimiento"].HeaderText = g.Traducir("colFechaNac");
                dataGridView1.Columns["Matricula"].HeaderText = g.Traducir("colMatricula");
                dataGridView1.Columns["IdEspecialidad"].HeaderText = g.Traducir("colEspecialidad");
                dataGridView1.Columns["NombreEspecialidad"].HeaderText = g.Traducir("colEspecialidad");

            }
        }
    }
}
