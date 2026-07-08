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

namespace UI
{
    public partial class frmPrincipal : Form , IObservadorIdioma
    {
        clsRolBLL rolBll = new clsRolBLL();
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void turnosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Pacientes.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmPacientes frm = new frmPacientes();
            frm.MdiParent = this;
            frm.Show();
        }

        private void aBMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Turnos.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmTurnos frm = new frmTurnos();
            frm.MdiParent = this;
            frm.Show();
        }

        private void especialidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Especialidades.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmEspecialidad frm = new frmEspecialidad();
            frm.MdiParent = this;
            frm.Show();
        }

        private void profesionalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Profesionales.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmProfesional frm = new frmProfesional();
            frm.MdiParent = this;
            frm.Show();
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Bitacora.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmBitacora frm = new frmBitacora();
            frm.MdiParent = this;
            frm.Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Roles.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            Form1 frm = new Form1 ();
            frm.MdiParent = this;
            frm.Show();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            clsGestorIdioma.GetInstancia().Suscribir(this);
            CargarComboIdiomas();
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
        }
        private void CargarComboIdiomas()
        {
            clsIdiomaBLL bllIdioma = new clsIdiomaBLL();
            var idiomas = bllIdioma.GetAll();

            cmbIdiomaPrincipal.Items.Clear();
            foreach (var idioma in idiomas)
                cmbIdiomaPrincipal.Items.Add(idioma.Codigo);

            cmbIdiomaPrincipal.SelectedItem = clsGestorIdioma.GetInstancia().IdiomaActual;
        }

        private void consultasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Usuarios.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmUsuario frm = new frmUsuario();
            frm.MdiParent = this;
            frm.Show();
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();
            idiomasToolStripMenuItem.Text = g.Traducir("idiomasToolStripMenuItem");
            inicioToolStripMenuItem.Text = g.Traducir("mnuGestiones");
            informesToolStripMenuItem.Text = g.Traducir("mnuInformes");
            salirToolStripMenuItem.Text = g.Traducir("mnuSalir");
            pacientesToolStripMenuItem.Text = g.Traducir("mnuPacientes");
            especialidadesToolStripMenuItem.Text = g.Traducir("mnuEspecialidades");
            profesionalesToolStripMenuItem.Text = g.Traducir("mnuProfesionales");
            aBMToolStripMenuItem.Text = g.Traducir("mnuTurnos");
            rolesToolStripMenuItem.Text = g.Traducir("mnuRoles");
            usuariosToolStripMenuItem.Text = g.Traducir("mnuUsuarios");
            bitacoraToolStripMenuItem.Text = g.Traducir("mnuBitacora");
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void cmbIdiomaPrincipal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdiomaPrincipal.SelectedItem == null) return;

            string codigo = cmbIdiomaPrincipal.SelectedItem.ToString();
            clsGestorIdioma.GetInstancia().CambiarIdioma(codigo);

            if (codigo != "en" && clsGestorIdioma.GetInstancia().TieneClavesSinTraducir())
            {
                MessageBox.Show(
                    "Este idioma todavía no tiene todas las traducciones cargadas. " +
                    "Los textos faltantes se van a mostrar en inglés.",
                    "Idioma incompleto",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void idiomasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Idiomas.Ver"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }
            frmIdiomas frm = new frmIdiomas();
            frm.MdiParent = this;
            frm.Show();
        }

        private void sOLOADMINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!rolBll.TienePermiso(clsSesionActual.GetInstancia().IdUsuario, "Sistema.Mantenimiento"))
            {
                MessageBox.Show(clsGestorIdioma.GetInstancia().Traducir("msgSinPermisos"));
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Esto va a recalcular el DVH/DVV de todos los pacientes a partir de sus datos actuales.\n" +
                "Usalo solo si sabés lo que estás haciendo (por ejemplo, después de cargar datos iniciales).\n\n" +
                "¿Continuar?",
                "Recalcular Dígitos Verificadores",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                clsDigitoVerificador.RecalcularTodos();
                MessageBox.Show("Recálculo completado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recalcular: " + ex.Message);
            }
        }
    }
     
}
