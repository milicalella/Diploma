using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios
{
    public partial class SeleccionarPropiedad : Form, IIdiomaObserver
    {
        private class ItemCombo
        {
            public string Display { get; set; }
            public string Valor { get; set; }
        }

        BLLPropiedad577MC _propiedadBLL = new BLLPropiedad577MC();

        int? _cantidadResultados;

        private readonly bool _modoAbm;
        private int? _idSeleccionado;
        private string _dirFiltro;
        private string _tipoFiltro;
        private string _estadoFiltro;
        private decimal? _precioMinFiltro;
        private decimal? _precioMaxFiltro;

        public Propiedad577MC PropiedadSeleccionada { get; private set; }

        public SeleccionarPropiedad(bool modoAbm = false)
        {
            _modoAbm = modoAbm;
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            FormClosed += (s, e) => ServiceSessionManager577MC.getIntancia().Idioma.Desuscribir(this);
            actualizarIdioma();

            if (_modoAbm)
            {
                gbABM.Visible = true;
                btnSeleccionar.Visible = false;
                CargarCombosABM();
            }
            else
            {
                gbABM.Visible = false;
                btnSeleccionar.Visible = true;

                // Vista compacta para el modo selección (una sola pantalla, sin zona vacía)
                this.MinimumSize = new System.Drawing.Size(930, 452);
                this.ClientSize = new System.Drawing.Size(950, 452);
                dgvPropiedades.Location = new System.Drawing.Point(24, 128);
                dgvPropiedades.Size = new System.Drawing.Size(900, 280);
                lblResultado.Location = new System.Drawing.Point(24, 420);
                btnSeleccionar.Location = new System.Drawing.Point(810, 406);
            }
        }

        private void SeleccionarPropiedad_Load(object sender, EventArgs e)
        {
            try
            {
                CargarCatalogo(_propiedadBLL.ObtenerTodas());
            }
            catch (Exception ex)
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                _dirFiltro = txtDireccion.Text;
                _tipoFiltro = cboTipo.SelectedValue as string;
                _estadoFiltro = cboEstado.SelectedValue as string;
                _precioMinFiltro = LeerPrecio(txtPrecioMin.Text, "minimo");
                _precioMaxFiltro = LeerPrecio(txtPrecioMax.Text, "maximo");

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AplicarFiltro()
        {
            List<Propiedad577MC> resultado = _propiedadBLL.Buscar(_dirFiltro, _tipoFiltro, _estadoFiltro, _precioMinFiltro, _precioMaxFiltro);
            CargarCatalogo(resultado);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDireccion.Clear();
            cboTipo.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            txtPrecioMin.Clear();
            txtPrecioMax.Clear();

            _dirFiltro = null;
            _tipoFiltro = null;
            _estadoFiltro = null;
            _precioMinFiltro = null;
            _precioMaxFiltro = null;

            CargarCatalogo(_propiedadBLL.ObtenerTodas());
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            Seleccionar();
        }

        private void dgvPropiedades_DoubleClick(object sender, EventArgs e)
        {
            if (_modoAbm)
            {
                CargarABM();
            }
            else
            {
                Seleccionar();
            }
        }

        private void dgvPropiedades_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_modoAbm && e.RowIndex >= 0)
            {
                CargarABM();
            }
        }

        private void CargarCatalogo(List<Propiedad577MC> propiedades)
        {
            dgvPropiedades.DataSource = null;
            dgvPropiedades.DataSource = propiedades;
            dgvPropiedades.ClearSelection();

            _cantidadResultados = propiedades.Count;
            MostrarResultados();
        }

        private void MostrarResultados()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_cantidadResultados.HasValue)
            {
                lblResultado.Text = string.Format(t.Translate("SeleccionarPropiedad.msgResultados"), _cantidadResultados.Value);
            }
            else
            {
                lblResultado.Text = "-";
            }
        }

        private decimal? LeerPrecio(string texto, string nombreClave)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (string.IsNullOrWhiteSpace(texto))
            {
                return null;
            }

            if (!decimal.TryParse(texto.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal valor))
            {
                throw new Exception(string.Format(
                    t.Translate("SeleccionarPropiedad.msgPrecioNumerico"),
                    t.Translate("SeleccionarPropiedad." + nombreClave)));
            }

            return valor;
        }

        private void Seleccionar()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            Propiedad577MC propiedad = null;

            if (dgvPropiedades.SelectedRows.Count > 0)
            {
                propiedad = dgvPropiedades.SelectedRows[0].DataBoundItem as Propiedad577MC;
            }

            if (propiedad == null && dgvPropiedades.CurrentRow != null)
            {
                propiedad = dgvPropiedades.CurrentRow.DataBoundItem as Propiedad577MC;
            }

            if (propiedad != null)
            {
                PropiedadSeleccionada = propiedad;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(t.Translate("SeleccionarPropiedad.msgDebeSeleccionar"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarCombos()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string tipoActual = cboTipo.SelectedValue as string;
            cboTipo.Items.Clear();
            foreach (var valor in new[] { "Casa", "Departamento", "Local", "Terreno" })
            {
                cboTipo.Items.Add(new ItemCombo { Display = t.Translate("TipoPropiedad." + valor), Valor = valor });
            }
            cboTipo.DisplayMember = "Display";
            cboTipo.ValueMember = "Valor";
            SeleccionarItemPorValor(cboTipo, tipoActual);

            string estadoActual = cboEstado.SelectedValue as string;
            cboEstado.Items.Clear();
            foreach (var valor in new[] { "Disponible", "Vendida", "Alquilada", "Reservada" })
            {
                cboEstado.Items.Add(new ItemCombo { Display = t.Translate("EstadoPropiedad." + valor), Valor = valor });
            }
            cboEstado.DisplayMember = "Display";
            cboEstado.ValueMember = "Valor";
            SeleccionarItemPorValor(cboEstado, estadoActual);
        }

        private void CargarCombosABM()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string tipoActual = cboTipoABM.SelectedValue as string;
            string estadoActual = cboEstadoABM.SelectedValue as string;

            foreach (var combo in new[] { cboTipoABM, cboEstadoABM })
            {
                combo.Items.Clear();
            }

            foreach (var valor in new[] { "Casa", "Departamento", "Local", "Terreno" })
            {
                cboTipoABM.Items.Add(new ItemCombo { Display = t.Translate("TipoPropiedad." + valor), Valor = valor });
            }
            cboTipoABM.DisplayMember = "Display";
            cboTipoABM.ValueMember = "Valor";

            foreach (var valor in new[] { "Disponible", "Vendida", "Alquilada", "Reservada" })
            {
                cboEstadoABM.Items.Add(new ItemCombo { Display = t.Translate("EstadoPropiedad." + valor), Valor = valor });
            }
            cboEstadoABM.DisplayMember = "Display";
            cboEstadoABM.ValueMember = "Valor";

            SeleccionarItemPorValor(cboTipoABM, tipoActual);
            SeleccionarItemPorValor(cboEstadoABM, estadoActual);
        }

        private void CargarABM()
        {
            Propiedad577MC propiedad = null;

            if (dgvPropiedades.SelectedRows.Count > 0)
            {
                propiedad = dgvPropiedades.SelectedRows[0].DataBoundItem as Propiedad577MC;
            }

            if (propiedad == null && dgvPropiedades.CurrentRow != null)
            {
                propiedad = dgvPropiedades.CurrentRow.DataBoundItem as Propiedad577MC;
            }

            if (propiedad == null)
            {
                return;
            }

            _idSeleccionado = propiedad.Id;
            txtDirABM.Text = propiedad.Direccion;
            txtPrecioABM.Text = propiedad.Precio.ToString("N2", CultureInfo.CurrentCulture);
            txtSuperficieABM.Text = propiedad.SuperficieM2.ToString("N2", CultureInfo.CurrentCulture);
            txtAmbientesABM.Text = propiedad.Ambientes.ToString(CultureInfo.CurrentCulture);
            txtDormitoriosABM.Text = propiedad.Dormitorios.ToString(CultureInfo.CurrentCulture);
            txtBaniosABM.Text = propiedad.Banios.ToString(CultureInfo.CurrentCulture);
            SeleccionarItemPorValor(cboTipoABM, propiedad.Tipo);
            SeleccionarItemPorValor(cboEstadoABM, propiedad.Estado);
        }

        private bool LeerCamposABM(out Propiedad577MC propiedad, out string mensajeClave)
        {
            propiedad = null;
            mensajeClave = null;

            if (string.IsNullOrWhiteSpace(txtDirABM.Text) ||
                cboTipoABM.SelectedIndex < 0 ||
                cboEstadoABM.SelectedIndex < 0)
            {
                mensajeClave = "SeleccionarPropiedad.msgCamposObligatorios";
                return false;
            }

            if (!decimal.TryParse(txtPrecioABM.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precio) ||
                !decimal.TryParse(txtSuperficieABM.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out decimal superficie))
            {
                mensajeClave = "SeleccionarPropiedad.msgNumeroInvalido";
                return false;
            }

            if (!int.TryParse(txtAmbientesABM.Text.Trim(), out int ambientes) ||
                !int.TryParse(txtDormitoriosABM.Text.Trim(), out int dormitorios) ||
                !int.TryParse(txtBaniosABM.Text.Trim(), out int banios))
            {
                mensajeClave = "SeleccionarPropiedad.msgNumeroInvalido";
                return false;
            }

            propiedad = new Propiedad577MC
            {
                Id = _idSeleccionado ?? 0,
                Direccion = txtDirABM.Text.Trim(),
                Tipo = cboTipoABM.SelectedValue as string,
                Estado = cboEstadoABM.SelectedValue as string,
                Precio = precio,
                SuperficieM2 = superficie,
                Ambientes = ambientes,
                Dormitorios = dormitorios,
                Banios = banios
            };

            return true;
        }

        private void btnGuardarProp_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!LeerCamposABM(out Propiedad577MC propiedad, out string mensajeClave))
            {
                MessageBox.Show(t.Translate(mensajeClave), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_idSeleccionado.HasValue)
                {
                    _propiedadBLL.ModificarPropiedad(propiedad);
                    MessageBox.Show(t.Translate("SeleccionarPropiedad.msgPropiedadModificada"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _propiedadBLL.RegistrarPropiedad(propiedad);
                    MessageBox.Show(t.Translate("SeleccionarPropiedad.msgPropiedadGuardada"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarABM();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificarProp_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!_idSeleccionado.HasValue)
            {
                MessageBox.Show(t.Translate("SeleccionarPropiedad.msgDebeSeleccionar"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!LeerCamposABM(out Propiedad577MC propiedad, out string mensajeClave))
            {
                MessageBox.Show(t.Translate(mensajeClave), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _propiedadBLL.ModificarPropiedad(propiedad);

                MessageBox.Show(t.Translate("SeleccionarPropiedad.msgPropiedadModificada"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarABM();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarProp_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!_idSeleccionado.HasValue)
            {
                MessageBox.Show(t.Translate("SeleccionarPropiedad.msgDebeSeleccionar"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                string.Format(t.Translate("SeleccionarPropiedad.msgConfirmarEliminar"), _idSeleccionado.Value),
                t.Translate("SeleccionarPropiedad.title"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _propiedadBLL.EliminarPropiedad(_idSeleccionado.Value);

                MessageBox.Show(t.Translate("SeleccionarPropiedad.msgPropiedadEliminada"), t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarABM();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevoProp_Click(object sender, EventArgs e)
        {
            LimpiarABM();
            txtDirABM.Focus();
        }

        private void LimpiarABM()
        {
            _idSeleccionado = null;
            txtDirABM.Clear();
            txtPrecioABM.Clear();
            txtSuperficieABM.Clear();
            txtAmbientesABM.Clear();
            txtDormitoriosABM.Clear();
            txtBaniosABM.Clear();
            cboTipoABM.SelectedIndex = -1;
            cboEstadoABM.SelectedIndex = -1;
            dgvPropiedades.ClearSelection();
        }

        private static void SeleccionarItemPorValor(ComboBox combo, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                combo.SelectedIndex = -1;
                return;
            }

            valor = valor.Trim();

            foreach (ItemCombo item in combo.Items)
            {
                if (string.Equals(item.Valor, valor, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("SeleccionarPropiedad.formTitle");
            lblDireccion.Text = t.Translate("SeleccionarPropiedad.labelDireccion");
            lblTipo.Text = t.Translate("SeleccionarPropiedad.labelTipo");
            lblEstado.Text = t.Translate("SeleccionarPropiedad.labelEstado");
            lblPrecioMin.Text = t.Translate("SeleccionarPropiedad.labelPrecioMin");
            lblPrecioMax.Text = t.Translate("SeleccionarPropiedad.labelPrecioMax");
            btnBuscar.Text = t.Translate("SeleccionarPropiedad.btnBuscar");
            btnLimpiar.Text = t.Translate("SeleccionarPropiedad.btnLimpiar");
            btnSeleccionar.Text = t.Translate("SeleccionarPropiedad.btnSeleccionar");

            gbABM.Text = t.Translate("SeleccionarPropiedad.gbABM");
            lblDirABM.Text = t.Translate("SeleccionarPropiedad.labelDireccion");
            lblTipoABM.Text = t.Translate("SeleccionarPropiedad.labelTipo");
            lblEstadoABM.Text = t.Translate("SeleccionarPropiedad.labelEstado");
            lblPrecioABM.Text = t.Translate("SeleccionarPropiedad.labelPrecio");
            lblSuperficieABM.Text = t.Translate("SeleccionarPropiedad.labelSuperficie");
            lblAmbientesABM.Text = t.Translate("SeleccionarPropiedad.labelAmbientes");
            lblDormitoriosABM.Text = t.Translate("SeleccionarPropiedad.labelDormitorios");
            lblBaniosABM.Text = t.Translate("SeleccionarPropiedad.labelBanios");
            btnGuardarProp.Text = t.Translate("SeleccionarPropiedad.btnGuardarProp");
            btnModificarProp.Text = t.Translate("SeleccionarPropiedad.btnModificarProp");
            btnEliminarProp.Text = t.Translate("SeleccionarPropiedad.btnEliminarProp");
            btnNuevoProp.Text = t.Translate("SeleccionarPropiedad.btnNuevoProp");

            colId.HeaderText = t.Translate("SeleccionarPropiedad.colCodigo");
            colDireccion.HeaderText = t.Translate("SeleccionarPropiedad.colDireccion");
            colTipo.HeaderText = t.Translate("SeleccionarPropiedad.colTipo");
            colEstado.HeaderText = t.Translate("SeleccionarPropiedad.colEstado");
            colPrecio.HeaderText = t.Translate("SeleccionarPropiedad.colPrecio");

            CargarCombos();
            CargarCombosABM();
            MostrarResultados();
        }
    }
}