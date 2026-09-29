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
            this.gbABM = new System.Windows.Forms.GroupBox();
            this.btnNuevoProp = new System.Windows.Forms.Button();
            this.btnEliminarProp = new System.Windows.Forms.Button();
            this.btnModificarProp = new System.Windows.Forms.Button();
            this.btnGuardarProp = new System.Windows.Forms.Button();
            this.lblBaniosABM = new System.Windows.Forms.Label();
            this.txtBaniosABM = new System.Windows.Forms.TextBox();
            this.lblDormitoriosABM = new System.Windows.Forms.Label();
            this.txtDormitoriosABM = new System.Windows.Forms.TextBox();
            this.lblAmbientesABM = new System.Windows.Forms.Label();
            this.txtAmbientesABM = new System.Windows.Forms.TextBox();
            this.lblSuperficieABM = new System.Windows.Forms.Label();
            this.txtSuperficieABM = new System.Windows.Forms.TextBox();
            this.lblPrecioABM = new System.Windows.Forms.Label();
            this.txtPrecioABM = new System.Windows.Forms.TextBox();
            this.lblEstadoABM = new System.Windows.Forms.Label();
            this.cboEstadoABM = new System.Windows.Forms.ComboBox();
            this.lblTipoABM = new System.Windows.Forms.Label();
            this.cboTipoABM = new System.Windows.Forms.ComboBox();
            this.lblDirABM = new System.Windows.Forms.Label();
            this.txtDirABM = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPropiedades)).BeginInit();
            this.gbABM.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPropiedades
            // 
            this.dgvPropiedades.AllowUserToAddRows = false;
            this.dgvPropiedades.AllowUserToDeleteRows = false;
            this.dgvPropiedades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPropiedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPropiedades.Location = new System.Drawing.Point(24, 128);
            this.dgvPropiedades.MultiSelect = false;
            this.dgvPropiedades.Name = "dgvPropiedades";
            this.dgvPropiedades.ReadOnly = true;
            this.dgvPropiedades.RowHeadersVisible = false;
            this.dgvPropiedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPropiedades.Size = new System.Drawing.Size(900, 200);
            this.dgvPropiedades.TabIndex = 10;
            this.dgvPropiedades.DoubleClick += new System.EventHandler(this.dgvPropiedades_DoubleClick);
            // 
            // lblDireccion
            // 
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblDireccion.Location = new System.Drawing.Point(24, 22);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(79, 17);
            this.lblDireccion.TabIndex = 0;
            this.lblDireccion.Text = "Dirección";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(120, 18);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(300, 20);
            this.txtDireccion.TabIndex = 1;
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblTipo.Location = new System.Drawing.Point(460, 22);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(42, 17);
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
            this.cboTipo.Location = new System.Drawing.Point(520, 18);
            this.cboTipo.Name = "cboTipo";
            this.cboTipo.Size = new System.Drawing.Size(160, 21);
            this.cboTipo.TabIndex = 3;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(24, 62);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(61, 17);
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
            this.cboEstado.Location = new System.Drawing.Point(120, 58);
            this.cboEstado.Name = "cboEstado";
            this.cboEstado.Size = new System.Drawing.Size(160, 21);
            this.cboEstado.TabIndex = 5;
            // 
            // lblPrecioMin
            // 
            this.lblPrecioMin.AutoSize = true;
            this.lblPrecioMin.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecioMin.Location = new System.Drawing.Point(320, 62);
            this.lblPrecioMin.Name = "lblPrecioMin";
            this.lblPrecioMin.Size = new System.Drawing.Size(94, 17);
            this.lblPrecioMin.TabIndex = 6;
            this.lblPrecioMin.Text = "Precio mín.";
            // 
            // txtPrecioMin
            // 
            this.txtPrecioMin.Location = new System.Drawing.Point(410, 58);
            this.txtPrecioMin.Name = "txtPrecioMin";
            this.txtPrecioMin.Size = new System.Drawing.Size(130, 20);
            this.txtPrecioMin.TabIndex = 7;
            // 
            // lblPrecioMax
            // 
            this.lblPrecioMax.AutoSize = true;
            this.lblPrecioMax.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecioMax.Location = new System.Drawing.Point(570, 62);
            this.lblPrecioMax.Name = "lblPrecioMax";
            this.lblPrecioMax.Size = new System.Drawing.Size(98, 17);
            this.lblPrecioMax.TabIndex = 8;
            this.lblPrecioMax.Text = "Precio máx.";
            // 
            // txtPrecioMax
            // 
            this.txtPrecioMax.Location = new System.Drawing.Point(680, 58);
            this.txtPrecioMax.Name = "txtPrecioMax";
            this.txtPrecioMax.Size = new System.Drawing.Size(130, 20);
            this.txtPrecioMax.TabIndex = 9;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnBuscar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnBuscar.Location = new System.Drawing.Point(700, 14);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 32);
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
            this.btnLimpiar.Location = new System.Drawing.Point(820, 14);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 32);
            this.btnLimpiar.TabIndex = 12;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSeleccionar.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSeleccionar.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.btnSeleccionar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSeleccionar.Location = new System.Drawing.Point(810, 546);
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Size = new System.Drawing.Size(116, 36);
            this.btnSeleccionar.TabIndex = 13;
            this.btnSeleccionar.Text = "Seleccionar";
            this.btnSeleccionar.UseVisualStyleBackColor = false;
            this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(24, 340);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(15, 17);
            this.lblResultado.TabIndex = 14;
            this.lblResultado.Text = "-";
            // 
            // gbABM
            // 
            this.gbABM.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbABM.Controls.Add(this.btnNuevoProp);
            this.gbABM.Controls.Add(this.btnEliminarProp);
            this.gbABM.Controls.Add(this.btnModificarProp);
            this.gbABM.Controls.Add(this.btnGuardarProp);
            this.gbABM.Controls.Add(this.lblBaniosABM);
            this.gbABM.Controls.Add(this.txtBaniosABM);
            this.gbABM.Controls.Add(this.lblDormitoriosABM);
            this.gbABM.Controls.Add(this.txtDormitoriosABM);
            this.gbABM.Controls.Add(this.lblAmbientesABM);
            this.gbABM.Controls.Add(this.txtAmbientesABM);
            this.gbABM.Controls.Add(this.lblSuperficieABM);
            this.gbABM.Controls.Add(this.txtSuperficieABM);
            this.gbABM.Controls.Add(this.lblPrecioABM);
            this.gbABM.Controls.Add(this.txtPrecioABM);
            this.gbABM.Controls.Add(this.lblEstadoABM);
            this.gbABM.Controls.Add(this.cboEstadoABM);
            this.gbABM.Controls.Add(this.lblTipoABM);
            this.gbABM.Controls.Add(this.cboTipoABM);
            this.gbABM.Controls.Add(this.lblDirABM);
            this.gbABM.Controls.Add(this.txtDirABM);
            this.gbABM.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
            this.gbABM.Location = new System.Drawing.Point(24, 360);
            this.gbABM.Name = "gbABM";
            this.gbABM.Size = new System.Drawing.Size(902, 232);
            this.gbABM.TabIndex = 15;
            this.gbABM.TabStop = false;
            this.gbABM.Text = "Alta / Baja / Modificación";
            // 
            // btnNuevoProp
            // 
            this.btnNuevoProp.BackColor = System.Drawing.Color.SteelBlue;
            this.btnNuevoProp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnNuevoProp.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnNuevoProp.Location = new System.Drawing.Point(534, 176);
            this.btnNuevoProp.Name = "btnNuevoProp";
            this.btnNuevoProp.Size = new System.Drawing.Size(160, 38);
            this.btnNuevoProp.TabIndex = 19;
            this.btnNuevoProp.Text = "Nuevo";
            this.btnNuevoProp.UseVisualStyleBackColor = false;
            this.btnNuevoProp.Click += new System.EventHandler(this.btnNuevoProp_Click);
            // 
            // btnEliminarProp
            // 
            this.btnEliminarProp.BackColor = System.Drawing.Color.SteelBlue;
            this.btnEliminarProp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnEliminarProp.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnEliminarProp.Location = new System.Drawing.Point(362, 176);
            this.btnEliminarProp.Name = "btnEliminarProp";
            this.btnEliminarProp.Size = new System.Drawing.Size(160, 38);
            this.btnEliminarProp.TabIndex = 18;
            this.btnEliminarProp.Text = "Eliminar";
            this.btnEliminarProp.UseVisualStyleBackColor = false;
            this.btnEliminarProp.Click += new System.EventHandler(this.btnEliminarProp_Click);
            // 
            // btnModificarProp
            // 
            this.btnModificarProp.BackColor = System.Drawing.Color.SteelBlue;
            this.btnModificarProp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnModificarProp.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnModificarProp.Location = new System.Drawing.Point(190, 176);
            this.btnModificarProp.Name = "btnModificarProp";
            this.btnModificarProp.Size = new System.Drawing.Size(160, 38);
            this.btnModificarProp.TabIndex = 17;
            this.btnModificarProp.Text = "Modificar";
            this.btnModificarProp.UseVisualStyleBackColor = false;
            this.btnModificarProp.Click += new System.EventHandler(this.btnModificarProp_Click);
            // 
            // btnGuardarProp
            // 
            this.btnGuardarProp.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGuardarProp.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardarProp.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGuardarProp.Location = new System.Drawing.Point(18, 176);
            this.btnGuardarProp.Name = "btnGuardarProp";
            this.btnGuardarProp.Size = new System.Drawing.Size(160, 38);
            this.btnGuardarProp.TabIndex = 16;
            this.btnGuardarProp.Text = "Guardar";
            this.btnGuardarProp.UseVisualStyleBackColor = false;
            this.btnGuardarProp.Click += new System.EventHandler(this.btnGuardarProp_Click);
            // 
            // lblBaniosABM
            // 
            this.lblBaniosABM.AutoSize = true;
            this.lblBaniosABM.Location = new System.Drawing.Point(350, 116);
            this.lblBaniosABM.Name = "lblBaniosABM";
            this.lblBaniosABM.Size = new System.Drawing.Size(56, 17);
            this.lblBaniosABM.TabIndex = 14;
            this.lblBaniosABM.Text = "Baños";
            // 
            // txtBaniosABM
            // 
            this.txtBaniosABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtBaniosABM.Location = new System.Drawing.Point(450, 112);
            this.txtBaniosABM.Name = "txtBaniosABM";
            this.txtBaniosABM.Size = new System.Drawing.Size(200, 22);
            this.txtBaniosABM.TabIndex = 15;
            // 
            // lblDormitoriosABM
            // 
            this.lblDormitoriosABM.AutoSize = true;
            this.lblDormitoriosABM.Location = new System.Drawing.Point(18, 116);
            this.lblDormitoriosABM.Name = "lblDormitoriosABM";
            this.lblDormitoriosABM.Size = new System.Drawing.Size(99, 17);
            this.lblDormitoriosABM.TabIndex = 12;
            this.lblDormitoriosABM.Text = "Dormitorios";
            // 
            // txtDormitoriosABM
            // 
            this.txtDormitoriosABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtDormitoriosABM.Location = new System.Drawing.Point(120, 112);
            this.txtDormitoriosABM.Name = "txtDormitoriosABM";
            this.txtDormitoriosABM.Size = new System.Drawing.Size(200, 22);
            this.txtDormitoriosABM.TabIndex = 13;
            // 
            // lblAmbientesABM
            // 
            this.lblAmbientesABM.AutoSize = true;
            this.lblAmbientesABM.Location = new System.Drawing.Point(680, 74);
            this.lblAmbientesABM.Name = "lblAmbientesABM";
            this.lblAmbientesABM.Size = new System.Drawing.Size(89, 17);
            this.lblAmbientesABM.TabIndex = 10;
            this.lblAmbientesABM.Text = "Ambientes";
            // 
            // txtAmbientesABM
            // 
            this.txtAmbientesABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtAmbientesABM.Location = new System.Drawing.Point(775, 70);
            this.txtAmbientesABM.Name = "txtAmbientesABM";
            this.txtAmbientesABM.Size = new System.Drawing.Size(105, 22);
            this.txtAmbientesABM.TabIndex = 11;
            // 
            // lblSuperficieABM
            // 
            this.lblSuperficieABM.AutoSize = true;
            this.lblSuperficieABM.Location = new System.Drawing.Point(350, 74);
            this.lblSuperficieABM.Name = "lblSuperficieABM";
            this.lblSuperficieABM.Size = new System.Drawing.Size(130, 17);
            this.lblSuperficieABM.TabIndex = 8;
            this.lblSuperficieABM.Text = "Superficie (m2)";
            // 
            // txtSuperficieABM
            // 
            this.txtSuperficieABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtSuperficieABM.Location = new System.Drawing.Point(486, 72);
            this.txtSuperficieABM.Name = "txtSuperficieABM";
            this.txtSuperficieABM.Size = new System.Drawing.Size(176, 22);
            this.txtSuperficieABM.TabIndex = 9;
            // 
            // lblPrecioABM
            // 
            this.lblPrecioABM.AutoSize = true;
            this.lblPrecioABM.Location = new System.Drawing.Point(18, 74);
            this.lblPrecioABM.Name = "lblPrecioABM";
            this.lblPrecioABM.Size = new System.Drawing.Size(56, 17);
            this.lblPrecioABM.TabIndex = 6;
            this.lblPrecioABM.Text = "Precio";
            // 
            // txtPrecioABM
            // 
            this.txtPrecioABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtPrecioABM.Location = new System.Drawing.Point(120, 70);
            this.txtPrecioABM.Name = "txtPrecioABM";
            this.txtPrecioABM.Size = new System.Drawing.Size(200, 22);
            this.txtPrecioABM.TabIndex = 7;
            // 
            // lblEstadoABM
            // 
            this.lblEstadoABM.AutoSize = true;
            this.lblEstadoABM.Location = new System.Drawing.Point(680, 32);
            this.lblEstadoABM.Name = "lblEstadoABM";
            this.lblEstadoABM.Size = new System.Drawing.Size(61, 17);
            this.lblEstadoABM.TabIndex = 4;
            this.lblEstadoABM.Text = "Estado";
            // 
            // cboEstadoABM
            // 
            this.cboEstadoABM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstadoABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.cboEstadoABM.FormattingEnabled = true;
            this.cboEstadoABM.Location = new System.Drawing.Point(747, 28);
            this.cboEstadoABM.Name = "cboEstadoABM";
            this.cboEstadoABM.Size = new System.Drawing.Size(133, 22);
            this.cboEstadoABM.TabIndex = 5;
            // 
            // lblTipoABM
            // 
            this.lblTipoABM.AutoSize = true;
            this.lblTipoABM.Location = new System.Drawing.Point(450, 32);
            this.lblTipoABM.Name = "lblTipoABM";
            this.lblTipoABM.Size = new System.Drawing.Size(42, 17);
            this.lblTipoABM.TabIndex = 2;
            this.lblTipoABM.Text = "Tipo";
            // 
            // cboTipoABM
            // 
            this.cboTipoABM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.cboTipoABM.FormattingEnabled = true;
            this.cboTipoABM.Location = new System.Drawing.Point(500, 28);
            this.cboTipoABM.Name = "cboTipoABM";
            this.cboTipoABM.Size = new System.Drawing.Size(150, 22);
            this.cboTipoABM.TabIndex = 3;
            // 
            // lblDirABM
            // 
            this.lblDirABM.AutoSize = true;
            this.lblDirABM.Location = new System.Drawing.Point(18, 32);
            this.lblDirABM.Name = "lblDirABM";
            this.lblDirABM.Size = new System.Drawing.Size(79, 17);
            this.lblDirABM.TabIndex = 0;
            this.lblDirABM.Text = "Dirección";
            // 
            // txtDirABM
            // 
            this.txtDirABM.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtDirABM.Location = new System.Drawing.Point(120, 28);
            this.txtDirABM.Name = "txtDirABM";
            this.txtDirABM.Size = new System.Drawing.Size(300, 22);
            this.txtDirABM.TabIndex = 1;
            // 
            // SeleccionarPropiedad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(950, 592);
            this.Controls.Add(this.gbABM);
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
            this.MinimumSize = new System.Drawing.Size(930, 560);
            this.Name = "SeleccionarPropiedad";
            this.Text = "Seleccionar Propiedad";
            this.Load += new System.EventHandler(this.SeleccionarPropiedad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPropiedades)).EndInit();
            this.gbABM.ResumeLayout(false);
            this.gbABM.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbABM;
        private System.Windows.Forms.TextBox txtDirABM;
        private System.Windows.Forms.Label lblDirABM;
        private System.Windows.Forms.ComboBox cboTipoABM;
        private System.Windows.Forms.Label lblTipoABM;
        private System.Windows.Forms.ComboBox cboEstadoABM;
        private System.Windows.Forms.Label lblEstadoABM;
        private System.Windows.Forms.TextBox txtPrecioABM;
        private System.Windows.Forms.Label lblPrecioABM;
        private System.Windows.Forms.TextBox txtSuperficieABM;
        private System.Windows.Forms.Label lblSuperficieABM;
        private System.Windows.Forms.TextBox txtAmbientesABM;
        private System.Windows.Forms.Label lblAmbientesABM;
        private System.Windows.Forms.TextBox txtDormitoriosABM;
        private System.Windows.Forms.Label lblDormitoriosABM;
        private System.Windows.Forms.TextBox txtBaniosABM;
        private System.Windows.Forms.Label lblBaniosABM;
        private System.Windows.Forms.Button btnGuardarProp;
        private System.Windows.Forms.Button btnModificarProp;
        private System.Windows.Forms.Button btnEliminarProp;
        private System.Windows.Forms.Button btnNuevoProp;
    }
}