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
    public partial class frmEspecialidad : Form, IObservadorIdioma
    {

        clsEspecialidadBLL bllEspecialidad = new clsEspecialidadBLL();
        clsRolBLL bllRol = new clsRolBLL();
        bool puedeAgregar, puedeModificar, puedeEliminar;
        bool modoEdicion = false;
        int idSeleccionado = 0;
        public frmEspecialidad()
        {
            InitializeComponent();
        }

        private void frmEspecialidad_Load(object sender, EventArgs e)
        {
            int idUsuario = clsSesionActual.GetInstancia().IdUsuario;
            puedeAgregar = bllRol.TienePermiso(idUsuario, "Especialidades.Agregar");
            puedeModificar = bllRol.TienePermiso(idUsuario, "Especialidades.Modificar");
            puedeEliminar = bllRol.TienePermiso(idUsuario, "Especialidades.Eliminar");

            cargarGrilla();
            bloquearCampos();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
        }



        public void cargarGrilla()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = chkVerEliminados.Checked ? bllEspecialidad.GetEliminadas() : bllEspecialidad.GetAll();
            if (dataGridView1.Columns["Activo"] != null)
                dataGridView1.Columns["Activo"].Visible = false;
        }
        public void bloquearCampos()
        {
            txtNombre.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;    
            btnGuardar.Enabled = false;
            btnNuevo.Enabled = true;
        }
        public void habilitarCampos()
        {
            txtNombre.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = true;
            btnGuardar.Enabled = true;
            btnNuevo.Enabled = false;
        }
        public void limpiarCampos()
        {
            txtNombre.Text = "";
            idSeleccionado = 0;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            modoEdicion = false;
            btnNuevo.Enabled = puedeAgregar;  
            limpiarCampos();
            habilitarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var g = clsGestorIdioma.GetInstancia(); 
            clsEspecialidadBE especialidadBE = new clsEspecialidadBE();
            if (modoEdicion == false)
            {
                especialidadBE.Nombre = txtNombre.Text;
                bool resultado = bllEspecialidad.Insert(especialidadBE);
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
                especialidadBE.IdEspecialidad = idSeleccionado;  
                especialidadBE.Nombre = txtNombre.Text;
                bool resultado = bllEspecialidad.Update(especialidadBE);
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
                bllEspecialidad.Delete(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idSeleccionado = Convert.ToInt32(fila.Cells["IdEspecialidad"].Value);
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                modoEdicion = true;
                habilitarCampos();
                btnGuardar.Enabled = puedeModificar;   
                btnEliminar.Enabled = puedeEliminar;
                button1 .Enabled = chkVerEliminados.Checked && puedeEliminar;

            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            bloquearCampos();
            modoEdicion = false;
        }

        private void chkVerEliminados_CheckedChanged(object sender, EventArgs e)
        {
            bool viendoEliminados = chkVerEliminados.Checked;

            txtNombre.Enabled = false;
            txtNombre.Text = "";
            btnCancelar.Enabled = false;
            btnGuardar.Enabled = false;
            btnEliminar.Enabled = false;
            btnNuevo.Enabled = !viendoEliminados && puedeAgregar;
            button1.Enabled = viendoEliminados && puedeEliminar;

            idSeleccionado = 0;
            modoEdicion = false;

            cargarGrilla();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (idSeleccionado <= 0) return;
            var g = clsGestorIdioma.GetInstancia();
            DialogResult confirm = MessageBox.Show(
                "¿Restaurar esta especialidad?", "Confirmar restauración", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                bllEspecialidad.Restaurar(idSeleccionado);
                cargarGrilla();
                bloquearCampos();
                limpiarCampos();
            }
        }

        private void frmEspecialidad_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            groupBox1.Text = g.Traducir("grpEspecialidades");
            lblNombre.Text = g.Traducir("lblNombre");
            btnNuevo.Text = g.Traducir("btnNuevo");
            btnGuardar.Text = g.Traducir("btnGuardar");
            btnEliminar.Text = g.Traducir("btnEliminar");
            btnCancelar.Text = g.Traducir("btnCancelar");
            this.Text = g.Traducir("titleEspecialidades");
        }
    }
}
