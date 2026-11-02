namespace Servicios
{
    partial class FrmRegistrarTerminosCondiciones
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
            this.gbTerminos = new System.Windows.Forms.GroupBox();
            this.txtDepositoGarantia = new System.Windows.Forms.TextBox();
            this.lblDepositoGarantia = new System.Windows.Forms.Label();
            this.txtMontoAcordado = new System.Windows.Forms.TextBox();
            this.lblMontoAcordado = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.cboFormaPago = new System.Windows.Forms.ComboBox();
            this.lblFormaPago = new System.Windows.Forms.Label();
            this.cboTipoContrato = new System.Windows.Forms.ComboBox();
            this.lblTipoContrato = new System.Windows.Forms.Label();
            this.btnConfirmarTerminos = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.gbTerminos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Verdana", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(20, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(276, 29);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Términos y Condiciones";
            // 
            // gbTerminos
            // 
            this.gbTerminos.Controls.Add(this.txtDepositoGarantia);
            this.gbTerminos.Controls.Add(this.lblDepositoGarantia);
            this.gbTerminos.Controls.Add(this.txtMontoAcordado);
            this.gbTerminos.Controls.Add(this.lblMontoAcordado);
            this.gbTerminos.Controls.Add(this.dtpFechaFin);
            this.gbTerminos.Controls.Add(this.lblFechaFin);
            this.gbTerminos.Controls.Add(this.dtpFechaInicio);
            this.gbTerminos.Controls.Add(this.lblFechaInicio);
            this.gbTerminos.Controls.Add(this.cboFormaPago);
            this.gbTerminos.Controls.Add(this.lblFormaPago);
            this.gbTerminos.Controls.Add(this.cboTipoContrato);
            this.gbTerminos.Controls.Add(this.lblTipoContrato);
            this.gbTerminos.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTerminos.Location = new System.Drawing.Point(20, 55);
            this.gbTerminos.Name = "gbTerminos";
            this.gbTerminos.Size = new System.Drawing.Size(520, 250);
            this.gbTerminos.TabIndex = 1;
            this.gbTerminos.TabStop = false;
            this.gbTerminos.Text = "Términos comerciales";
            // 
            // txtDepositoGarantia
            // 
            this.txtDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDepositoGarantia.Location = new System.Drawing.Point(220, 207);
            this.txtDepositoGarantia.Name = "txtDepositoGarantia";
            this.txtDepositoGarantia.Size = new System.Drawing.Size(270, 22);
            this.txtDepositoGarantia.TabIndex = 11;
            // 
            // lblDepositoGarantia
            // 
            this.lblDepositoGarantia.AutoSize = true;
            this.lblDepositoGarantia.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepositoGarantia.Location = new System.Drawing.Point(25, 210);
            this.lblDepositoGarantia.Name = "lblDepositoGarantia";
            this.lblDepositoGarantia.Size = new System.Drawing.Size(141, 14);
            this.lblDepositoGarantia.TabIndex = 10;
            this.lblDepositoGarantia.Text = "Depósito de garantía:";
            // 
            // txtMontoAcordado
            // 
            this.txtMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMontoAcordado.Location = new System.Drawing.Point(220, 172);
            this.txtMontoAcordado.Name = "txtMontoAcordado";
            this.txtMontoAcordado.Size = new System.Drawing.Size(270, 22);
            this.txtMontoAcordado.TabIndex = 9;
            // 
            // lblMontoAcordado
            // 
            this.lblMontoAcordado.AutoSize = true;
            this.lblMontoAcordado.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMontoAcordado.Location = new System.Drawing.Point(25, 175);
            this.lblMontoAcordado.Name = "lblMontoAcordado";
            this.lblMontoAcordado.Size = new System.Drawing.Size(116, 14);
            this.lblMontoAcordado.TabIndex = 8;
            this.lblMontoAcordado.Text = "Monto acordado:";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(220, 137);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(270, 22);
            this.dtpFechaFin.TabIndex = 7;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFin.Location = new System.Drawing.Point(25, 140);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(66, 14);
            this.lblFechaFin.TabIndex = 6;
            this.lblFechaFin.Text = "Fecha fin:";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(220, 102);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(270, 22);
            this.dtpFechaInicio.TabIndex = 5;
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicio.Location = new System.Drawing.Point(25, 105);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(84, 14);
            this.lblFechaInicio.TabIndex = 4;
            this.lblFechaInicio.Text = "Fecha inicio:";
            // 
            // cboFormaPago
            // 
            this.cboFormaPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFormaPago.FormattingEnabled = true;
            this.cboFormaPago.Location = new System.Drawing.Point(220, 67);
            this.cboFormaPago.Name = "cboFormaPago";
            this.cboFormaPago.Size = new System.Drawing.Size(270, 22);
            this.cboFormaPago.TabIndex = 3;
            // 
            // lblFormaPago
            // 
            this.lblFormaPago.AutoSize = true;
            this.lblFormaPago.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormaPago.Location = new System.Drawing.Point(25, 70);
            this.lblFormaPago.Name = "lblFormaPago";
            this.lblFormaPago.Size = new System.Drawing.Size(90, 14);
            this.lblFormaPago.TabIndex = 2;
            this.lblFormaPago.Text = "Forma de pago:";
            // 
            // cboTipoContrato
            // 
            this.cboTipoContrato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTipoContrato.FormattingEnabled = true;
            this.cboTipoContrato.Location = new System.Drawing.Point(220, 32);
            this.cboTipoContrato.Name = "cboTipoContrato";
            this.cboTipoContrato.Size = new System.Drawing.Size(270, 22);
            this.cboTipoContrato.TabIndex = 1;
            // 
            // lblTipoContrato
            // 
            this.lblTipoContrato.AutoSize = true;
            this.lblTipoContrato.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoContrato.Location = new System.Drawing.Point(25, 35);
            this.lblTipoContrato.Name = "lblTipoContrato";
            this.lblTipoContrato.Size = new System.Drawing.Size(113, 14);
            this.lblTipoContrato.TabIndex = 0;
            this.lblTipoContrato.Text = "Tipo de contrato:";
            // 
            // btnConfirmarTerminos
            // 
            this.btnConfirmarTerminos.BackColor = System.Drawing.Color.SteelBlue;
            this.btnConfirmarTerminos.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmarTerminos.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnConfirmarTerminos.Location = new System.Drawing.Point(120, 325);
            this.btnConfirmarTerminos.Name = "btnConfirmarTerminos";
            this.btnConfirmarTerminos.Size = new System.Drawing.Size(180, 36);
            this.btnConfirmarTerminos.TabIndex = 2;
            this.btnConfirmarTerminos.Text = "Confirmar Términos";
            this.btnConfirmarTerminos.UseVisualStyleBackColor = false;
            this.btnConfirmarTerminos.Click += new System.EventHandler(this.btnConfirmarTerminos_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCancelar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnCancelar.Location = new System.Drawing.Point(320, 325);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 36);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmRegistrarTerminosCondiciones
            // 
            this.AcceptButton = this.btnConfirmarTerminos;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(560, 400);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmarTerminos);
            this.Controls.Add(this.gbTerminos);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRegistrarTerminosCondiciones";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Términos y Condiciones";
            this.gbTerminos.ResumeLayout(false);
            this.gbTerminos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbTerminos;
        private System.Windows.Forms.TextBox txtDepositoGarantia;
        private System.Windows.Forms.Label lblDepositoGarantia;
        private System.Windows.Forms.TextBox txtMontoAcordado;
        private System.Windows.Forms.Label lblMontoAcordado;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private System.Windows.Forms.Label lblFechaInicio;
        private System.Windows.Forms.ComboBox cboFormaPago;
        private System.Windows.Forms.Label lblFormaPago;
        private System.Windows.Forms.ComboBox cboTipoContrato;
        private System.Windows.Forms.Label lblTipoContrato;
        private System.Windows.Forms.Button btnConfirmarTerminos;
        private System.Windows.Forms.Button btnCancelar;
    }
}
