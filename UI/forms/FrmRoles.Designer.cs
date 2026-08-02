namespace UI.forms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.trvGruposJerarquia = new System.Windows.Forms.TreeView();
            this.trvPermisos = new System.Windows.Forms.TreeView();
            this.lblPermisos = new System.Windows.Forms.Label();
            this.lblGrupos = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnActualizarAsignacion = new System.Windows.Forms.Button();
            this.btnEliminarGrupo = new System.Windows.Forms.Button();
            this.btnCrearGrupo = new System.Windows.Forms.Button();
            this.lstGrupos = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.trvRolesUsuario = new System.Windows.Forms.TreeView();
            this.trvRolesAsignados = new System.Windows.Forms.TreeView();
            this.lblRolesAsignados = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGuardarUsuario = new System.Windows.Forms.Button();
            this.lstUsuarios = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1080, 652);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.trvGruposJerarquia);
            this.tabPage1.Controls.Add(this.trvPermisos);
            this.tabPage1.Controls.Add(this.lblPermisos);
            this.tabPage1.Controls.Add(this.lblGrupos);
            this.tabPage1.Controls.Add(this.txtNombre);
            this.tabPage1.Controls.Add(this.btnActualizarAsignacion);
            this.tabPage1.Controls.Add(this.btnEliminarGrupo);
            this.tabPage1.Controls.Add(this.btnCrearGrupo);
            this.tabPage1.Controls.Add(this.lstGrupos);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1072, 626);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Grupos";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // trvGruposJerarquia
            // 
            this.trvGruposJerarquia.Location = new System.Drawing.Point(337, 57);
            this.trvGruposJerarquia.Name = "trvGruposJerarquia";
            this.trvGruposJerarquia.Size = new System.Drawing.Size(373, 423);
            this.trvGruposJerarquia.TabIndex = 6;
            // 
            // trvPermisos
            // 
            this.trvPermisos.CheckBoxes = true;
            this.trvPermisos.Location = new System.Drawing.Point(716, 27);
            this.trvPermisos.Name = "trvPermisos";
            this.trvPermisos.Size = new System.Drawing.Size(353, 591);
            this.trvPermisos.TabIndex = 8;
            this.trvPermisos.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvPermisos_AfterCheck);
            this.trvPermisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvPermisos_AfterSelect);
            // 
            // lblPermisos
            // 
            this.lblPermisos.AutoSize = true;
            this.lblPermisos.Location = new System.Drawing.Point(713, 11);
            this.lblPermisos.Name = "lblPermisos";
            this.lblPermisos.Size = new System.Drawing.Size(149, 13);
            this.lblPermisos.TabIndex = 7;
            this.lblPermisos.Text = "Arbol de permisos en cascada";
            // 
            // lblGrupos
            // 
            this.lblGrupos.AutoSize = true;
            this.lblGrupos.Location = new System.Drawing.Point(3, 6);
            this.lblGrupos.Name = "lblGrupos";
            this.lblGrupos.Size = new System.Drawing.Size(41, 13);
            this.lblGrupos.TabIndex = 0;
            this.lblGrupos.Text = "Grupos";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(418, 29);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(76, 20);
            this.txtNombre.TabIndex = 3;
            // 
            // btnActualizarAsignacion
            // 
            this.btnActualizarAsignacion.Location = new System.Drawing.Point(581, 29);
            this.btnActualizarAsignacion.Name = "btnActualizarAsignacion";
            this.btnActualizarAsignacion.Size = new System.Drawing.Size(75, 23);
            this.btnActualizarAsignacion.TabIndex = 5;
            this.btnActualizarAsignacion.Text = "Actualizar";
            this.btnActualizarAsignacion.UseVisualStyleBackColor = true;
            this.btnActualizarAsignacion.Click += new System.EventHandler(this.btnGuardarAsignacion_Click);
            // 
            // btnEliminarGrupo
            // 
            this.btnEliminarGrupo.Location = new System.Drawing.Point(500, 29);
            this.btnEliminarGrupo.Name = "btnEliminarGrupo";
            this.btnEliminarGrupo.Size = new System.Drawing.Size(75, 23);
            this.btnEliminarGrupo.TabIndex = 4;
            this.btnEliminarGrupo.Text = "Eliminar";
            this.btnEliminarGrupo.UseVisualStyleBackColor = true;
            this.btnEliminarGrupo.Click += new System.EventHandler(this.btnEliminarGrupo_Click);
            // 
            // btnCrearGrupo
            // 
            this.btnCrearGrupo.Location = new System.Drawing.Point(337, 27);
            this.btnCrearGrupo.Name = "btnCrearGrupo";
            this.btnCrearGrupo.Size = new System.Drawing.Size(75, 23);
            this.btnCrearGrupo.TabIndex = 2;
            this.btnCrearGrupo.Text = "Crear";
            this.btnCrearGrupo.UseVisualStyleBackColor = true;
            this.btnCrearGrupo.Click += new System.EventHandler(this.btnCrearGrupo_Click);
            // 
            // lstGrupos
            // 
            this.lstGrupos.FormattingEnabled = true;
            this.lstGrupos.Location = new System.Drawing.Point(6, 27);
            this.lstGrupos.Name = "lstGrupos";
            this.lstGrupos.Size = new System.Drawing.Size(316, 576);
            this.lstGrupos.TabIndex = 1;
            this.lstGrupos.SelectedIndexChanged += new System.EventHandler(this.lstGrupos_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.trvRolesUsuario);
            this.tabPage2.Controls.Add(this.trvRolesAsignados);
            this.tabPage2.Controls.Add(this.lblRolesAsignados);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btnGuardarUsuario);
            this.tabPage2.Controls.Add(this.lstUsuarios);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1072, 626);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Usuarios";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // trvRolesUsuario
            // 
            this.trvRolesUsuario.CheckBoxes = true;
            this.trvRolesUsuario.Location = new System.Drawing.Point(723, 29);
            this.trvRolesUsuario.Name = "trvRolesUsuario";
            this.trvRolesUsuario.Size = new System.Drawing.Size(320, 589);
            this.trvRolesUsuario.TabIndex = 6;
            // 
            // trvRolesAsignados
            // 
            this.trvRolesAsignados.Location = new System.Drawing.Point(330, 60);
            this.trvRolesAsignados.Name = "trvRolesAsignados";
            this.trvRolesAsignados.Size = new System.Drawing.Size(387, 268);
            this.trvRolesAsignados.TabIndex = 5;
            // 
            // lblRolesAsignados
            // 
            this.lblRolesAsignados.AutoSize = true;
            this.lblRolesAsignados.Location = new System.Drawing.Point(327, 44);
            this.lblRolesAsignados.Name = "lblRolesAsignados";
            this.lblRolesAsignados.Size = new System.Drawing.Size(91, 13);
            this.lblRolesAsignados.TabIndex = 4;
            this.lblRolesAsignados.Text = "Roles del usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(428, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Roles asignado al usuario:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuarios";
            // 
            // btnGuardarUsuario
            // 
            this.btnGuardarUsuario.Location = new System.Drawing.Point(332, 6);
            this.btnGuardarUsuario.Name = "btnGuardarUsuario";
            this.btnGuardarUsuario.Size = new System.Drawing.Size(90, 35);
            this.btnGuardarUsuario.TabIndex = 2;
            this.btnGuardarUsuario.Text = "Guardar";
            this.btnGuardarUsuario.UseVisualStyleBackColor = true;
            this.btnGuardarUsuario.Click += new System.EventHandler(this.btnGuardarUsuario_Click);
            // 
            // lstUsuarios
            // 
            this.lstUsuarios.FormattingEnabled = true;
            this.lstUsuarios.Location = new System.Drawing.Point(8, 25);
            this.lstUsuarios.Name = "lstUsuarios";
            this.lstUsuarios.Size = new System.Drawing.Size(313, 589);
            this.lstUsuarios.TabIndex = 1;
            this.lstUsuarios.SelectedIndexChanged += new System.EventHandler(this.lstUsuarios_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 652);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "TurnoSync | ABM Roles";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Shown);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnActualizarAsignacion;
        private System.Windows.Forms.Button btnEliminarGrupo;
        private System.Windows.Forms.Button btnCrearGrupo;
        private System.Windows.Forms.ListBox lstGrupos;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnGuardarUsuario;
        private System.Windows.Forms.ListBox lstUsuarios;
        private System.Windows.Forms.Label lblPermisos;
        private System.Windows.Forms.Label lblGrupos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView trvPermisos;
        private System.Windows.Forms.TreeView trvRolesUsuario;
        private System.Windows.Forms.TreeView trvRolesAsignados;
        private System.Windows.Forms.Label lblRolesAsignados;
        private System.Windows.Forms.TreeView trvGruposJerarquia;
    }
}