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

namespace UI.forms
{
    public partial class frmBitacora : Form, IObservadorIdioma
    {
        clsBitacoraBLL bllBitacora = new clsBitacoraBLL();
        clsUsuarioBLL usuarioBLL = new clsUsuarioBLL();


        public frmBitacora()
        {
            InitializeComponent();
        }

        private void frmBitacora_Load(object sender, EventArgs e)
        {
            cargarGrilla();
            CargarFiltros();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
            clsEstiloUI.PersonalizarForm(this);
            clsEstiloUI.EstilizarGrilla(dataGridView1);
        }
        private void cargarGrilla()
        {
            dataGridView1.DataSource = bllBitacora.GetAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargarGrilla();
        }
        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();

            button1.Text = g.Traducir("btnRefrescar");
            this.Text = g.Traducir("titleBitacora");

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["Id"].HeaderText = g.Traducir("colID");
                dataGridView1.Columns["Fecha"].HeaderText = g.Traducir("colFecha");
                dataGridView1.Columns["UsuarioId"].HeaderText = g.Traducir("colUsuario");
                dataGridView1.Columns["Actividad"].HeaderText = g.Traducir("colActividad");
                dataGridView1.Columns["Informacion"].HeaderText = g.Traducir("colInformacion");
            }
        }
        private void CargarFiltros()
        {
            List<clsUsuarioBE> usuarios = usuarioBLL.GetAll();
            List<clsUsuarioBE> listaUsuarios = new List<clsUsuarioBE> {
                new clsUsuarioBE { IdUsuario = 0, NombreUsuario = "Todos" }
            };
            listaUsuarios.AddRange(usuarios);
            cmbUsuario.DataSource = listaUsuarios;
            cmbUsuario.DisplayMember = "NombreUsuario";
            cmbUsuario.ValueMember = "IdUsuario";

            List<string> actividades = new List<string> { "Todas" };
            actividades.AddRange(bllBitacora.GetActividadesDistintas());
            cmbActividad.DataSource = actividades;
        }

        private void frmBitacora_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            clsBitacoraFiltroBE filtro = new clsBitacoraFiltroBE();

            filtro.FechaDesde = dtpDesde.Checked ? dtpDesde.Value : (DateTime?)null;
            filtro.FechaHasta = dtpHasta.Checked ? dtpHasta.Value : (DateTime?)null;

            int idUsuarioSeleccionado = (int)cmbUsuario.SelectedValue;
            filtro.UsuarioId = idUsuarioSeleccionado == 0 ? (int?)null : idUsuarioSeleccionado;

            string actividadSeleccionada = cmbActividad.SelectedItem as string;
            filtro.Actividad = actividadSeleccionada == "Todas" ? null : actividadSeleccionada;

            dataGridView1.DataSource = bllBitacora.GetFiltrado(filtro);
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
