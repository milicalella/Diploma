namespace Servicios
{
    partial class FrmConsultarDisponibilidad
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.gbPropiedad = new System.Windows.Forms.GroupBox();
            this.lblPropiedadInfo = new System.Windows.Forms.Label();
            this.gbHorario = new System.Windows.Forms.GroupBox();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.dtpHoraFin = new System.Windows.Forms.DateTimePicker();
            this.lblFin = new System.Windows.Forms.Label();
            this.dtpHoraInicio = new System.Windows.Forms.DateTimePicker();
            this.lblInicio = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblTituloVisitas = new System.Windows.Forms.Label();
            this.dgvVisitas = new System.Windows.Forms.DataGridView();
            this.colIdVisita = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHorario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbPropiedad.SuspendLayout();
            this.gbHorario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitas)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(24, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(232, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Consultar Disponibilidad";
            // 
            // gbPropiedad
            // 
            this.gbPropiedad.Controls.Add(this.lblPropiedadInfo);
            this.gbPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gbPropiedad.Location = new System.Drawing.Point(24, 50);
            this.gbPropiedad.Name = "gbPropiedad";
            this.gbPropiedad.Size = new System.Drawing.Size(560, 100);
            this.gbPropiedad.TabIndex = 1;
            this.gbPropiedad.TabStop = false;
            this.gbPropiedad.Text = "Propiedad";
            // 
            // lblPropiedadInfo
            // 
            this.lblPropiedadInfo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPropiedadInfo.Location = new System.Drawing.Point(20, 30);
            this.lblPropiedadInfo.Name = "lblPropiedadInfo";
            this.lblPropiedadInfo.Size = new System.Drawing.Size(520, 50);
            this.lblPropiedadInfo.TabIndex = 0;
            this.lblPropiedadInfo.Text = "-";
            // 
            // gbHorario
            // 
            this.gbHorario.Controls.Add(this.lblResultado);
            this.gbHorario.Controls.Add(this.btnConsultar);
            this.gbHorario.Controls.Add(this.dtpHoraFin);
            this.gbHorario.Controls.Add(this.lblFin);
            this.gbHorario.Controls.Add(this.dtpHoraInicio);
            this.gbHorario.Controls.Add(this.lblInicio);
            this.gbHorario.Controls.Add(this.dtpFecha);
            this.gbHorario.Controls.Add(this.lblFecha);
            this.gbHorario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gbHorario.Location = new System.Drawing.Point(24, 160);
            this.gbHorario.Name = "gbHorario";
            this.gbHorario.Size = new System.Drawing.Size(560, 180);
            this.gbHorario.TabIndex = 2;
            this.gbHorario.TabStop = false;
            this.gbHorario.Text = "Fecha y Horario";
            // 
            // lblResultado
            // 
            this.lblResultado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(20, 110);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(520, 44);
            this.lblResultado.TabIndex = 7;
            this.lblResultado.Text = "-";
            // 
            // btnConsultar
            // 
            this.btnConsultar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnConsultar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnConsultar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnConsultar.Location = new System.Drawing.Point(340, 58);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(200, 34);
            this.btnConsultar.TabIndex = 6;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = false;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            // 
            // dtpHoraFin
            // 
            this.dtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraFin.Location = new System.Drawing.Point(100, 63);
            this.dtpHoraFin.Name = "dtpHoraFin";
            this.dtpHoraFin.ShowUpDown = true;
            this.dtpHoraFin.Size = new System.Drawing.Size(110, 22);
            this.dtpHoraFin.TabIndex = 5;
            // 
            // lblFin
            // 
            this.lblFin.AutoSize = true;
            this.lblFin.Location = new System.Drawing.Point(20, 68);
            this.lblFin.Name = "lblFin";
            this.lblFin.Size = new System.Drawing.Size(60, 14);
            this.lblFin.TabIndex = 4;
            this.lblFin.Text = "Hora fin";
            // 
            // dtpHoraInicio
            // 
            this.dtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraInicio.Location = new System.Drawing.Point(340, 25);
            this.dtpHoraInicio.Name = "dtpHoraInicio";
            this.dtpHoraInicio.ShowUpDown = true;
            this.dtpHoraInicio.Size = new System.Drawing.Size(110, 22);
            this.dtpHoraInicio.TabIndex = 3;
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.Location = new System.Drawing.Point(250, 30);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(98, 14);
            this.lblInicio.TabIndex = 2;
            this.lblInicio.Text = "Hora de inicio";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(100, 25);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(120, 22);
            this.dtpFecha.TabIndex = 1;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(20, 30);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(46, 14);
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Text = "Fecha";
            // 
            // lblTituloVisitas
            // 
            this.lblTituloVisitas.AutoSize = true;
            this.lblTituloVisitas.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.lblTituloVisitas.Location = new System.Drawing.Point(24, 354);
            this.lblTituloVisitas.Name = "lblTituloVisitas";
            this.lblTituloVisitas.Size = new System.Drawing.Size(210, 14);
            this.lblTituloVisitas.TabIndex = 3;
            this.lblTituloVisitas.Text = "Visitas registradas de la fecha:";
            // 
            // dgvVisitas
            // 
            this.dgvVisitas.AllowUserToAddRows = false;
            this.dgvVisitas.AllowUserToDeleteRows = false;
            this.dgvVisitas.AutoGenerateColumns = false;
            this.dgvVisitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVisitas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdVisita,
            this.colHorario,
            this.colEstado});
            this.dgvVisitas.Location = new System.Drawing.Point(24, 374);
            this.dgvVisitas.MultiSelect = false;
            this.dgvVisitas.Name = "dgvVisitas";
            this.dgvVisitas.RowHeadersVisible = false;
            this.dgvVisitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVisitas.Size = new System.Drawing.Size(560, 140);
            this.dgvVisitas.TabIndex = 4;
            this.dgvVisitas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvVisitas_CellFormatting);
            // 
            // colIdVisita
            // 
            this.colIdVisita.DataPropertyName = "IdVisita";
            this.colIdVisita.HeaderText = "Id";
            this.colIdVisita.Name = "colIdVisita";
            this.colIdVisita.ReadOnly = true;
            this.colIdVisita.Width = 60;
            // 
            // colHorario
            // 
            this.colHorario.DataPropertyName = "RangoHorario";
            this.colHorario.HeaderText = "Horario";
            this.colHorario.Name = "colHorario";
            this.colHorario.ReadOnly = true;
            this.colHorario.Width = 110;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "Estado";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 130;
            // 
            // btnAceptar
            // 
            this.btnAceptar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAceptar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnAceptar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnAceptar.Location = new System.Drawing.Point(24, 526);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(120, 40);
            this.btnAceptar.TabIndex = 5;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCancelar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnCancelar.Location = new System.Drawing.Point(160, 526);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 40);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmConsultarDisponibilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(620, 588);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.dgvVisitas);
            this.Controls.Add(this.lblTituloVisitas);
            this.Controls.Add(this.gbHorario);
            this.Controls.Add(this.gbPropiedad);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmConsultarDisponibilidad";
            this.Text = "Consultar Disponibilidad";
            this.Load += new System.EventHandler(this.FrmConsultarDisponibilidad_Load);
            this.gbPropiedad.ResumeLayout(false);
            this.gbHorario.ResumeLayout(false);
            this.gbHorario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbPropiedad;
        private System.Windows.Forms.Label lblPropiedadInfo;
        private System.Windows.Forms.GroupBox gbHorario;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.DateTimePicker dtpHoraFin;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.DateTimePicker dtpHoraInicio;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblTituloVisitas;
        private System.Windows.Forms.DataGridView dgvVisitas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdVisita;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHorario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}