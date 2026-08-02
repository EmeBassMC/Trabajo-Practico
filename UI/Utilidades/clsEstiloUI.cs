using System.Drawing;
using System.Windows.Forms;

namespace UI.Utilidades
{
    public static class clsEstiloUI
    {
        public static readonly Color ColorPrincipal = Color.FromArgb(45, 62, 80);
        public static readonly Color ColorTexto = Color.FromArgb(50, 50, 50);
        public static readonly Color ColorFondo = Color.FromArgb(245, 245, 245);
        public static readonly Color ColorSeleccion = Color.FromArgb(93, 135, 173);
        public static readonly Color ColorFilaAlterna = Color.FromArgb(240, 244, 248);

        public static void PersonalizarForm(Form form)
        {
            form.BackColor = ColorFondo;
            foreach (Control c in form.Controls)
                PersonalizarControlRecursivo(c);
        }

        private static void PersonalizarControlRecursivo(Control control)
        {
            if (control is Button btn)
            {
                btn.BackColor = ColorPrincipal;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Popup;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
            }
            else if (control is GroupBox gb)
            {
                gb.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                gb.ForeColor = ColorPrincipal;
            }
            else if (control is TabControl tc)
            {
                tc.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
            else if (control is Label lbl)
            {
                lbl.Font = new Font("Segoe UI", 9);
                lbl.ForeColor = ColorTexto;
            }
            else if (control is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Font = new Font("Segoe UI", 9);
            }
            else if (control is ComboBox cmb)
            {
                cmb.FlatStyle = FlatStyle.Popup;
                cmb.Font = new Font("Segoe UI", 9);
            }
            else if (control is CheckBox chk)
            {
                chk.Font = new Font("Segoe UI", 9);
                chk.ForeColor = ColorTexto;
            }
            else if (control is ListBox lst)
            {
                lst.Font = new Font("Segoe UI", 9);
                lst.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is TreeView trv)
            {
                trv.Font = new Font("Segoe UI", 9);
                trv.BorderStyle = BorderStyle.FixedSingle;
                trv.LineColor = ColorPrincipal;
            }

            foreach (Control child in control.Controls)
                PersonalizarControlRecursivo(child);
        }

        public static void EstilizarGrilla(DataGridView grilla)
        {
            grilla.EnableHeadersVisualStyles = false;
            grilla.BorderStyle = BorderStyle.None;
            grilla.BackgroundColor = Color.White;
            grilla.GridColor = Color.FromArgb(225, 225, 225);
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.RowHeadersVisible = false;

            grilla.ColumnHeadersDefaultCellStyle.BackColor = ColorPrincipal;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grilla.ColumnHeadersHeight = 32;

            grilla.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grilla.DefaultCellStyle.SelectionBackColor = ColorSeleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Color.White;
            grilla.AlternatingRowsDefaultCellStyle.BackColor = ColorFilaAlterna;

            grilla.RowTemplate.Height = 26;
            grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}