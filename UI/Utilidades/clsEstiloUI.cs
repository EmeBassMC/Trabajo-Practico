using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace UI.Utilidades
{
    public static class clsEstiloUI
    {
        public static readonly Color ColorPrincipal = Color.FromArgb(45, 62, 80);
        public static readonly Color ColorTexto = Color.FromArgb(50, 50, 50);
        public static readonly Color ColorFondo = Color.FromArgb(245, 245, 245);
        public static readonly Color ColorSeleccion = Color.FromArgb(93, 135, 173);
        public static readonly Color ColorFilaAlterna = Color.FromArgb(240, 244, 248);
        public static void AlinearJuntoAEtiqueta(Label etiqueta, Control campo, int separacion = 15)
        {
            campo.Left = etiqueta.Right + separacion;
        }
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        // Le pide a Windows 10/11 que pinte la barra de título oscura, para que
        // no desentone con el resto de la app. En Windows más viejo simplemente
        // no hace nada — no rompe.
        public static void AplicarBarraOscura(Form form)
        {
            try
            {
                int valor = 1;
                DwmSetWindowAttribute(form.Handle, 20, ref valor, sizeof(int));
            }
            catch { }
        }

        private static Icon _iconoApp;

        // Logo dibujado por código (círculo + "TS"), así no depende de ningún
        // archivo .ico externo que se pueda perder al mover el proyecto.
        public static Icon IconoApp
        {
            get
            {
                if (_iconoApp == null)
                {
                    Bitmap bmp = new Bitmap(64, 64);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.Clear(Color.Transparent);
                        using (Brush fondo = new SolidBrush(ColorPrincipal))
                            g.FillEllipse(fondo, 2, 2, 60, 60);
                        using (Font f = new Font("Segoe UI", 20, FontStyle.Bold))
                        using (Brush texto = new SolidBrush(Color.White))
                        {
                            SizeF tam = g.MeasureString("TS", f);
                            g.DrawString("TS", f, texto, (64 - tam.Width) / 2, (64 - tam.Height) / 2);
                        }
                    }
                    _iconoApp = Icon.FromHandle(bmp.GetHicon());
                }
                return _iconoApp;
            }
        }
        public static void PersonalizarForm(Form form)
        {
            form.Icon = IconoApp;
            AplicarBarraOscura(form);
            form.BackColor = ColorFondo;
            foreach (Control c in form.Controls)
                PersonalizarControlRecursivo(c);
            AlinearEtiquetasRecursivo(form);
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
        public static void EstilizarMenu(MenuStrip menu)
        {
            menu.RenderMode = ToolStripRenderMode.Professional;
            menu.Renderer = new ToolStripProfessionalRenderer(new ColoresMenu());
            menu.BackColor = ColorPrincipal;
            menu.ForeColor = Color.White;
            menu.Font = new Font("Segoe UI", 9.5f);

            foreach (ToolStripItem item in menu.Items)
            {
                if (item is ToolStripComboBox tscb)
                {
                    tscb.Font = new Font("Segoe UI", 9);
                    continue;
                }

                item.ForeColor = Color.White; // ítems de primer nivel: van sobre la barra oscura
                if (item is ToolStripMenuItem mi)
                    EstilizarDropDownRecursivo(mi.DropDownItems);
            }
        }

        private static void EstilizarDropDownRecursivo(ToolStripItemCollection items)
        {
            foreach (ToolStripItem sub in items)
            {
                sub.ForeColor = ColorTexto; // el desplegable tiene fondo blanco
                if (sub is ToolStripMenuItem subMenu)
                    EstilizarDropDownRecursivo(subMenu.DropDownItems);
            }
        }
        // Paleta para el menú: mismo esquema de color que el resto de la app,
        // aplicado al MenuStrip vía un Renderer (así el hover/seleccionado también queda temático).
        private class ColoresMenu : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => ColorPrincipal;
            public override Color MenuStripGradientEnd => ColorPrincipal;
            public override Color MenuItemSelected => ColorSeleccion;
            public override Color MenuItemSelectedGradientBegin => ColorSeleccion;
            public override Color MenuItemSelectedGradientEnd => ColorSeleccion;
            public override Color MenuItemPressedGradientBegin => ColorSeleccion;
            public override Color MenuItemPressedGradientEnd => ColorSeleccion;
            public override Color MenuItemBorder => ColorSeleccion;
            public override Color ImageMarginGradientBegin => ColorPrincipal;
            public override Color ImageMarginGradientMiddle => ColorPrincipal;
            public override Color ImageMarginGradientEnd => ColorPrincipal;
            public override Color ToolStripDropDownBackground => Color.White;
            public override Color MenuBorder => ColorPrincipal;
        }
        private const int SeparacionEtiquetaCampo = 15;

        // Busca, en cada contenedor, qué "campo" (TextBox/ComboBox/etc.) está pegado
        // a la derecha de cada Label, y lo suscribe para que se reacomode solo cada
        // vez que la etiqueta cambia de ancho (por ejemplo, al cambiar de idioma).
        // Recursivo: baja por GroupBox, Panel, TabPage, etc.
        private static void AlinearEtiquetasRecursivo(Control contenedor)
        {
            var controles = contenedor.Controls.Cast<Control>().ToList();

            foreach (Control c in controles)
            {
                if (c is Label lbl && lbl.AutoSize)
                {
                    Control campo = controles
                        .Where(o => o != lbl && EsControlDeCampo(o) && SeSuperponeVerticalmente(lbl, o) && o.Left >= lbl.Left)
                        .OrderBy(o => o.Left)
                        .FirstOrDefault();

                    if (campo != null)
                    {
                        campo.Left = lbl.Right + SeparacionEtiquetaCampo; // posición inicial
                        lbl.Resize += (s, e) => { campo.Left = lbl.Right + SeparacionEtiquetaCampo; }; // y de ahí en más, solo
                    }
                }
            }

            foreach (Control c in controles)
            {
                if (c.Controls.Count > 0)
                    AlinearEtiquetasRecursivo(c);
            }
        }

        private static bool EsControlDeCampo(Control c)
        {
            return c is TextBox || c is ComboBox || c is DateTimePicker || c is MaskedTextBox || c is NumericUpDown;
        }

        private static bool SeSuperponeVerticalmente(Control a, Control b)
        {
            int centroA = a.Top + a.Height / 2;
            return centroA >= b.Top && centroA <= b.Top + b.Height;
        }
    }

}