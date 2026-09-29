namespace Servicios
{
    partial class SeleccionarPropiedad
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
            this.dgvPropiedades = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboTipo = new System.Windows.Forms.ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cboEstado = new System.Windows.Forms.ComboBox();
            this.lblPrecioMin = new System.Windows.Forms.Label();
            this.txtPrecioMin = new System.Windows.Forms.TextBox();
            this.lblPrecioMax = new System.Windows.Forms.Label();
            this.txtPrecioMax = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnSeleccionar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPropiedades)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPropiedades
            // 
            this.dgvPropiedades.AllowUserToAddRows = false;
            this.dgvPropiedades.AllowUserToDeleteRows = false;
            this.dgvPropiedades.AutoGenerateColumns = false;
            this.dgvPropiedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPropiedades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colDireccion,
            this.colTipo,
            this.colEstado,
            this.colPrecio});
            this.dgvPropiedades.Location = new System.Drawing.Point(32, 140);
            this.dgvPropiedades.MultiSelect = false;
            this.dgvPropiedades.Name = "dgvPropiedades";
            this.dgvPropiedades.ReadOnly = true;
            this.dgvPropiedades.RowHeadersVisible = false;
            this.dgvPropiedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPropiedades.Size = new System.Drawing.Size(740, 280);
            this.dgvPropiedades.TabIndex = 10;
            this.dgvPropiedades.DoubleClick += new System.EventHandler(this.dgvPropiedades_DoubleClick);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Código";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 70;
            // 
            // colDireccion
            // 
            this.colDireccion.DataPropertyName = "Direccion";
            this.colDireccion.HeaderText = "Dirección";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.ReadOnly = true;
            this.colDireccion.Width = 300;
            // 
            // colTipo
            // 
            this.colTipo.DataPropertyName = "Tipo";
            this.colTipo.HeaderText = "Tipo";
            this.colTipo.Name = "colTipo";
            this.colTipo.ReadOnly = true;
            this.colTipo.Width = 120;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "Estado";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 100;
            // 
            // colPrecio
            // 
            this.colPrecio.DataPropertyName = "Precio";
            this.colPrecio.DefaultCellStyle.Format = "N2";
            this.colPrecio.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPrecio.HeaderText = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.ReadOnly = true;
            this.colPrecio.Width = 120;
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblDireccion.Location = new System.Drawing.Point(32, 22);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(85, 17);
            this.lblDireccion.TabIndex = 0;
            this.lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(150, 18);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(250, 20);
            this.txtDireccion.TabIndex = 1;
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblTipo.Location = new System.Drawing.Point(440, 22);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(43, 17);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo";
            // 
            // cboTipo
            // 
            this.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipo.FormattingEnabled = true;
            this.cboTipo.Items.AddRange(new object[] {
            "Casa",
            "Departamento",
            "Local",
            "Terreno"});
            this.cboTipo.Location = new System.Drawing.Point(500, 18);
            this.cboTipo.Name = "cboTipo";
            this.cboTipo.Size = new System.Drawing.Size(140, 21);
            this.cboTipo.TabIndex = 3;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(32, 62);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(65, 17);
            this.lblEstado.TabIndex = 4;
            this.lblEstado.Text = "Estado";
            // 
            // cboEstado
            // 
            this.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstado.FormattingEnabled = true;
            this.cboEstado.Items.AddRange(new object[] {
            "Disponible",
            "Vendida",
            "Alquilada",
            "Reservada"});
            this.cboEstado.Location = new System.Drawing.Point(150, 58);
            this.cboEstado.Name = "cboEstado";
            this.cboEstado.Size = new System.Drawing.Size(140, 21);
            this.cboEstado.TabIndex = 5;
            // 
            // lblPrecioMin
            // 
            this.lblPrecioMin.AutoSize = true;
            this.lblPrecioMin.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecioMin.Location = new System.Drawing.Point(32, 102);
            this.lblPrecioMin.Name = "lblPrecioMin";
            this.lblPrecioMin.Size = new System.Drawing.Size(95, 17);
            this.lblPrecioMin.TabIndex = 6;
            this.lblPrecioMin.Text = "Precio mín.";
            // 
            // txtPrecioMin
            // 
            this.txtPrecioMin.Location = new System.Drawing.Point(150, 98);
            this.txtPrecioMin.Name = "txtPrecioMin";
            this.txtPrecioMin.Size = new System.Drawing.Size(120, 20);
            this.txtPrecioMin.TabIndex = 7;
            // 
            // lblPrecioMax
            // 
            this.lblPrecioMax.AutoSize = true;
            this.lblPrecioMax.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecioMax.Location = new System.Drawing.Point(310, 102);
            this.lblPrecioMax.Name = "lblPrecioMax";
            this.lblPrecioMax.Size = new System.Drawing.Size(99, 17);
            this.lblPrecioMax.TabIndex = 8;
            this.lblPrecioMax.Text = "Precio máx.";
            // 
            // txtPrecioMax
            // 
            this.txtPrecioMax.Location = new System.Drawing.Point(440, 98);
            this.txtPrecioMax.Name = "txtPrecioMax";
            this.txtPrecioMax.Size = new System.Drawing.Size(120, 20);
            this.txtPrecioMax.TabIndex = 9;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnBuscar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnBuscar.Location = new System.Drawing.Point(610, 55);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 35);
            this.btnBuscar.TabIndex = 11;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLimpiar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnLimpiar.Location = new System.Drawing.Point(610, 98);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 35);
            this.btnLimpiar.TabIndex = 12;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.btnSeleccionar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSeleccionar.Location = new System.Drawing.Point(630, 440);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(140, 40);
            this.btnSeleccionar.TabIndex = 13;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = false;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(32, 432);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(20, 17);
            this.lblResultado.TabIndex = 14;
            this.lblResultado.Text = "-";
            // 
            // SeleccionarPropiedad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(810, 505);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnSeleccionar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtPrecioMax);
            this.Controls.Add(this.lblPrecioMax);
            this.Controls.Add(this.txtPrecioMin);
            this.Controls.Add(this.lblPrecioMin);
            this.Controls.Add(this.cboEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cboTipo);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.lblDireccion);
            this.Controls.Add(this.dgvPropiedades);
            this.Name = "SeleccionarPropiedad";
            this.Text = "Seleccionar Propiedad";
            this.Load += new System.EventHandler(this.SeleccionarPropiedad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPropiedades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPropiedades;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cboTipo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cboEstado;
        private System.Windows.Forms.Label lblPrecioMin;
        private System.Windows.Forms.TextBox txtPrecioMin;
        private System.Windows.Forms.Label lblPrecioMax;
        private System.Windows.Forms.TextBox txtPrecioMax;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnSeleccionar;
        private System.Windows.Forms.Label lblResultado;
    }
}