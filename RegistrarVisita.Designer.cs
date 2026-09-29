namespace Servicios
{
    partial class RegistrarVisita
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.gbCliente = new System.Windows.Forms.GroupBox();
            this.lblClienteInfo = new System.Windows.Forms.Label();
            this.btnBuscarCliente = new System.Windows.Forms.Button();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.lblDNI = new System.Windows.Forms.Label();
            this.gbPropiedad = new System.Windows.Forms.GroupBox();
            this.btnSeleccionarPropiedad = new System.Windows.Forms.Button();
            this.lblPropiedadInfo = new System.Windows.Forms.Label();
            this.gbHorario = new System.Windows.Forms.GroupBox();
            this.lblDisponibilidad = new System.Windows.Forms.Label();
            this.dtpHoraFin = new System.Windows.Forms.DateTimePicker();
            this.lblFin = new System.Windows.Forms.Label();
            this.dtpHoraInicio = new System.Windows.Forms.DateTimePicker();
            this.lblInicio = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.lblResultadoRegistro = new System.Windows.Forms.Label();
            this.gbCliente.SuspendLayout();
            this.gbPropiedad.SuspendLayout();
            this.gbHorario.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(24, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(202, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registro de Visita";
            // 
            // gbCliente
            // 
            this.gbCliente.Controls.Add(this.lblClienteInfo);
            this.gbCliente.Controls.Add(this.btnBuscarCliente);
            this.gbCliente.Controls.Add(this.txtDNI);
            this.gbCliente.Controls.Add(this.lblDNI);
            this.gbCliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gbCliente.Location = new System.Drawing.Point(24, 50);
            this.gbCliente.Name = "gbCliente";
            this.gbCliente.Size = new System.Drawing.Size(810, 115);
            this.gbCliente.TabIndex = 1;
            this.gbCliente.TabStop = false;
            this.gbCliente.Text = "Datos del Cliente ";
            // 
            // lblClienteInfo
            // 
            this.lblClienteInfo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblClienteInfo.Location = new System.Drawing.Point(20, 70);
            this.lblClienteInfo.Name = "lblClienteInfo";
            this.lblClienteInfo.Size = new System.Drawing.Size(760, 34);
            this.lblClienteInfo.TabIndex = 4;
            this.lblClienteInfo.Text = "-";
            // 
            // btnBuscarCliente
            // 
            this.btnBuscarCliente.BackColor = System.Drawing.Color.SteelBlue;
            this.btnBuscarCliente.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscarCliente.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnBuscarCliente.Location = new System.Drawing.Point(330, 24);
            this.btnBuscarCliente.Name = "btnBuscarCliente";
            this.btnBuscarCliente.Size = new System.Drawing.Size(170, 32);
            this.btnBuscarCliente.TabIndex = 2;
            this.btnBuscarCliente.Text = "Buscar Cliente";
            this.btnBuscarCliente.UseVisualStyleBackColor = false;
            this.btnBuscarCliente.Click += new System.EventHandler(this.btnBuscarCliente_Click);
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(110, 27);
            this.txtDNI.MaxLength = 8;
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(200, 22);
            this.txtDNI.TabIndex = 1;
            this.txtDNI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDNI_KeyDown);
            // 
            // lblDNI
            // 
            this.lblDNI.AutoSize = true;
            this.lblDNI.Location = new System.Drawing.Point(20, 30);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(33, 14);
            this.lblDNI.TabIndex = 0;
            this.lblDNI.Text = "DNI";
            // 
            // gbPropiedad
            // 
            this.gbPropiedad.Controls.Add(this.btnSeleccionarPropiedad);
            this.gbPropiedad.Controls.Add(this.lblPropiedadInfo);
            this.gbPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gbPropiedad.Location = new System.Drawing.Point(24, 175);
            this.gbPropiedad.Name = "gbPropiedad";
            this.gbPropiedad.Size = new System.Drawing.Size(810, 100);
            this.gbPropiedad.TabIndex = 2;
            this.gbPropiedad.TabStop = false;
            this.gbPropiedad.Text = "Propiedad ";
            // 
            // btnSeleccionarPropiedad
            // 
            this.btnSeleccionarPropiedad.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionarPropiedad.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnSeleccionarPropiedad.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSeleccionarPropiedad.Location = new System.Drawing.Point(620, 30);
            this.btnSeleccionarPropiedad.Name = "btnSeleccionarPropiedad";
            this.btnSeleccionarPropiedad.Size = new System.Drawing.Size(170, 40);
            this.btnSeleccionarPropiedad.TabIndex = 1;
            this.btnSeleccionarPropiedad.Text = "Seleccionar Propiedad";
            this.btnSeleccionarPropiedad.UseVisualStyleBackColor = false;
            this.btnSeleccionarPropiedad.Click += new System.EventHandler(this.btnSeleccionarPropiedad_Click);
            // 
            // lblPropiedadInfo
            // 
            this.lblPropiedadInfo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPropiedadInfo.Location = new System.Drawing.Point(20, 35);
            this.lblPropiedadInfo.Name = "lblPropiedadInfo";
            this.lblPropiedadInfo.Size = new System.Drawing.Size(580, 40);
            this.lblPropiedadInfo.TabIndex = 0;
            this.lblPropiedadInfo.Text = "-";
            // 
            // gbHorario
            // 
            this.gbHorario.Controls.Add(this.lblDisponibilidad);
            this.gbHorario.Controls.Add(this.dtpHoraFin);
            this.gbHorario.Controls.Add(this.lblFin);
            this.gbHorario.Controls.Add(this.dtpHoraInicio);
            this.gbHorario.Controls.Add(this.lblInicio);
            this.gbHorario.Controls.Add(this.dtpFecha);
            this.gbHorario.Controls.Add(this.lblFecha);
            this.gbHorario.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gbHorario.Location = new System.Drawing.Point(24, 285);
            this.gbHorario.Name = "gbHorario";
            this.gbHorario.Size = new System.Drawing.Size(810, 125);
            this.gbHorario.TabIndex = 3;
            this.gbHorario.TabStop = false;
            this.gbHorario.Text = "Fecha y Horario ";
            // 
            // lblDisponibilidad
            // 
            this.lblDisponibilidad.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblDisponibilidad.Location = new System.Drawing.Point(20, 72);
            this.lblDisponibilidad.Name = "lblDisponibilidad";
            this.lblDisponibilidad.Size = new System.Drawing.Size(760, 34);
            this.lblDisponibilidad.TabIndex = 6;
            this.lblDisponibilidad.Text = "-";
            // 
            // dtpHoraFin
            // 
            this.dtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraFin.Location = new System.Drawing.Point(680, 27);
            this.dtpHoraFin.Name = "dtpHoraFin";
            this.dtpHoraFin.ShowUpDown = true;
            this.dtpHoraFin.Size = new System.Drawing.Size(110, 22);
            this.dtpHoraFin.TabIndex = 5;
            // 
            // lblFin
            // 
            this.lblFin.AutoSize = true;
            this.lblFin.Location = new System.Drawing.Point(580, 30);
            this.lblFin.Name = "lblFin";
            this.lblFin.Size = new System.Drawing.Size(60, 14);
            this.lblFin.TabIndex = 4;
            this.lblFin.Text = "Hora fin";
            // 
            // dtpHoraInicio
            // 
            this.dtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpHoraInicio.Location = new System.Drawing.Point(440, 27);
            this.dtpHoraInicio.Name = "dtpHoraInicio";
            this.dtpHoraInicio.ShowUpDown = true;
            this.dtpHoraInicio.Size = new System.Drawing.Size(110, 22);
            this.dtpHoraInicio.TabIndex = 3;
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.Location = new System.Drawing.Point(320, 30);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(98, 14);
            this.lblInicio.TabIndex = 2;
            this.lblInicio.Text = "Hora de inicio";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(150, 27);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(130, 22);
            this.dtpFecha.TabIndex = 1;
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
            // btnConfirmar
            // 
            this.btnConfirmar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnConfirmar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.btnConfirmar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnConfirmar.Location = new System.Drawing.Point(24, 432);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(210, 42);
            this.btnConfirmar.TabIndex = 4;
            this.btnConfirmar.Text = "Confirmar Visita";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLimpiar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnLimpiar.Location = new System.Drawing.Point(250, 432);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(120, 42);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // lblResultadoRegistro
            // 
            this.lblResultadoRegistro.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblResultadoRegistro.Location = new System.Drawing.Point(24, 486);
            this.lblResultadoRegistro.Name = "lblResultadoRegistro";
            this.lblResultadoRegistro.Size = new System.Drawing.Size(810, 40);
            this.lblResultadoRegistro.TabIndex = 6;
            this.lblResultadoRegistro.Text = "-";
            // 
            // RegistrarVisita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(860, 545);
            this.Controls.Add(this.lblResultadoRegistro);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.gbHorario);
            this.Controls.Add(this.gbPropiedad);
            this.Controls.Add(this.gbCliente);
            this.Controls.Add(this.lblTitulo);
            this.Name = "RegistrarVisita";
            this.Text = "Registro de Visita";
            this.Load += new System.EventHandler(this.RegistrarVisita_Load);
            this.gbCliente.ResumeLayout(false);
            this.gbCliente.PerformLayout();
            this.gbPropiedad.ResumeLayout(false);
            this.gbHorario.ResumeLayout(false);
            this.gbHorario.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbCliente;
        private System.Windows.Forms.Label lblClienteInfo;
        private System.Windows.Forms.Button btnBuscarCliente;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.GroupBox gbPropiedad;
        private System.Windows.Forms.Button btnSeleccionarPropiedad;
        private System.Windows.Forms.Label lblPropiedadInfo;
        private System.Windows.Forms.GroupBox gbHorario;
        private System.Windows.Forms.Label lblDisponibilidad;
        private System.Windows.Forms.DateTimePicker dtpHoraFin;
        private System.Windows.Forms.Label lblFin;
        private System.Windows.Forms.DateTimePicker dtpHoraInicio;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Label lblResultadoRegistro;
    }
}