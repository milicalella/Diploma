namespace Servicios
{
    partial class FrmActualizarEstadoPropiedadContrato
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
            this.colFechaInicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFechaFin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMontoAcordado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbDetalle = new System.Windows.Forms.GroupBox();
            this.txtDepositoGarantia = new System.Windows.Forms.TextBox();
            this.lblDepositoGarantia = new System.Windows.Forms.Label();
            this.txtFormaPago = new System.Windows.Forms.TextBox();
            this.lblFormaPago = new System.Windows.Forms.Label();
            this.txtMontoAcordado = new System.Windows.Forms.TextBox();
            this.lblMontoAcordado = new System.Windows.Forms.Label();
            this.txtFechaFin = new System.Windows.Forms.TextBox();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.txtFechaInicio = new System.Windows.Forms.TextBox();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.txtTipoContrato = new System.Windows.Forms.TextBox();
            this.lblTipoContrato = new System.Windows.Forms.Label();
            this.txtPropiedadDireccion = new System.Windows.Forms.TextBox();
            this.lblPropiedadDireccion = new System.Windows.Forms.Label();
            this.txtCodigoPropiedad = new System.Windows.Forms.TextBox();
            this.lblCodigoPropiedad = new System.Windows.Forms.Label();
            this.txtDNICliente = new System.Windows.Forms.TextBox();
            this.lblDNICliente = new System.Windows.Forms.Label();
            this.txtDNIPropietario = new System.Windows.Forms.TextBox();
            this.lblDNIPropietario = new System.Windows.Forms.Label();
            this.txtEstadoActual = new System.Windows.Forms.TextBox();
            this.lblEstadoActual = new System.Windows.Forms.Label();
            this.txtFechaEmision = new System.Windows.Forms.TextBox();
            this.lblFechaEmision = new System.Windows.Forms.Label();
            this.txtNroContrato = new System.Windows.Forms.TextBox();
            this.lblNroContrato = new System.Windows.Forms.Label();
            this.gbDecision = new System.Windows.Forms.GroupBox();
            this.cboNuevoEstado = new System.Windows.Forms.ComboBox();
            this.lblNuevoEstado = new System.Windows.Forms.Label();
            this.btnModificarEstado = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.gbContratos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).BeginInit();
            this.gbDetalle.SuspendLayout();
            this.gbDecision.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(360, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Actualizar Estado de Propiedad y Contrato";
            // 
            // gbContratos
            // 
            this.gbContratos.Controls.Add(this.dgvContratos);
            this.gbContratos.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbContratos.Location = new System.Drawing.Point(20, 55);
            this.gbContratos.Name = "gbContratos";
            this.gbContratos.Size = new System.Drawing.Size(980, 230);
            this.gbContratos.TabIndex = 1;
            this.gbContratos.TabStop = false;
            this.gbContratos.Text = "Contratos pendientes";
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
            this.colFechaInicio,
            this.colFechaFin,
            this.colMontoAcordado,
            this.colEstado});
            this.dgvContratos.Location = new System.Drawing.Point(15, 25);
            this.dgvContratos.MultiSelect = false;
            this.dgvContratos.Name = "dgvContratos";
            this.dgvContratos.ReadOnly = true;
            this.dgvContratos.RowHeadersVisible = false;
            this.dgvContratos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContratos.Size = new System.Drawing.Size(950, 190);
            this.dgvContratos.TabIndex = 0;
            this.dgvContratos.SelectionChanged += new System.EventHandler(this.dgvContratos_SelectionChanged);
            // 
            // colNroContrato
            // 
            this.colNroContrato.DataPropertyName = "NroContrato";
            this.colNroContrato.HeaderText = "Nro. Contrato";
            this.colNroContrato.Name = "colNroContrato";
            this.colNroContrato.ReadOnly = true;
            this.colNroContrato.Width = 110;
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
            this.colPropiedadDireccion.Width = 220;
            // 
            // colTipoContrato
            // 
            this.colTipoContrato.DataPropertyName = "TipoContrato";
            this.colTipoContrato.HeaderText = "Tipo Contrato";
            this.colTipoContrato.Name = "colTipoContrato";
            this.colTipoContrato.ReadOnly = true;
            this.colTipoContrato.Width = 90;
            // 
            // colFechaInicio
            // 
            this.colFechaInicio.DataPropertyName = "FechaInicio";
            this.colFechaInicio.DefaultCellStyle.Format = "d";
            this.colFechaInicio.HeaderText = "Inicio";
            this.colFechaInicio.Name = "colFechaInicio";
            this.colFechaInicio.ReadOnly = true;
            this.colFechaInicio.Width = 90;
            // 
            // colFechaFin
            // 
            this.colFechaFin.DataPropertyName = "FechaFin";
            this.colFechaFin.DefaultCellStyle.Format = "d";
            this.colFechaFin.HeaderText = "Fin";
            this.colFechaFin.Name = "colFechaFin";
            this.colFechaFin.ReadOnly = true;
            this.colFechaFin.Width = 90;
            // 
            // colMontoAcordado
            // 
            this.colMontoAcordado.DataPropertyName = "MontoAcordado";
            this.colMontoAcordado.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colMontoAcordado.DefaultCellStyle.Format = "N2";
            this.colMontoAcordado.HeaderText = "Monto";
            this.colMontoAcordado.Name = "colMontoAcordado";
            this.colMontoAcordado.ReadOnly = true;
            this.colMontoAcordado.Width = 90;
            // 
            // colEstado
            // 
            this.colEstado.DataPropertyName = "Estado";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 70;
            // 
            // gbDetalle
            // 
            this.gbDetalle.Controls.Add(this.txtDepositoGarantia);
            this.gbDetalle.Controls.Add(this.lblDepositoGarantia);
            this.gbDetalle.Controls.Add(this.txtFormaPago);
            this.gbDetalle.Controls.Add(this.lblFormaPago);
            this.gbDetalle.Controls.Add(this.txtMontoAcordado);
            this.gbDetalle.Controls.Add(this.lblMontoAcordado);
            this.gbDetalle.Controls.Add(this.txtFechaFin);
            this.gbDetalle.Controls.Add(this.lblFechaFin);
            this.gbDetalle.Controls.Add(this.txtFechaInicio);
            this.gbDetalle.Controls.Add(this.lblFechaInicio);
            this.gbDetalle.Controls.Add(this.txtTipoContrato);
            this.gbDetalle.Controls.Add(this.lblTipoContrato);
            this.gbDetalle.Controls.Add(this.txtPropiedadDireccion);
            this.gbDetalle.Controls.Add(this.lblPropiedadDireccion);
            this.gbDetalle.Controls.Add(this.txtCodigoPropiedad);
            this.gbDetalle.Controls.Add(this.lblCodigoPropiedad);
            this.gbDetalle.Controls.Add(this.txtDNICliente);
            this.gbDetalle.Controls.Add(this.lblDNICliente);
            this.gbDetalle.Controls.Add(this.txtDNIPropietario);
            this.gbDetalle.Controls.Add(this.lblDNIPropietario);
            this.gbDetalle.Controls.Add(this.txtEstadoActual);
            this.gbDetalle.Controls.Add(this.lblEstadoActual);
            this.gbDetalle.Controls.Add(this.txtFechaEmision);
            this.gbDetalle.Controls.Add(this.lblFechaEmision);
            this.gbDetalle.Controls.Add(this.txtNroContrato);
            this.gbDetalle.Controls.Add(this.lblNroContrato);
            this.gbDetalle.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDetalle.Location = new System.Drawing.Point(20, 295);
            this.gbDetalle.Name = "gbDetalle";
            this.gbDetalle.Size = new System.Drawing.Size(980, 215);
            this.gbDetalle.TabIndex = 2;
            this.gbDetalle.TabStop = false;
            this.gbDetalle.Text = "Contrato seleccionado";
            // 
            // txtNroContrato
            // 
            this.txtNroContrato.BackColor = System.Drawing.Color.White;
            this.txtNroContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroContrato.Location = new System.Drawing.Point(150, 28);
            this.txtNroContrato.Name = "txtNroContrato";
            this.txtNroContrato.ReadOnly = true;
            this.txtNroContrato.Size = new System.Drawing.Size(130, 22);
            this.txtNroContrato.TabIndex = 0;
            this.txtNroContrato.TabStop = false;
            // 
            // lblNroContrato
            // 
            this.lblNroContrato.AutoSize = true;
            this.lblNroContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroContrato.Location = new System.Drawing.Point(20, 31);
            this.lblNroContrato.Name = "lblNroContrato";
            this.lblNroContrato.Size = new System.Drawing.Size(94, 14);
            this.lblNroContrato.TabIndex = 1;
            this.lblNroContrato.Text = "Nro. Contrato:";
            // 
            // txtFechaEmision
            // 
            this.txtFechaEmision.BackColor = System.Drawing.Color.White;
            this.txtFechaEmision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaEmision.Location = new System.Drawing.Point(430, 28);
            this.txtFechaEmision.Name = "txtFechaEmision";
            this.txtFechaEmision.ReadOnly = true;
            this.txtFechaEmision.Size = new System.Drawing.Size(100, 22);
            this.txtFechaEmision.TabIndex = 2;
            this.txtFechaEmision.TabStop = false;
            // 
            // lblFechaEmision
            // 
            this.lblFechaEmision.AutoSize = true;
            this.lblFechaEmision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaEmision.Location = new System.Drawing.Point(320, 31);
            this.lblFechaEmision.Name = "lblFechaEmision";
            this.lblFechaEmision.Size = new System.Drawing.Size(101, 14);
            this.lblFechaEmision.TabIndex = 3;
            this.lblFechaEmision.Text = "Fecha Emisión:";
            // 
            // txtEstadoActual
            // 
            this.txtEstadoActual.BackColor = System.Drawing.Color.White;
            this.txtEstadoActual.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstadoActual.Location = new System.Drawing.Point(700, 28);
            this.txtEstadoActual.Name = "txtEstadoActual";
            this.txtEstadoActual.ReadOnly = true;
            this.txtEstadoActual.Size = new System.Drawing.Size(110, 22);
            this.txtEstadoActual.TabIndex = 4;
            this.txtEstadoActual.TabStop = false;
            // 
            // lblEstadoActual
            // 
            this.lblEstadoActual.AutoSize = true;
            this.lblEstadoActual.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoActual.Location = new System.Drawing.Point(580, 31);
            this.lblEstadoActual.Name = "lblEstadoActual";
            this.lblEstadoActual.Size = new System.Drawing.Size(96, 14);
            this.lblEstadoActual.TabIndex = 5;
            this.lblEstadoActual.Text = "Estado actual:";
            // 
            // txtDNIPropietario
            // 
            this.txtDNIPropietario.BackColor = System.Drawing.Color.White;
            this.txtDNIPropietario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNIPropietario.Location = new System.Drawing.Point(150, 63);
            this.txtDNIPropietario.Name = "txtDNIPropietario";
            this.txtDNIPropietario.ReadOnly = true;
            this.txtDNIPropietario.Size = new System.Drawing.Size(130, 22);
            this.txtDNIPropietario.TabIndex = 6;
            this.txtDNIPropietario.TabStop = false;
            // 
            // lblDNIPropietario
            // 
            this.lblDNIPropietario.AutoSize = true;
            this.lblDNIPropietario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNIPropietario.Location = new System.Drawing.Point(20, 66);
            this.lblDNIPropietario.Name = "lblDNIPropietario";
            this.lblDNIPropietario.Size = new System.Drawing.Size(111, 14);
            this.lblDNIPropietario.TabIndex = 7;
            this.lblDNIPropietario.Text = "DNI Propietario:";
            // 
            // txtDNICliente
            // 
            this.txtDNICliente.BackColor = System.Drawing.Color.White;
            this.txtDNICliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNICliente.Location = new System.Drawing.Point(430, 63);
            this.txtDNICliente.Name = "txtDNICliente";
            this.txtDNICliente.ReadOnly = true;
            this.txtDNICliente.Size = new System.Drawing.Size(130, 22);
            this.txtDNICliente.TabIndex = 8;
            this.txtDNICliente.TabStop = false;
            // 
            // lblDNICliente
            // 
            this.lblDNICliente.AutoSize = true;
            this.lblDNICliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNICliente.Location = new System.Drawing.Point(320, 66);
            this.lblDNICliente.Name = "lblDNICliente";
            this.lblDNICliente.Size = new System.Drawing.Size(84, 14);
            this.lblDNICliente.TabIndex = 9;
            this.lblDNICliente.Text = "DNI Cliente:";
            // 
            // txtCodigoPropiedad
            // 
            this.txtCodigoPropiedad.BackColor = System.Drawing.Color.White;
            this.txtCodigoPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoPropiedad.Location = new System.Drawing.Point(700, 63);
            this.txtCodigoPropiedad.Name = "txtCodigoPropiedad";
            this.txtCodigoPropiedad.ReadOnly = true;
            this.txtCodigoPropiedad.Size = new System.Drawing.Size(110, 22);
            this.txtCodigoPropiedad.TabIndex = 10;
            this.txtCodigoPropiedad.TabStop = false;
            // 
            // lblCodigoPropiedad
            // 
            this.lblCodigoPropiedad.AutoSize = true;
            this.lblCodigoPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoPropiedad.Location = new System.Drawing.Point(580, 66);
            this.lblCodigoPropiedad.Name = "lblCodigoPropiedad";
            this.lblCodigoPropiedad.Size = new System.Drawing.Size(119, 14);
            this.lblCodigoPropiedad.TabIndex = 11;
            this.lblCodigoPropiedad.Text = "Código Propiedad:";
            // 
            // txtPropiedadDireccion
            // 
            this.txtPropiedadDireccion.BackColor = System.Drawing.Color.White;
            this.txtPropiedadDireccion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPropiedadDireccion.Location = new System.Drawing.Point(150, 98);
            this.txtPropiedadDireccion.Name = "txtPropiedadDireccion";
            this.txtPropiedadDireccion.ReadOnly = true;
            this.txtPropiedadDireccion.Size = new System.Drawing.Size(830, 22);
            this.txtPropiedadDireccion.TabIndex = 12;
            this.txtPropiedadDireccion.TabStop = false;
            // 
            // lblPropiedadDireccion
            // 
            this.lblPropiedadDireccion.AutoSize = true;
            this.lblPropiedadDireccion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPropiedadDireccion.Location = new System.Drawing.Point(20, 101);
            this.lblPropiedadDireccion.Name = "lblPropiedadDireccion";
            this.lblPropiedadDireccion.Size = new System.Drawing.Size(79, 14);
            this.lblPropiedadDireccion.TabIndex = 13;
            this.lblPropiedadDireccion.Text = "Dirección:";
            // 
            // txtTipoContrato
            // 
            this.txtTipoContrato.BackColor = System.Drawing.Color.White;
            this.txtTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTipoContrato.Location = new System.Drawing.Point(150, 133);
            this.txtTipoContrato.Name = "txtTipoContrato";
            this.txtTipoContrato.ReadOnly = true;
            this.txtTipoContrato.Size = new System.Drawing.Size(130, 22);
            this.txtTipoContrato.TabIndex = 14;
            this.txtTipoContrato.TabStop = false;
            // 
            // lblTipoContrato
            // 
            this.lblTipoContrato.AutoSize = true;
            this.lblTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoContrato.Location = new System.Drawing.Point(20, 136);
            this.lblTipoContrato.Name = "lblTipoContrato";
            this.lblTipoContrato.Size = new System.Drawing.Size(100, 14);
            this.lblTipoContrato.TabIndex = 15;
            this.lblTipoContrato.Text = "Tipo Contrato:";
            // 
            // txtFechaInicio
            // 
            this.txtFechaInicio.BackColor = System.Drawing.Color.White;
            this.txtFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaInicio.Location = new System.Drawing.Point(430, 133);
            this.txtFechaInicio.Name = "txtFechaInicio";
            this.txtFechaInicio.ReadOnly = true;
            this.txtFechaInicio.Size = new System.Drawing.Size(100, 22);
            this.txtFechaInicio.TabIndex = 16;
            this.txtFechaInicio.TabStop = false;
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicio.Location = new System.Drawing.Point(320, 136);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(84, 14);
            this.lblFechaInicio.TabIndex = 17;
            this.lblFechaInicio.Text = "Fecha Inicio:";
            // 
            // txtFechaFin
            // 
            this.txtFechaFin.BackColor = System.Drawing.Color.White;
            this.txtFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaFin.Location = new System.Drawing.Point(700, 133);
            this.txtFechaFin.Name = "txtFechaFin";
            this.txtFechaFin.ReadOnly = true;
            this.txtFechaFin.Size = new System.Drawing.Size(110, 22);
            this.txtFechaFin.TabIndex = 18;
            this.txtFechaFin.TabStop = false;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFin.Location = new System.Drawing.Point(580, 136);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(69, 14);
            this.lblFechaFin.TabIndex = 19;
            this.lblFechaFin.Text = "Fecha Fin:";
            // 
            // txtMontoAcordado
            // 
            this.txtMontoAcordado.BackColor = System.Drawing.Color.White;
            this.txtMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMontoAcordado.Location = new System.Drawing.Point(150, 168);
            this.txtMontoAcordado.Name = "txtMontoAcordado";
            this.txtMontoAcordado.ReadOnly = true;
            this.txtMontoAcordado.Size = new System.Drawing.Size(130, 22);
            this.txtMontoAcordado.TabIndex = 20;
            this.txtMontoAcordado.TabStop = false;
            // 
            // lblMontoAcordado
            // 
            this.lblMontoAcordado.AutoSize = true;
            this.lblMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoAcordado.Location = new System.Drawing.Point(20, 171);
            this.lblMontoAcordado.Name = "lblMontoAcordado";
            this.lblMontoAcordado.Size = new System.Drawing.Size(116, 14);
            this.lblMontoAcordado.TabIndex = 21;
            this.lblMontoAcordado.Text = "Monto Acordado:";
            // 
            // txtFormaPago
            // 
            this.txtFormaPago.BackColor = System.Drawing.Color.White;
            this.txtFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormaPago.Location = new System.Drawing.Point(430, 168);
            this.txtFormaPago.Name = "txtFormaPago";
            this.txtFormaPago.ReadOnly = true;
            this.txtFormaPago.Size = new System.Drawing.Size(130, 22);
            this.txtFormaPago.TabIndex = 22;
            this.txtFormaPago.TabStop = false;
            // 
            // lblFormaPago
            // 
            this.lblFormaPago.AutoSize = true;
            this.lblFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormaPago.Location = new System.Drawing.Point(320, 171);
            this.lblFormaPago.Name = "lblFormaPago";
            this.lblFormaPago.Size = new System.Drawing.Size(90, 14);
            this.lblFormaPago.TabIndex = 23;
            this.lblFormaPago.Text = "Forma Pago:";
            // 
            // txtDepositoGarantia
            // 
            this.txtDepositoGarantia.BackColor = System.Drawing.Color.White;
            this.txtDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepositoGarantia.Location = new System.Drawing.Point(700, 168);
            this.txtDepositoGarantia.Name = "txtDepositoGarantia";
            this.txtDepositoGarantia.ReadOnly = true;
            this.txtDepositoGarantia.Size = new System.Drawing.Size(110, 22);
            this.txtDepositoGarantia.TabIndex = 24;
            this.txtDepositoGarantia.TabStop = false;
            // 
            // lblDepositoGarantia
            // 
            this.lblDepositoGarantia.AutoSize = true;
            this.lblDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositoGarantia.Location = new System.Drawing.Point(580, 171);
            this.lblDepositoGarantia.Name = "lblDepositoGarantia";
            this.lblDepositoGarantia.Size = new System.Drawing.Size(94, 14);
            this.lblDepositoGarantia.TabIndex = 25;
            this.lblDepositoGarantia.Text = "Depósito Gtía:";
            // 
            // gbDecision
            // 
            this.gbDecision.Controls.Add(this.cboNuevoEstado);
            this.gbDecision.Controls.Add(this.lblNuevoEstado);
            this.gbDecision.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbDecision.Location = new System.Drawing.Point(20, 520);
            this.gbDecision.Name = "gbDecision";
            this.gbDecision.Size = new System.Drawing.Size(480, 80);
            this.gbDecision.TabIndex = 3;
            this.gbDecision.TabStop = false;
            this.gbDecision.Text = "Nuevo estado";
            // 
            // cboNuevoEstado
            // 
            this.cboNuevoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNuevoEstado.Enabled = false;
            this.cboNuevoEstado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboNuevoEstado.FormattingEnabled = true;
            this.cboNuevoEstado.Location = new System.Drawing.Point(150, 32);
            this.cboNuevoEstado.Name = "cboNuevoEstado";
            this.cboNuevoEstado.Size = new System.Drawing.Size(200, 22);
            this.cboNuevoEstado.TabIndex = 0;
            this.cboNuevoEstado.SelectedIndexChanged += new System.EventHandler(this.cboNuevoEstado_SelectedIndexChanged);
            // 
            // lblNuevoEstado
            // 
            this.lblNuevoEstado.AutoSize = true;
            this.lblNuevoEstado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNuevoEstado.Location = new System.Drawing.Point(20, 35);
            this.lblNuevoEstado.Name = "lblNuevoEstado";
            this.lblNuevoEstado.Size = new System.Drawing.Size(87, 14);
            this.lblNuevoEstado.TabIndex = 1;
            this.lblNuevoEstado.Text = "Nuevo estado:";
            // 
            // btnModificarEstado
            // 
            this.btnModificarEstado.BackColor = System.Drawing.Color.SteelBlue;
            this.btnModificarEstado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificarEstado.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnModificarEstado.Location = new System.Drawing.Point(650, 540);
            this.btnModificarEstado.Name = "btnModificarEstado";
            this.btnModificarEstado.Size = new System.Drawing.Size(170, 40);
            this.btnModificarEstado.TabIndex = 4;
            this.btnModificarEstado.Text = "Modificar Estado";
            this.btnModificarEstado.UseVisualStyleBackColor = false;
            this.btnModificarEstado.Click += new System.EventHandler(this.btnModificarEstado_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGuardar.Enabled = false;
            this.btnGuardar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGuardar.Location = new System.Drawing.Point(835, 540);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(150, 40);
            this.btnGuardar.TabIndex = 5;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // FrmActualizarEstadoPropiedadContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1020, 620);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnModificarEstado);
            this.Controls.Add(this.gbDecision);
            this.Controls.Add(this.gbDetalle);
            this.Controls.Add(this.gbContratos);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmActualizarEstadoPropiedadContrato";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Actualizar Estado de Propiedad y Contrato";
            this.gbContratos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContratos)).EndInit();
            this.gbDetalle.ResumeLayout(false);
            this.gbDetalle.PerformLayout();
            this.gbDecision.ResumeLayout(false);
            this.gbDecision.PerformLayout();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaInicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFechaFin;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMontoAcordado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.GroupBox gbDetalle;
        private System.Windows.Forms.TextBox txtNroContrato;
        private System.Windows.Forms.Label lblNroContrato;
        private System.Windows.Forms.TextBox txtFechaEmision;
        private System.Windows.Forms.Label lblFechaEmision;
        private System.Windows.Forms.TextBox txtEstadoActual;
        private System.Windows.Forms.Label lblEstadoActual;
        private System.Windows.Forms.TextBox txtDNIPropietario;
        private System.Windows.Forms.Label lblDNIPropietario;
        private System.Windows.Forms.TextBox txtDNICliente;
        private System.Windows.Forms.Label lblDNICliente;
        private System.Windows.Forms.TextBox txtCodigoPropiedad;
        private System.Windows.Forms.Label lblCodigoPropiedad;
        private System.Windows.Forms.TextBox txtPropiedadDireccion;
        private System.Windows.Forms.Label lblPropiedadDireccion;
        private System.Windows.Forms.TextBox txtTipoContrato;
        private System.Windows.Forms.Label lblTipoContrato;
        private System.Windows.Forms.TextBox txtFechaInicio;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.TextBox txtFechaFin;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.TextBox txtMontoAcordado;
        private System.Windows.Forms.Label lblMontoAcordado;
        private System.Windows.Forms.TextBox txtFormaPago;
        private System.Windows.Forms.Label lblFormaPago;
        private System.Windows.Forms.TextBox txtDepositoGarantia;
        private System.Windows.Forms.Label lblDepositoGarantia;
        private System.Windows.Forms.GroupBox gbDecision;
        private System.Windows.Forms.ComboBox cboNuevoEstado;
        private System.Windows.Forms.Label lblNuevoEstado;
        private System.Windows.Forms.Button btnModificarEstado;
        private System.Windows.Forms.Button btnGuardar;
    }
}
