namespace UI
{
    partial class frmTurnos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label lblEspecialidades;
            this.Turnos = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnCancelarForm = new System.Windows.Forms.Button();
            this.cbmProfesional = new System.Windows.Forms.ComboBox();
            this.cmbPaciente = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblNombreProfesional = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.dtpFechaTurno = new System.Windows.Forms.DateTimePicker();
            this.dgvTurnos = new System.Windows.Forms.DataGridView();
            lblEspecialidades = new System.Windows.Forms.Label();
            this.Turnos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEspecialidades
            // 
            lblEspecialidades.AutoSize = true;
            lblEspecialidades.Location = new System.Drawing.Point(15, 91);
            lblEspecialidades.Name = "lblEspecialidades";
            lblEspecialidades.Size = new System.Drawing.Size(40, 13);
            lblEspecialidades.TabIndex = 23;
            lblEspecialidades.Text = "Estado";
            // 
            // Turnos
            // 
            this.Turnos.Controls.Add(this.label2);
            this.Turnos.Controls.Add(this.txtObservaciones);
            this.Turnos.Controls.Add(this.btnCancelarForm);
            this.Turnos.Controls.Add(this.cbmProfesional);
            this.Turnos.Controls.Add(this.cmbPaciente);
            this.Turnos.Controls.Add(lblEspecialidades);
            this.Turnos.Controls.Add(this.label1);
            this.Turnos.Controls.Add(this.cmbEstado);
            this.Turnos.Controls.Add(this.btnNuevo);
            this.Turnos.Controls.Add(this.btnGuardar);
            this.Turnos.Controls.Add(this.btnConfirmar);
            this.Turnos.Controls.Add(this.label7);
            this.Turnos.Controls.Add(this.lblNombreProfesional);
            this.Turnos.Controls.Add(this.btnCancelar);
            this.Turnos.Controls.Add(this.dtpFechaTurno);
            this.Turnos.Location = new System.Drawing.Point(12, 12);
            this.Turnos.Name = "Turnos";
            this.Turnos.Size = new System.Drawing.Size(776, 241);
            this.Turnos.TabIndex = 0;
            this.Turnos.TabStop = false;
            this.Turnos.Text = "Turnos";
            this.Turnos.Enter += new System.EventHandler(this.Turnos_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(90, 141);
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(96, 20);
            this.txtObservaciones.TabIndex = 5;
            this.txtObservaciones.TextChanged += new System.EventHandler(this.txtObservaciones_TextChanged);
            // 
            // btnCancelarForm
            // 
            this.btnCancelarForm.Location = new System.Drawing.Point(122, 177);
            this.btnCancelarForm.Name = "btnCancelarForm";
            this.btnCancelarForm.Size = new System.Drawing.Size(115, 23);
            this.btnCancelarForm.TabIndex = 7;
            this.btnCancelarForm.Text = "Cancelar Form";
            this.btnCancelarForm.UseVisualStyleBackColor = true;
            this.btnCancelarForm.Click += new System.EventHandler(this.btnCancelarForm_Click);
            // 
            // cbmProfesional
            // 
            this.cbmProfesional.FormattingEnabled = true;
            this.cbmProfesional.Location = new System.Drawing.Point(90, 55);
            this.cbmProfesional.Name = "cbmProfesional";
            this.cbmProfesional.Size = new System.Drawing.Size(96, 21);
            this.cbmProfesional.TabIndex = 2;
            this.cbmProfesional.SelectedIndexChanged += new System.EventHandler(this.cbmProfesional_SelectedIndexChanged);
            // 
            // cmbPaciente
            // 
            this.cmbPaciente.FormattingEnabled = true;
            this.cmbPaciente.Location = new System.Drawing.Point(90, 28);
            this.cmbPaciente.Name = "cmbPaciente";
            this.cmbPaciente.Size = new System.Drawing.Size(96, 21);
            this.cmbPaciente.TabIndex = 1;
            this.cmbPaciente.SelectedIndexChanged += new System.EventHandler(this.cmbPaciente_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Paciente";
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(90, 82);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(96, 21);
            this.cmbEstado.TabIndex = 3;
            this.cmbEstado.SelectedIndexChanged += new System.EventHandler(this.cmbEstado_SelectedIndexChanged);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(9, 177);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(107, 23);
            this.btnNuevo.TabIndex = 6;
            this.btnNuevo.Text = "Crear";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(203, 31);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(107, 23);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Crear turno";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Location = new System.Drawing.Point(203, 62);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(107, 23);
            this.btnConfirmar.TabIndex = 9;
            this.btnConfirmar.Text = "Confirmar Turno";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Fecha de turno";
            // 
            // lblNombreProfesional
            // 
            this.lblNombreProfesional.AutoSize = true;
            this.lblNombreProfesional.Location = new System.Drawing.Point(14, 60);
            this.lblNombreProfesional.Name = "lblNombreProfesional";
            this.lblNombreProfesional.Size = new System.Drawing.Size(59, 13);
            this.lblNombreProfesional.TabIndex = 10;
            this.lblNombreProfesional.Text = "Profesional";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(203, 91);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(107, 23);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "Cancelar Turno";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // dtpFechaTurno
            // 
            this.dtpFechaTurno.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaTurno.Location = new System.Drawing.Point(90, 112);
            this.dtpFechaTurno.Name = "dtpFechaTurno";
            this.dtpFechaTurno.Size = new System.Drawing.Size(96, 20);
            this.dtpFechaTurno.TabIndex = 4;
            this.dtpFechaTurno.ValueChanged += new System.EventHandler(this.dtpFechaTurno_ValueChanged);
            // 
            // dgvTurnos
            // 
            this.dgvTurnos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTurnos.Location = new System.Drawing.Point(10, 259);
            this.dgvTurnos.Name = "dgvTurnos";
            this.dgvTurnos.Size = new System.Drawing.Size(778, 179);
            this.dgvTurnos.TabIndex = 1;
            this.dgvTurnos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTurnos_CellContentClick);
            // 
            // frmTurnos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvTurnos);
            this.Controls.Add(this.Turnos);
            this.Name = "frmTurnos";
            this.Text = "TurnoSync | Turnos";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTurnos_FormClosed);
            this.Load += new System.EventHandler(this.frmTurnos_Load);
            this.Turnos.ResumeLayout(false);
            this.Turnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTurnos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Turnos;
        private System.Windows.Forms.ComboBox cbmProfesional;
        private System.Windows.Forms.ComboBox cmbPaciente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblNombreProfesional;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.DateTimePicker dtpFechaTurno;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnCancelarForm;
        private System.Windows.Forms.DataGridView dgvTurnos;
    }
}