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
    public partial class frmIdiomas : Form, IObservadorIdioma
    {
        private clsIdiomaBLL bllIdioma = new clsIdiomaBLL();
        private clsTraduccionBLL bllTraduccion = new clsTraduccionBLL();

        public frmIdiomas()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(string idioma)
        {
            var g = clsGestorIdioma.GetInstancia();
            this.Text = g.Traducir("titleIdiomas");
            btnAgregar.Text = g.Traducir("btnAgregar");
            btnEditar.Text = g.Traducir("btnEditar");
            btnEliminar.Text = g.Traducir("btnEliminar");
            lblCodigo.Text = g.Traducir("lblCodigo");
            lblNombre.Text = g.Traducir("lblNombre");
            tabIdiomas.Text = g.Traducir("tabIdiomas");
            tabTraducciones.Text = g.Traducir("tabTraducciones");
            btnGuardarTraduccion.Text = g.Traducir("btnGuardarTraduccion");
            lblSeleccionarIdioma.Text = g.Traducir("lblIdioma");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstIdiomas.SelectedItem == null)
                {
                    MessageBox.Show("Seleccioná un idioma primero.");
                    return;
                }

                clsIdiomaBE idioma = lstIdiomas.SelectedItem as clsIdiomaBE;

                if (idioma.Codigo == "es" || idioma.Codigo == "en")
                {
                    MessageBox.Show("No podés eliminar los idiomas base del sistema.");
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "¿Eliminás el idioma '" + idioma.Nombre + "'? Se borrarán todas sus traducciones.",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    if (bllIdioma.Delete(idioma.IdIdioma))
                        CargarIdiomas();
                    else
                        MessageBox.Show("No se pudo eliminar el idioma.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstIdiomas.SelectedItem == null)
                {
                    MessageBox.Show("Seleccioná un idioma primero.");
                    return;
                }

                clsIdiomaBE idioma = lstIdiomas.SelectedItem as clsIdiomaBE;

                if (idioma.Codigo == "es" || idioma.Codigo == "en")
                {
                    MessageBox.Show("No podés editar los idiomas base del sistema.");
                    return;
                }

                idioma.Codigo = txtCodigo.Text.Trim();
                idioma.Nombre = txtNombre.Text.Trim();

                if (bllIdioma.Update(idioma))
                    CargarIdiomas();
                else
                    MessageBox.Show("No se pudo actualizar el idioma.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text))
                {
                    MessageBox.Show("Completá el código y el nombre.");
                    return;
                }

                if (txtCodigo.Text.Trim().ToLower() == "es" || txtCodigo.Text.Trim().ToLower() == "en")
                {
                    MessageBox.Show("No podés agregar un idioma con ese código, ya existe.");
                    return;
                }

                clsIdiomaBE idioma = new clsIdiomaBE();
                idioma.Codigo = txtCodigo.Text.Trim();
                idioma.Nombre = txtNombre.Text.Trim();

                if (bllIdioma.Insert(idioma))
                {
                    clsIdiomaBE creado = bllIdioma.GetAll()
                        .OrderByDescending(i => i.IdIdioma)
                        .FirstOrDefault();

                    if (creado != null)
                        bllIdioma.CrearClavesVaciasParaIdioma(creado.IdIdioma);

                    txtCodigo.Text = "";
                    txtNombre.Text = "";
                    CargarIdiomas();
                }
                else
                    MessageBox.Show("No se pudo agregar el idioma.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void lstIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstIdiomas.SelectedItem == null) return;
            clsIdiomaBE idioma = lstIdiomas.SelectedItem as clsIdiomaBE;
            txtCodigo.Text = idioma.Codigo;
            txtNombre.Text = idioma.Nombre;
        }
        private void CargarIdiomas()
        {
            lstIdiomas.DataSource = null;
            lstIdiomas.DataSource = bllIdioma.GetAll();
            lstIdiomas.DisplayMember = "Nombre";
            lstIdiomas.ValueMember = "IdIdioma";

            // sincronizar el combo de la solapa 2
            cmbIdioma.DataSource = null;
            cmbIdioma.DataSource = bllIdioma.GetAll();
            cmbIdioma.DisplayMember = "Nombre";
            cmbIdioma.ValueMember = "IdIdioma";


            // cargar traducciones del primer idioma del combo
            if (cmbIdioma.SelectedItem != null)
            {
                clsIdiomaBE idioma = cmbIdioma.SelectedItem as clsIdiomaBE;
                CargarTraducciones(idioma.IdIdioma);
            }
        }

        private void frmIdiomas_Shown(object sender, EventArgs e)
        {
            int idUsuario = clsSesionActual.GetInstancia().IdUsuario;
            clsRolBLL bllRol = new clsRolBLL();

            bool puedeGestionar = bllRol.TienePermiso(idUsuario, "Idiomas.Gestionar");

            btnAgregar.Enabled = puedeGestionar;
            btnEditar.Enabled = puedeGestionar;
            btnEliminar.Enabled = puedeGestionar;
            btnGuardarTraduccion.Enabled = puedeGestionar;
            btnEscanearForms.Enabled = puedeGestionar;

            dgvTraducciones.RowHeadersVisible = false;

            CargarIdiomas();
            clsGestorIdioma.GetInstancia().Suscribir(this);
            ActualizarIdioma(clsGestorIdioma.GetInstancia().IdiomaActual);
            PersonalizarForm();
        }
        private void PersonalizarForm()
        {
            clsEstiloUI.PersonalizarForm(this);
            clsEstiloUI.EstilizarGrilla(dgvTraducciones);
        }

        private void frmIdiomas_FormClosed(object sender, FormClosedEventArgs e)
        {
            clsGestorIdioma.GetInstancia().Desuscribir(this);
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedItem == null) return;
            clsIdiomaBE idioma = cmbIdioma.SelectedItem as clsIdiomaBE;
            CargarTraducciones(idioma.IdIdioma);
        }
        private void CargarTraducciones(int idIdioma)
        {
            dgvTraducciones.DataSource = null;
            dgvTraducciones.DataSource = bllTraduccion.GetByIdioma(idIdioma);
            dgvTraducciones.Columns["IdIdioma"].Visible = false;
            dgvTraducciones.Columns["IdClave"].Visible = false;  
            dgvTraducciones.Columns["IdTraduccion"].Visible = false;  
            dgvTraducciones.Columns["Formulario"].HeaderText = "Formulario";
            dgvTraducciones.Columns["Formulario"].ReadOnly = true;
            dgvTraducciones.Columns["Formulario"].DisplayIndex = 1; // justo después de Clave
            AplicarFiltroSinUso();
        }

        // Oculta las filas cuyo Formulario quedó marcado "(sin uso)" no las lee ningún control, así que estorban al buscar qué traducir.
        private void AplicarFiltroSinUso()
        {
            dgvTraducciones.CurrentCell = null;
            foreach (DataGridViewRow fila in dgvTraducciones.Rows)
            {
                object valorFormulario = fila.Cells["Formulario"].Value;
                bool esSinUso = valorFormulario != null && valorFormulario.ToString() == "(sin uso)";
                fila.Visible = !(chkOcultarSinUso.Checked && esSinUso);
            }
        }

        private void chkOcultarSinUso_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltroSinUso();
        }
        private void btnGuardarTraduccion_Click(object sender, EventArgs e)
        {
            try
            {
                // Si hay una celda en edición (escribiste y todavía no saliste de ella con
                // Tab/clic afuera), el DataGridView no bajó el texto nuevo al objeto todavía.
                // Sin este EndEdit, se guardaría el valor viejo.
                dgvTraducciones.EndEdit();

                if (dgvTraducciones.CurrentRow == null) return;

                clsTraduccionBE t = dgvTraducciones.CurrentRow.DataBoundItem as clsTraduccionBE;
                if (t == null) return;

                if (bllTraduccion.Update(t))
                {
                    // recargar el idioma actual en el gestor y notificar a todos
                    clsGestorIdioma.GetInstancia().CambiarIdioma(
                        clsGestorIdioma.GetInstancia().IdiomaActual);

                    if (cmbIdioma.SelectedItem != null)
                    {
                        clsIdiomaBE idioma = cmbIdioma.SelectedItem as clsIdiomaBE;
                        CargarTraducciones(idioma.IdIdioma);
                    }
                }
                else
                    MessageBox.Show("No se pudo guardar la traducción.");
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnEscanearForms_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> claves = new Dictionary<string, string>();
                Dictionary<string, string> formularios = new Dictionary<string, string>();

                var assembly = System.Reflection.Assembly.GetAssembly(typeof(frmIdiomas));
                var tiposForm = assembly.GetTypes()
                    .Where(t => typeof(Form).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var tipo in tiposForm)
                {
                    try
                    {
                        Form instancia = (Form)Activator.CreateInstance(tipo);
                        ExtraerClaves(instancia.Controls, claves, tipo.Name, formularios);
                        instancia.Dispose();
                    }
                    catch { }
                }

                int insertados = bllTraduccion.EscanearYGenerarClaves(claves, formularios);
                MessageBox.Show("Se generaron " + insertados + " claves nuevas.");

                if (cmbIdioma.SelectedItem != null)
                    CargarTraducciones(((clsIdiomaBE)cmbIdioma.SelectedItem).IdIdioma);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void RegistrarFormulario(string clave, string nombreFormulario, Dictionary<string, string> formularios)
        {
            if (!formularios.ContainsKey(clave))
                formularios[clave] = nombreFormulario;
            else if (!formularios[clave].Split(',').Select(s => s.Trim()).Contains(nombreFormulario))
                formularios[clave] += ", " + nombreFormulario;
        }

        private void ExtraerClaves(Control.ControlCollection controles, Dictionary<string, string> claves, string nombreFormulario, Dictionary<string, string> formularios)
        {
            foreach (Control c in controles)
            {
                if (c is Button || c is Label || c is GroupBox || c is TabPage)
                {
                    if (!string.IsNullOrEmpty(c.Name) && !claves.ContainsKey(c.Name))
                    {
                        string texto = string.IsNullOrEmpty(c.Text) ? c.Name : c.Text;
                        claves.Add(c.Name, texto);
                    }
                    if (!string.IsNullOrEmpty(c.Name))
                        RegistrarFormulario(c.Name, nombreFormulario, formularios);
                }

                // para MenuStrip extraer los items
                if (c is MenuStrip)
                {
                    ExtraerItemsMenu(((MenuStrip)c).Items, claves, nombreFormulario, formularios);
                }

                if (c.Controls.Count > 0)
                    ExtraerClaves(c.Controls, claves, nombreFormulario, formularios);
            }
        }

        private void ExtraerItemsMenu(System.Windows.Forms.ToolStripItemCollection items, Dictionary<string, string> claves, string nombreFormulario, Dictionary<string, string> formularios)
        {
            foreach (ToolStripItem item in items)
            {
                if (!string.IsNullOrEmpty(item.Name) && !claves.ContainsKey(item.Name))
                {
                    string texto = string.IsNullOrEmpty(item.Text) ? item.Name : item.Text;
                    claves.Add(item.Name, texto);
                }
                if (!string.IsNullOrEmpty(item.Name))
                    RegistrarFormulario(item.Name, nombreFormulario, formularios);
                if (item is ToolStripMenuItem)
                    ExtraerItemsMenu(((ToolStripMenuItem)item).DropDownItems, claves, nombreFormulario, formularios);
            }
        }


        private void dgvTraducciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (DataGridViewRow fila in dgvTraducciones.Rows)
                {
                    string id = fila.Cells["IdTraduccion"].Value?.ToString() ?? "";
                    string clave = fila.Cells["Clave"].Value?.ToString() ?? "";
                    string texto = fila.Cells["Texto"].Value?.ToString() ?? "";
                    sb.AppendLine(id + "\t" + clave + "\t" + texto);
                }

                string path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "traducciones.txt");
                System.IO.File.WriteAllText(path, sb.ToString());
                MessageBox.Show("Exportado en: " + path);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void tabTraducciones_Click(object sender, EventArgs e)
        {

        }
    }
}