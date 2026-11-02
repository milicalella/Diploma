namespace Servicios
{
    partial class FrmReimprimirContrato
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

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.gbContratos = new System.Windows.Forms.GroupBox();
            this.dgvContratos = new System.Windows.Forms.DataGridView();
            this.colNroContrato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFechaEmision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDNICliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropiedadDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipoContrato = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontoAcordado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerarPdf = new System.Windows.Forms.Button();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.gbContratos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(280, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Reimprimir Copia Final";
            // 
            // gbContratos
            // 
            this.gbContratos.Controls.Add(this.dgvContratos);
            this.gbContratos.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbContratos.Location = new System.Drawing.Point(20, 55);
            this.gbContratos.Name = "gbContratos";
            this.gbContratos.Size = new System.Drawing.Size(980, 320);
            this.gbContratos.TabIndex = 1;
            this.gbContratos.TabStop = false;
            this.gbContratos.Text = "Contratos firmados";
            // 
            // dgvContratos
            // 
            this.dgvContratos.AllowUserToAddRows = false;
            this.dgvContratos.AllowUserToDeleteRows = false;
            this.dgvContratos.AutoGenerateColumns = false;
            this.dgvContratos.BackgroundColor = System.Drawing.Color.White;
            this.dgvContratos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContratos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNroContrato,
            this.colFechaEmision,
            this.colDNICliente,
            this.colPropiedadDireccion,
            this.colTipoContrato,
            this.colMontoAcordado,
            this.colEstado});
            this.dgvContratos.Location = new System.Drawing.Point(15, 25);
            this.dgvContratos.MultiSelect = false;
            this.dgvContratos.Name = "dgvContratos";
            this.dgvContratos.ReadOnly = true;
            this.dgvContratos.RowHeadersVisible = false;
            this.dgvContratos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContratos.Size = new System.Drawing.Size(950, 280);
            this.dgvContratos.TabIndex = 0;
            this.dgvContratos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContratos_CellDoubleClick);
            this.dgvContratos.SelectionChanged += new System.EventHandler(this.dgvContratos_SelectionChanged);
            // 
            // colNroContrato
            // 
            this.colNroContrato.DataPropertyName = "NroContrato";
            this.colNroContrato.HeaderText = "Nro. Contrato";
            this.colNroContrato.Name = "colNroContrato";
            this.colNroContrato.ReadOnly = true;
            this.colNroContrato.Width = 130;
            // 
            // colFechaEmision
            // 
            this.colFechaEmision.DataPropertyName = "FechaEmision";
            this.colFechaEmision.DefaultCellStyle.Format = "d";
            this.colFechaEmision.HeaderText = "Emisión";
            this.colFechaEmision.Name = "colFechaEmision";
            this.colFechaEmision.ReadOnly = true;
            this.colFechaEmision.Width = 90;
            // 
            // colDNICliente
            // 
            this.colDNICliente.DataPropertyName = "DNICliente";
            this.colDNICliente.HeaderText = "DNI Cliente";
            this.colDNICliente.Name = "colDNICliente";
            this.colDNICliente.ReadOnly = true;
            this.colDNICliente.Width = 100;
            // 
            // colPropiedadDireccion
            // 
            this.colPropiedadDireccion.DataPropertyName = "PropiedadDireccion";
            this.colPropiedadDireccion.HeaderText = "Dirección Propiedad";
            this.colPropiedadDireccion.Name = "colPropiedadDireccion";
            this.colPropiedadDireccion.ReadOnly = true;
            this.colPropiedadDireccion.Width = 300;
            // 
            // colTipoContrato
            // 
            this.colTipoContrato.DataPropertyName = "TipoContrato";
            this.colTipoContrato.HeaderText = "Tipo Contrato";
            this.colTipoContrato.Name = "colTipoContrato";
            this.colTipoContrato.ReadOnly = true;
            this.colTipoContrato.Width = 100;
            // 
            // colMontoAcordado
            // 
            this.colMontoAcordado.DataPropertyName = "MontoAcordado";
            this.colMontoAcordado.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colMontoAcordado.DefaultCellStyle.Format = "N2";
            this.colMontoAcordado.HeaderText = "Monto";
            this.colMontoAcordado.Name = "colMontoAcordado";
            this.colMontoAcordado.ReadOnly = true;
            this.colMontoAcordado.Width = 110;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "Estado";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 90;
            // 
            // btnGenerarPdf
            // 
            this.btnGenerarPdf.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGenerarPdf.Enabled = false;
            this.btnGenerarPdf.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarPdf.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGenerarPdf.Location = new System.Drawing.Point(810, 395);
            this.btnGenerarPdf.Name = "btnGenerarPdf";
            this.btnGenerarPdf.Size = new System.Drawing.Size(190, 40);
            this.btnGenerarPdf.TabIndex = 2;
            this.btnGenerarPdf.Text = "Generar PDF";
            this.btnGenerarPdf.UseVisualStyleBackColor = false;
            this.btnGenerarPdf.Click += new System.EventHandler(this.btnGenerarPdf_Click);
            // 
            // lblAyuda
            // 
            this.lblAyuda.AutoSize = true;
            this.lblAyuda.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAyuda.ForeColor = System.Drawing.Color.DimGray;
            this.lblAyuda.Location = new System.Drawing.Point(20, 407);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(470, 14);
            this.lblAyuda.TabIndex = 3;
            this.lblAyuda.Text = "Seleccione un contrato firmado y presione Generar PDF (o haga doble clic en la fila).";
            // 
            // FrmReimprimirContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1020, 460);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnGenerarPdf);
            this.Controls.Add(this.gbContratos);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmReimprimirContrato";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reimprimir Copia Final";
            this.gbContratos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbContratos;
        private System.Windows.Forms.DataGridView dgvContratos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNroContrato;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaEmision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDNICliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropiedadDireccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipoContrato;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontoAcordado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.Button btnGenerarPdf;
        private System.Windows.Forms.Label lblAyuda;
    }
}
