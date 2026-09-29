namespace Servicios
{
    partial class ActualizarEstadoVisita
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
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.cboFiltroEstado = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvVisitas = new System.Windows.Forms.DataGridView();
            this.colIdVisita = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDNI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRango = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbActualizar = new System.Windows.Forms.GroupBox();
            this.lblEstadoNuevo = new System.Windows.Forms.Label();
            this.cboNuevoEstado = new System.Windows.Forms.ComboBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitas)).BeginInit();
            this.gbActualizar.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFiltroEstado
            // 
            this.lblFiltroEstado.AutoSize = true;
            this.lblFiltroEstado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblFiltroEstado.Location = new System.Drawing.Point(32, 22);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(124, 17);
            this.lblFiltroEstado.TabIndex = 0;
            this.lblFiltroEstado.Text = "Filtrar estado";
            // 
            // cboFiltroEstado
            // 
            this.cboFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroEstado.FormattingEnabled = true;
            this.cboFiltroEstado.Items.AddRange(new object[] {
            "Todos",
            "Pendiente",
            "Realizada",
            "Cancelada"});
            this.cboFiltroEstado.Location = new System.Drawing.Point(180, 18);
            this.cboFiltroEstado.Name = "cboFiltroEstado";
            this.cboFiltroEstado.Size = new System.Drawing.Size(140, 21);
            this.cboFiltroEstado.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnBuscar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnBuscar.Location = new System.Drawing.Point(360, 12);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 35);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLimpiar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnLimpiar.Location = new System.Drawing.Point(490, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 35);
            this.btnLimpiar.TabIndex = 3;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // dgvVisitas
            // 
            this.dgvVisitas.AllowUserToAddRows = false;
            this.dgvVisitas.AllowUserToDeleteRows = false;
            this.dgvVisitas.AutoGenerateColumns = false;
            this.dgvVisitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVisitas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIdVisita,
            this.colDireccion,
            this.colDNI,
            this.colFecha,
            this.colRango,
            this.colEstado});
            this.dgvVisitas.Location = new System.Drawing.Point(32, 62);
            this.dgvVisitas.MultiSelect = false;
            this.dgvVisitas.Name = "dgvVisitas";
            this.dgvVisitas.ReadOnly = true;
            this.dgvVisitas.RowHeadersVisible = false;
            this.dgvVisitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVisitas.Size = new System.Drawing.Size(780, 300);
            this.dgvVisitas.TabIndex = 4;
            this.dgvVisitas.SelectionChanged += new System.EventHandler(this.dgvVisitas_SelectionChanged);
            // 
            // colIdVisita
            // 
            this.colIdVisita.DataPropertyName = "IdVisita";
            this.colIdVisita.HeaderText = "Id";
            this.colIdVisita.Name = "colIdVisita";
            this.colIdVisita.ReadOnly = true;
            this.colIdVisita.Width = 55;
            // 
            // colDireccion
            // 
            this.colDireccion.DataPropertyName = "PropiedadDireccion";
            this.colDireccion.HeaderText = "Dirección";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.ReadOnly = true;
            this.colDireccion.Width = 280;
            // 
            // colDNI
            // 
            this.colDNI.DataPropertyName = "DNICliente";
            this.colDNI.HeaderText = "DNI Cliente";
            this.colDNI.Name = "colDNI";
            this.colDNI.ReadOnly = true;
            this.colDNI.Width = 90;
            // 
            // colFecha
            // 
            this.colFecha.DataPropertyName = "Fecha";
            this.colFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
            this.colFecha.HeaderText = "Fecha";
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            this.colFecha.Width = 95;
            // 
            // colRango
            // 
            this.colRango.DataPropertyName = "RangoHorario";
            this.colRango.HeaderText = "Horario";
            this.colRango.Name = "colRango";
            this.colRango.ReadOnly = true;
            this.colRango.Width = 110;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "Estado";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 100;
            // 
            // gbActualizar
            // 
            this.gbActualizar.Controls.Add(this.lblEstadoNuevo);
            this.gbActualizar.Controls.Add(this.cboNuevoEstado);
            this.gbActualizar.Controls.Add(this.lblObservaciones);
            this.gbActualizar.Controls.Add(this.txtObservaciones);
            this.gbActualizar.Controls.Add(this.btnGuardar);
            this.gbActualizar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.gbActualizar.Location = new System.Drawing.Point(32, 385);
            this.gbActualizar.Name = "gbActualizar";
            this.gbActualizar.Size = new System.Drawing.Size(780, 150);
            this.gbActualizar.TabIndex = 5;
            this.gbActualizar.TabStop = false;
            this.gbActualizar.Text = "Actualizar estado de la visita";
            // 
            // lblEstadoNuevo
            // 
            this.lblEstadoNuevo.AutoSize = true;
            this.lblEstadoNuevo.Location = new System.Drawing.Point(20, 30);
            this.lblEstadoNuevo.Name = "lblEstadoNuevo";
            this.lblEstadoNuevo.Size = new System.Drawing.Size(62, 17);
            this.lblEstadoNuevo.TabIndex = 0;
            this.lblEstadoNuevo.Text = "Estado";
            // 
            // cboNuevoEstado
            // 
            this.cboNuevoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNuevoEstado.FormattingEnabled = true;
            this.cboNuevoEstado.Items.AddRange(new object[] {
            "Realizada",
            "Cancelada"});
            this.cboNuevoEstado.Location = new System.Drawing.Point(160, 26);
            this.cboNuevoEstado.Name = "cboNuevoEstado";
            this.cboNuevoEstado.Size = new System.Drawing.Size(140, 24);
            this.cboNuevoEstado.TabIndex = 1;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(20, 80);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(132, 17);
            this.lblObservaciones.TabIndex = 2;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(160, 60);
            this.txtObservaciones.MaxLength = 500;
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservaciones.Size = new System.Drawing.Size(430, 60);
            this.txtObservaciones.TabIndex = 3;
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGuardar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGuardar.Location = new System.Drawing.Point(620, 70);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(140, 40);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // ActualizarEstadoVisita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(850, 560);
            this.Controls.Add(this.gbActualizar);
            this.Controls.Add(this.dgvVisitas);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.cboFiltroEstado);
            this.Controls.Add(this.lblFiltroEstado);
            this.Name = "ActualizarEstadoVisita";
            this.Text = "Actualizar Estado de Visita";
            this.Load += new System.EventHandler(this.ActualizarEstadoVisita_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitas)).EndInit();
            this.gbActualizar.ResumeLayout(false);
            this.gbActualizar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFiltroEstado;
        private System.Windows.Forms.ComboBox cboFiltroEstado;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.DataGridView dgvVisitas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdVisita;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDNI;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRango;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.GroupBox gbActualizar;
        private System.Windows.Forms.Label lblEstadoNuevo;
        private System.Windows.Forms.ComboBox cboNuevoEstado;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Button btnGuardar;
    }
}