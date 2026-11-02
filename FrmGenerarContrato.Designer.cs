namespace Servicios
{
    partial class FrmGenerarContrato
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
            this.gbClientes = new System.Windows.Forms.GroupBox();
            this.btnRegistrarCliente = new System.Windows.Forms.Button();
            this.txtDNICliente = new System.Windows.Forms.TextBox();
            this.lblDNICliente = new System.Windows.Forms.Label();
            this.btnRegistrarPropietario = new System.Windows.Forms.Button();
            this.txtDNIPropietario = new System.Windows.Forms.TextBox();
            this.lblDNIPropietario = new System.Windows.Forms.Label();
            this.gbPropiedad = new System.Windows.Forms.GroupBox();
            this.txtBanios = new System.Windows.Forms.TextBox();
            this.lblBanios = new System.Windows.Forms.Label();
            this.txtDormitorios = new System.Windows.Forms.TextBox();
            this.lblDormitorios = new System.Windows.Forms.Label();
            this.txtAmbientes = new System.Windows.Forms.TextBox();
            this.lblAmbientes = new System.Windows.Forms.Label();
            this.txtSuperficie = new System.Windows.Forms.TextBox();
            this.lblSuperficie = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtEstado = new System.Windows.Forms.TextBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtTipo = new System.Windows.Forms.TextBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.btnSeleccionarPropiedad = new System.Windows.Forms.Button();
            this.txtCodigoPropiedad = new System.Windows.Forms.TextBox();
            this.lblCodigoPropiedad = new System.Windows.Forms.Label();
            this.gbTerminos = new System.Windows.Forms.GroupBox();
            this.btnTerminosCondiciones = new System.Windows.Forms.Button();
            this.txtDepositoGarantia = new System.Windows.Forms.TextBox();
            this.lblDepositoGarantia = new System.Windows.Forms.Label();
            this.txtMontoAcordado = new System.Windows.Forms.TextBox();
            this.lblMontoAcordado = new System.Windows.Forms.Label();
            this.txtFechaFin = new System.Windows.Forms.TextBox();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.txtFechaInicio = new System.Windows.Forms.TextBox();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.txtFormaPago = new System.Windows.Forms.TextBox();
            this.lblFormaPago = new System.Windows.Forms.Label();
            this.txtTipoContrato = new System.Windows.Forms.TextBox();
            this.lblTipoContrato = new System.Windows.Forms.Label();
            this.dtpFechaEmision = new System.Windows.Forms.DateTimePicker();
            this.lblFechaEmision = new System.Windows.Forms.Label();
            this.btnGenerarContrato = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.gbClientes.SuspendLayout();
            this.gbPropiedad.SuspendLayout();
            this.gbTerminos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(196, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Generar Contrato";
            // 
            // gbClientes
            // 
            this.gbClientes.Controls.Add(this.btnRegistrarCliente);
            this.gbClientes.Controls.Add(this.txtDNICliente);
            this.gbClientes.Controls.Add(this.lblDNICliente);
            this.gbClientes.Controls.Add(this.btnRegistrarPropietario);
            this.gbClientes.Controls.Add(this.txtDNIPropietario);
            this.gbClientes.Controls.Add(this.lblDNIPropietario);
            this.gbClientes.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbClientes.Location = new System.Drawing.Point(20, 55);
            this.gbClientes.Name = "gbClientes";
            this.gbClientes.Size = new System.Drawing.Size(720, 110);
            this.gbClientes.TabIndex = 1;
            this.gbClientes.TabStop = false;
            this.gbClientes.Text = "Clientes";
            // 
            // btnRegistrarCliente
            // 
            this.btnRegistrarCliente.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRegistrarCliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarCliente.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnRegistrarCliente.Location = new System.Drawing.Point(535, 66);
            this.btnRegistrarCliente.Name = "btnRegistrarCliente";
            this.btnRegistrarCliente.Size = new System.Drawing.Size(165, 28);
            this.btnRegistrarCliente.TabIndex = 5;
            this.btnRegistrarCliente.Text = "Registrar cliente...";
            this.btnRegistrarCliente.UseVisualStyleBackColor = false;
            this.btnRegistrarCliente.Click += new System.EventHandler(this.btnRegistrarCliente_Click);
            // 
            // txtDNICliente
            // 
            this.txtDNICliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNICliente.Location = new System.Drawing.Point(240, 67);
            this.txtDNICliente.Name = "txtDNICliente";
            this.txtDNICliente.Size = new System.Drawing.Size(280, 22);
            this.txtDNICliente.TabIndex = 4;
            // 
            // lblDNICliente
            // 
            this.lblDNICliente.AutoSize = true;
            this.lblDNICliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNICliente.Location = new System.Drawing.Point(20, 70);
            this.lblDNICliente.Name = "lblDNICliente";
            this.lblDNICliente.Size = new System.Drawing.Size(173, 14);
            this.lblDNICliente.TabIndex = 3;
            this.lblDNICliente.Text = "DNI Inquilino/Comprador:";
            // 
            // btnRegistrarPropietario
            // 
            this.btnRegistrarPropietario.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRegistrarPropietario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrarPropietario.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnRegistrarPropietario.Location = new System.Drawing.Point(535, 28);
            this.btnRegistrarPropietario.Name = "btnRegistrarPropietario";
            this.btnRegistrarPropietario.Size = new System.Drawing.Size(165, 28);
            this.btnRegistrarPropietario.TabIndex = 2;
            this.btnRegistrarPropietario.Text = "Registrar propietario...";
            this.btnRegistrarPropietario.UseVisualStyleBackColor = false;
            this.btnRegistrarPropietario.Click += new System.EventHandler(this.btnRegistrarPropietario_Click);
            // 
            // txtDNIPropietario
            // 
            this.txtDNIPropietario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDNIPropietario.Location = new System.Drawing.Point(240, 29);
            this.txtDNIPropietario.Name = "txtDNIPropietario";
            this.txtDNIPropietario.Size = new System.Drawing.Size(280, 22);
            this.txtDNIPropietario.TabIndex = 1;
            // 
            // lblDNIPropietario
            // 
            this.lblDNIPropietario.AutoSize = true;
            this.lblDNIPropietario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNIPropietario.Location = new System.Drawing.Point(20, 32);
            this.lblDNIPropietario.Name = "lblDNIPropietario";
            this.lblDNIPropietario.Size = new System.Drawing.Size(111, 14);
            this.lblDNIPropietario.TabIndex = 0;
            this.lblDNIPropietario.Text = "DNI Propietario:";
            // 
            // gbPropiedad
            // 
            this.gbPropiedad.Controls.Add(this.txtBanios);
            this.gbPropiedad.Controls.Add(this.lblBanios);
            this.gbPropiedad.Controls.Add(this.txtDormitorios);
            this.gbPropiedad.Controls.Add(this.lblDormitorios);
            this.gbPropiedad.Controls.Add(this.txtAmbientes);
            this.gbPropiedad.Controls.Add(this.lblAmbientes);
            this.gbPropiedad.Controls.Add(this.txtSuperficie);
            this.gbPropiedad.Controls.Add(this.lblSuperficie);
            this.gbPropiedad.Controls.Add(this.txtPrecio);
            this.gbPropiedad.Controls.Add(this.lblPrecio);
            this.gbPropiedad.Controls.Add(this.txtEstado);
            this.gbPropiedad.Controls.Add(this.lblEstado);
            this.gbPropiedad.Controls.Add(this.txtTipo);
            this.gbPropiedad.Controls.Add(this.lblTipo);
            this.gbPropiedad.Controls.Add(this.txtDireccion);
            this.gbPropiedad.Controls.Add(this.lblDireccion);
            this.gbPropiedad.Controls.Add(this.btnSeleccionarPropiedad);
            this.gbPropiedad.Controls.Add(this.txtCodigoPropiedad);
            this.gbPropiedad.Controls.Add(this.lblCodigoPropiedad);
            this.gbPropiedad.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPropiedad.Location = new System.Drawing.Point(20, 175);
            this.gbPropiedad.Name = "gbPropiedad";
            this.gbPropiedad.Size = new System.Drawing.Size(720, 210);
            this.gbPropiedad.TabIndex = 2;
            this.gbPropiedad.TabStop = false;
            this.gbPropiedad.Text = "Propiedad seleccionada";
            // 
            // txtBanios
            // 
            this.txtBanios.BackColor = System.Drawing.Color.White;
            this.txtBanios.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBanios.Location = new System.Drawing.Point(620, 169);
            this.txtBanios.Name = "txtBanios";
            this.txtBanios.ReadOnly = true;
            this.txtBanios.Size = new System.Drawing.Size(80, 22);
            this.txtBanios.TabIndex = 18;
            this.txtBanios.TabStop = false;
            // 
            // lblBanios
            // 
            this.lblBanios.AutoSize = true;
            this.lblBanios.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBanios.Location = new System.Drawing.Point(560, 172);
            this.lblBanios.Name = "lblBanios";
            this.lblBanios.Size = new System.Drawing.Size(50, 14);
            this.lblBanios.TabIndex = 17;
            this.lblBanios.Text = "Baños:";
            // 
            // txtDormitorios
            // 
            this.txtDormitorios.BackColor = System.Drawing.Color.White;
            this.txtDormitorios.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDormitorios.Location = new System.Drawing.Point(460, 169);
            this.txtDormitorios.Name = "txtDormitorios";
            this.txtDormitorios.ReadOnly = true;
            this.txtDormitorios.Size = new System.Drawing.Size(80, 22);
            this.txtDormitorios.TabIndex = 16;
            this.txtDormitorios.TabStop = false;
            // 
            // lblDormitorios
            // 
            this.lblDormitorios.AutoSize = true;
            this.lblDormitorios.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDormitorios.Location = new System.Drawing.Point(360, 172);
            this.lblDormitorios.Name = "lblDormitorios";
            this.lblDormitorios.Size = new System.Drawing.Size(84, 14);
            this.lblDormitorios.TabIndex = 15;
            this.lblDormitorios.Text = "Dormitorios:";
            // 
            // txtAmbientes
            // 
            this.txtAmbientes.BackColor = System.Drawing.Color.White;
            this.txtAmbientes.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmbientes.Location = new System.Drawing.Point(240, 169);
            this.txtAmbientes.Name = "txtAmbientes";
            this.txtAmbientes.ReadOnly = true;
            this.txtAmbientes.Size = new System.Drawing.Size(100, 22);
            this.txtAmbientes.TabIndex = 14;
            this.txtAmbientes.TabStop = false;
            // 
            // lblAmbientes
            // 
            this.lblAmbientes.AutoSize = true;
            this.lblAmbientes.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmbientes.Location = new System.Drawing.Point(20, 172);
            this.lblAmbientes.Name = "lblAmbientes";
            this.lblAmbientes.Size = new System.Drawing.Size(78, 14);
            this.lblAmbientes.TabIndex = 13;
            this.lblAmbientes.Text = "Ambientes:";
            // 
            // txtSuperficie
            // 
            this.txtSuperficie.BackColor = System.Drawing.Color.White;
            this.txtSuperficie.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSuperficie.Location = new System.Drawing.Point(550, 134);
            this.txtSuperficie.Name = "txtSuperficie";
            this.txtSuperficie.ReadOnly = true;
            this.txtSuperficie.Size = new System.Drawing.Size(150, 22);
            this.txtSuperficie.TabIndex = 12;
            this.txtSuperficie.TabStop = false;
            // 
            // lblSuperficie
            // 
            this.lblSuperficie.AutoSize = true;
            this.lblSuperficie.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSuperficie.Location = new System.Drawing.Point(450, 137);
            this.lblSuperficie.Name = "lblSuperficie";
            this.lblSuperficie.Size = new System.Drawing.Size(97, 14);
            this.lblSuperficie.TabIndex = 11;
            this.lblSuperficie.Text = "Superficie m2:";
            // 
            // txtPrecio
            // 
            this.txtPrecio.BackColor = System.Drawing.Color.White;
            this.txtPrecio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrecio.Location = new System.Drawing.Point(240, 134);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.ReadOnly = true;
            this.txtPrecio.Size = new System.Drawing.Size(180, 22);
            this.txtPrecio.TabIndex = 10;
            this.txtPrecio.TabStop = false;
            // 
            // lblPrecio
            // 
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecio.Location = new System.Drawing.Point(20, 137);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(51, 14);
            this.lblPrecio.TabIndex = 9;
            this.lblPrecio.Text = "Precio:";
            // 
            // txtEstado
            // 
            this.txtEstado.BackColor = System.Drawing.Color.White;
            this.txtEstado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstado.Location = new System.Drawing.Point(550, 99);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.ReadOnly = true;
            this.txtEstado.Size = new System.Drawing.Size(150, 22);
            this.txtEstado.TabIndex = 8;
            this.txtEstado.TabStop = false;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(450, 102);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(56, 14);
            this.lblEstado.TabIndex = 7;
            this.lblEstado.Text = "Estado:";
            // 
            // txtTipo
            // 
            this.txtTipo.BackColor = System.Drawing.Color.White;
            this.txtTipo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTipo.Location = new System.Drawing.Point(240, 99);
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.ReadOnly = true;
            this.txtTipo.Size = new System.Drawing.Size(180, 22);
            this.txtTipo.TabIndex = 6;
            this.txtTipo.TabStop = false;
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipo.Location = new System.Drawing.Point(20, 102);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(41, 14);
            this.lblTipo.TabIndex = 5;
            this.lblTipo.Text = "Tipo:";
            // 
            // txtDireccion
            // 
            this.txtDireccion.BackColor = System.Drawing.Color.White;
            this.txtDireccion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDireccion.Location = new System.Drawing.Point(240, 64);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.ReadOnly = true;
            this.txtDireccion.Size = new System.Drawing.Size(460, 22);
            this.txtDireccion.TabIndex = 4;
            this.txtDireccion.TabStop = false;
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccion.Location = new System.Drawing.Point(20, 67);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(71, 14);
            this.lblDireccion.TabIndex = 3;
            this.lblDireccion.Text = "Dirección:";
            // 
            // btnSeleccionarPropiedad
            // 
            this.btnSeleccionarPropiedad.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionarPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarPropiedad.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSeleccionarPropiedad.Location = new System.Drawing.Point(535, 28);
            this.btnSeleccionarPropiedad.Name = "btnSeleccionarPropiedad";
            this.btnSeleccionarPropiedad.Size = new System.Drawing.Size(165, 28);
            this.btnSeleccionarPropiedad.TabIndex = 2;
            this.btnSeleccionarPropiedad.Text = "Seleccionar propiedad...";
            this.btnSeleccionarPropiedad.UseVisualStyleBackColor = false;
            this.btnSeleccionarPropiedad.Click += new System.EventHandler(this.btnSeleccionarPropiedad_Click);
            // 
            // txtCodigoPropiedad
            // 
            this.txtCodigoPropiedad.BackColor = System.Drawing.Color.White;
            this.txtCodigoPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoPropiedad.Location = new System.Drawing.Point(240, 29);
            this.txtCodigoPropiedad.Name = "txtCodigoPropiedad";
            this.txtCodigoPropiedad.ReadOnly = true;
            this.txtCodigoPropiedad.Size = new System.Drawing.Size(140, 22);
            this.txtCodigoPropiedad.TabIndex = 1;
            this.txtCodigoPropiedad.TabStop = false;
            // 
            // lblCodigoPropiedad
            // 
            this.lblCodigoPropiedad.AutoSize = true;
            this.lblCodigoPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoPropiedad.Location = new System.Drawing.Point(20, 32);
            this.lblCodigoPropiedad.Name = "lblCodigoPropiedad";
            this.lblCodigoPropiedad.Size = new System.Drawing.Size(122, 14);
            this.lblCodigoPropiedad.TabIndex = 0;
            this.lblCodigoPropiedad.Text = "Código Propiedad:";
            // 
            // gbTerminos
            // 
            this.gbTerminos.Controls.Add(this.btnTerminosCondiciones);
            this.gbTerminos.Controls.Add(this.txtDepositoGarantia);
            this.gbTerminos.Controls.Add(this.lblDepositoGarantia);
            this.gbTerminos.Controls.Add(this.txtMontoAcordado);
            this.gbTerminos.Controls.Add(this.lblMontoAcordado);
            this.gbTerminos.Controls.Add(this.txtFechaFin);
            this.gbTerminos.Controls.Add(this.lblFechaFin);
            this.gbTerminos.Controls.Add(this.txtFechaInicio);
            this.gbTerminos.Controls.Add(this.lblFechaInicio);
            this.gbTerminos.Controls.Add(this.txtFormaPago);
            this.gbTerminos.Controls.Add(this.lblFormaPago);
            this.gbTerminos.Controls.Add(this.txtTipoContrato);
            this.gbTerminos.Controls.Add(this.lblTipoContrato);
            this.gbTerminos.Controls.Add(this.dtpFechaEmision);
            this.gbTerminos.Controls.Add(this.lblFechaEmision);
            this.gbTerminos.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTerminos.Location = new System.Drawing.Point(20, 395);
            this.gbTerminos.Name = "gbTerminos";
            this.gbTerminos.Size = new System.Drawing.Size(720, 175);
            this.gbTerminos.TabIndex = 3;
            this.gbTerminos.TabStop = false;
            this.gbTerminos.Text = "Términos y Condiciones";
            // 
            // btnTerminosCondiciones
            // 
            this.btnTerminosCondiciones.BackColor = System.Drawing.Color.SteelBlue;
            this.btnTerminosCondiciones.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTerminosCondiciones.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnTerminosCondiciones.Location = new System.Drawing.Point(535, 28);
            this.btnTerminosCondiciones.Name = "btnTerminosCondiciones";
            this.btnTerminosCondiciones.Size = new System.Drawing.Size(165, 30);
            this.btnTerminosCondiciones.TabIndex = 2;
            this.btnTerminosCondiciones.Text = "Términos y Condiciones";
            this.btnTerminosCondiciones.UseVisualStyleBackColor = false;
            this.btnTerminosCondiciones.Click += new System.EventHandler(this.btnTerminosCondiciones_Click);
            // 
            // txtDepositoGarantia
            // 
            this.txtDepositoGarantia.BackColor = System.Drawing.Color.White;
            this.txtDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepositoGarantia.Location = new System.Drawing.Point(550, 137);
            this.txtDepositoGarantia.Name = "txtDepositoGarantia";
            this.txtDepositoGarantia.ReadOnly = true;
            this.txtDepositoGarantia.Size = new System.Drawing.Size(150, 22);
            this.txtDepositoGarantia.TabIndex = 14;
            this.txtDepositoGarantia.TabStop = false;
            // 
            // lblDepositoGarantia
            // 
            this.lblDepositoGarantia.AutoSize = true;
            this.lblDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositoGarantia.Location = new System.Drawing.Point(450, 140);
            this.lblDepositoGarantia.Name = "lblDepositoGarantia";
            this.lblDepositoGarantia.Size = new System.Drawing.Size(94, 14);
            this.lblDepositoGarantia.TabIndex = 13;
            this.lblDepositoGarantia.Text = "Depósito Gtía:";
            // 
            // txtMontoAcordado
            // 
            this.txtMontoAcordado.BackColor = System.Drawing.Color.White;
            this.txtMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMontoAcordado.Location = new System.Drawing.Point(240, 137);
            this.txtMontoAcordado.Name = "txtMontoAcordado";
            this.txtMontoAcordado.ReadOnly = true;
            this.txtMontoAcordado.Size = new System.Drawing.Size(180, 22);
            this.txtMontoAcordado.TabIndex = 12;
            this.txtMontoAcordado.TabStop = false;
            // 
            // lblMontoAcordado
            // 
            this.lblMontoAcordado.AutoSize = true;
            this.lblMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoAcordado.Location = new System.Drawing.Point(20, 140);
            this.lblMontoAcordado.Name = "lblMontoAcordado";
            this.lblMontoAcordado.Size = new System.Drawing.Size(116, 14);
            this.lblMontoAcordado.TabIndex = 11;
            this.lblMontoAcordado.Text = "Monto Acordado:";
            // 
            // txtFechaFin
            // 
            this.txtFechaFin.BackColor = System.Drawing.Color.White;
            this.txtFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaFin.Location = new System.Drawing.Point(550, 102);
            this.txtFechaFin.Name = "txtFechaFin";
            this.txtFechaFin.ReadOnly = true;
            this.txtFechaFin.Size = new System.Drawing.Size(150, 22);
            this.txtFechaFin.TabIndex = 10;
            this.txtFechaFin.TabStop = false;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFin.Location = new System.Drawing.Point(450, 105);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(66, 14);
            this.lblFechaFin.TabIndex = 9;
            this.lblFechaFin.Text = "Fecha Fin:";
            // 
            // txtFechaInicio
            // 
            this.txtFechaInicio.BackColor = System.Drawing.Color.White;
            this.txtFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFechaInicio.Location = new System.Drawing.Point(240, 102);
            this.txtFechaInicio.Name = "txtFechaInicio";
            this.txtFechaInicio.ReadOnly = true;
            this.txtFechaInicio.Size = new System.Drawing.Size(180, 22);
            this.txtFechaInicio.TabIndex = 8;
            this.txtFechaInicio.TabStop = false;
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicio.Location = new System.Drawing.Point(20, 105);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(84, 14);
            this.lblFechaInicio.TabIndex = 7;
            this.lblFechaInicio.Text = "Fecha Inicio:";
            // 
            // txtFormaPago
            // 
            this.txtFormaPago.BackColor = System.Drawing.Color.White;
            this.txtFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFormaPago.Location = new System.Drawing.Point(550, 67);
            this.txtFormaPago.Name = "txtFormaPago";
            this.txtFormaPago.ReadOnly = true;
            this.txtFormaPago.Size = new System.Drawing.Size(150, 22);
            this.txtFormaPago.TabIndex = 6;
            this.txtFormaPago.TabStop = false;
            // 
            // lblFormaPago
            // 
            this.lblFormaPago.AutoSize = true;
            this.lblFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormaPago.Location = new System.Drawing.Point(450, 70);
            this.lblFormaPago.Name = "lblFormaPago";
            this.lblFormaPago.Size = new System.Drawing.Size(90, 14);
            this.lblFormaPago.TabIndex = 5;
            this.lblFormaPago.Text = "Forma Pago:";
            // 
            // txtTipoContrato
            // 
            this.txtTipoContrato.BackColor = System.Drawing.Color.White;
            this.txtTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTipoContrato.Location = new System.Drawing.Point(240, 67);
            this.txtTipoContrato.Name = "txtTipoContrato";
            this.txtTipoContrato.ReadOnly = true;
            this.txtTipoContrato.Size = new System.Drawing.Size(180, 22);
            this.txtTipoContrato.TabIndex = 4;
            this.txtTipoContrato.TabStop = false;
            // 
            // lblTipoContrato
            // 
            this.lblTipoContrato.AutoSize = true;
            this.lblTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoContrato.Location = new System.Drawing.Point(20, 70);
            this.lblTipoContrato.Name = "lblTipoContrato";
            this.lblTipoContrato.Size = new System.Drawing.Size(77, 14);
            this.lblTipoContrato.TabIndex = 3;
            this.lblTipoContrato.Text = "Tipo Contrato:";
            // 
            // dtpFechaEmision
            // 
            this.dtpFechaEmision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaEmision.Location = new System.Drawing.Point(240, 29);
            this.dtpFechaEmision.Name = "dtpFechaEmision";
            this.dtpFechaEmision.Size = new System.Drawing.Size(180, 22);
            this.dtpFechaEmision.TabIndex = 1;
            // 
            // lblFechaEmision
            // 
            this.lblFechaEmision.AutoSize = true;
            this.lblFechaEmision.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaEmision.Location = new System.Drawing.Point(20, 32);
            this.lblFechaEmision.Name = "lblFechaEmision";
            this.lblFechaEmision.Size = new System.Drawing.Size(101, 14);
            this.lblFechaEmision.TabIndex = 0;
            this.lblFechaEmision.Text = "Fecha Emisión:";
            // 
            // btnGenerarContrato
            // 
            this.btnGenerarContrato.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGenerarContrato.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarContrato.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGenerarContrato.Location = new System.Drawing.Point(240, 590);
            this.btnGenerarContrato.Name = "btnGenerarContrato";
            this.btnGenerarContrato.Size = new System.Drawing.Size(200, 36);
            this.btnGenerarContrato.TabIndex = 4;
            this.btnGenerarContrato.Text = "Generar Contrato";
            this.btnGenerarContrato.UseVisualStyleBackColor = false;
            this.btnGenerarContrato.Click += new System.EventHandler(this.btnGenerarContrato_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLimpiar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnLimpiar.Location = new System.Drawing.Point(460, 590);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(120, 36);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // FrmGenerarContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(760, 646);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnGenerarContrato);
            this.Controls.Add(this.gbTerminos);
            this.Controls.Add(this.gbPropiedad);
            this.Controls.Add(this.gbClientes);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmGenerarContrato";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generar Contrato";
            this.gbClientes.ResumeLayout(false);
            this.gbClientes.PerformLayout();
            this.gbPropiedad.ResumeLayout(false);
            this.gbPropiedad.PerformLayout();
            this.gbTerminos.ResumeLayout(false);
            this.gbTerminos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbClientes;
        private System.Windows.Forms.Button btnRegistrarCliente;
        private System.Windows.Forms.TextBox txtDNICliente;
        private System.Windows.Forms.Label lblDNICliente;
        private System.Windows.Forms.Button btnRegistrarPropietario;
        private System.Windows.Forms.TextBox txtDNIPropietario;
        private System.Windows.Forms.Label lblDNIPropietario;
        private System.Windows.Forms.GroupBox gbPropiedad;
        private System.Windows.Forms.TextBox txtBanios;
        private System.Windows.Forms.Label lblBanios;
        private System.Windows.Forms.TextBox txtDormitorios;
        private System.Windows.Forms.Label lblDormitorios;
        private System.Windows.Forms.TextBox txtAmbientes;
        private System.Windows.Forms.Label lblAmbientes;
        private System.Windows.Forms.TextBox txtSuperficie;
        private System.Windows.Forms.Label lblSuperficie;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtEstado;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TextBox txtTipo;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.Button btnSeleccionarPropiedad;
        private System.Windows.Forms.TextBox txtCodigoPropiedad;
        private System.Windows.Forms.Label lblCodigoPropiedad;
        private System.Windows.Forms.GroupBox gbTerminos;
        private System.Windows.Forms.Button btnTerminosCondiciones;
        private System.Windows.Forms.TextBox txtDepositoGarantia;
        private System.Windows.Forms.Label lblDepositoGarantia;
        private System.Windows.Forms.TextBox txtMontoAcordado;
        private System.Windows.Forms.Label lblMontoAcordado;
        private System.Windows.Forms.TextBox txtFechaFin;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.TextBox txtFechaInicio;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.TextBox txtFormaPago;
        private System.Windows.Forms.Label lblFormaPago;
        private System.Windows.Forms.TextBox txtTipoContrato;
        private System.Windows.Forms.Label lblTipoContrato;
        private System.Windows.Forms.DateTimePicker dtpFechaEmision;
        private System.Windows.Forms.Label lblFechaEmision;
        private System.Windows.Forms.Button btnGenerarContrato;
        private System.Windows.Forms.Button btnLimpiar;
    }
}
