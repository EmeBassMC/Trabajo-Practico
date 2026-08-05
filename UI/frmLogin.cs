using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Utilidades;

namespace UI
{
    public partial class frmLogin : Form, IObservadorIdioma
    {
        clsUsuarioBLL usuario = new clsUsuarioBLL();
        private readonly bool integridadOk;

        // Constructor sin parametros por compatibilidad, por si algo en el proyecto
        // todavia hace "new frmLogin()" a mano. Asume integridad OK.
        public frmLogin() : this(true) { }

        public frmLogin(bool integridadOk)
        {
            InitializeComponent();
            this.integridadOk = integridadOk;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
            clsGestorIdioma.GetInstancia().Suscribir(this);
            personalizarForm();
            CargarComboIdiomas();
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
            txtUsuario.Focus();

        }
        private void CargarComboIdiomas()
        {
            clsIdiomaBLL bllIdioma = new clsIdiomaBLL();
            var idiomas = bllIdioma.GetAll();
            cmbIdioma.Items.Clear();
            foreach (var idioma in idiomas)
                cmbIdioma.Items.Add(idioma.Codigo);
            cmbIdioma.SelectedItem = clsGestorIdioma.GetInstancia().IdiomaActual;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                label3.Text = "Complete todos los campos";
                label3.Visible = true;
                return;
            }

            bool resultado = usuario.Login(txtUsuario.Text, txtPassword.Text);
            if (resultado)
            {
                clsUsuarioBE u = usuario.GetByUsername(txtUsuario.Text);

                if (!integridadOk)
                {
                    clsRolBLL rolBll = new clsRolBLL();
                    if (!rolBll.TienePermiso(u.IdUsuario, "Sistema.Mantenimiento"))
                    {
                        label3.Text = "Se detectaron inconsistencias en la base. Solo un administrador puede ingresar.";
                        label3.Visible = true;
                        txtPassword.Clear();
                        return; // no guarda sesion, no cierra el form: se queda en el login
                    }

                    MessageBox.Show(
                        "Se detectaron inconsistencias de integridad en la base de datos.\n" +
                        "Podés ingresar por ser administrador, pero se recomienda recalcular los dígitos " +
                        "verificadores o restaurar un backup desde el menú \"SOLO ADMIN\".",
                        "Integridad comprometida",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                clsSesionActual.GetInstancia().IdUsuario = u.IdUsuario;
                clsSesionActual.GetInstancia().NombreUsuario = txtUsuario.Text;
                this.Close();
            }
            else
            {
                label3.Text = "Usuario o contraseña incorrectos";
                label3.Visible = true;
            }
        }



        private void personalizarForm()
        {
            this.Icon = clsEstiloUI.IconoApp;
            clsEstiloUI.AplicarBarraOscura(this);
            this.BackColor = Color.FromArgb(45, 62, 80);
            this.Text = "TurnoSync | Login";

            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 26, FontStyle.Bold);

            lblSubtitulo.ForeColor = Color.FromArgb(189, 195, 199);
            lblSubtitulo.Font = new Font("Segoe UI", 10);

            cmbIdioma.Font = new Font("Segoe UI", 9);
            cmbIdioma.FlatStyle = FlatStyle.Popup;

            lblUsuario.ForeColor = Color.White;
            lblUsuario.Font = new Font("Segoe UI", 10);
            lblContraseña.ForeColor = Color.White;
            lblContraseña.Font = new Font("Segoe UI", 10);
            lblIdioma.ForeColor = Color.White;
            lblIdioma.Font = new Font("Segoe UI", 9);
            label3.ForeColor = Color.FromArgb(231, 76, 60);
            label3.Font = new Font("Segoe UI", 9);

            txtUsuario.Font = new Font("Segoe UI", 11);
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            button1.BackColor = Color.FromArgb(52, 152, 219);
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Popup;
            button1.FlatAppearance.BorderSize = 0;
            button1.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button1.Cursor = Cursors.Hand;
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            lblUsuario.Text = g.Traducir("lblUsuario");
            lblContraseña.Text = g.Traducir("lblClave");
            lblIdioma.Text = g.Traducir("lblIdioma");
            button1.Text = g.Traducir("btnIngresar");
        }
        private void lblIdioma_Click(object sender, EventArgs e)
        {

        }

        private void lblUsuario_Click(object sender, EventArgs e)
        {

        }

        private void lblContraseña_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem == null) return;
            clsGestorIdioma.GetInstancia().CambiarIdioma(cmbIdioma.SelectedItem.ToString());
        }
    }
}