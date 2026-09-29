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

        public Propiedad577MC PropiedadSeleccionada { get; private set; }

        public SeleccionarPropiedad()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void SeleccionarPropiedad_Load(object sender, EventArgs e)
        {
            CargarCatalogo(_propiedadBLL.ObtenerTodas());
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                decimal? precioMin = LeerPrecio(txtPrecioMin.Text, "minimo");
                decimal? precioMax = LeerPrecio(txtPrecioMax.Text, "maximo");

                List<Propiedad577MC> resultado = _propiedadBLL.Buscar(
                    txtDireccion.Text,
                    cboTipo.SelectedValue as string,
                    cboEstado.SelectedValue as string,
                    precioMin,
                    precioMax);

                CargarCatalogo(resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("SeleccionarPropiedad.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtDireccion.Clear();
            cboTipo.SelectedIndex = -1;
            cboEstado.SelectedIndex = -1;
            txtPrecioMin.Clear();
            txtPrecioMax.Clear();

            CargarCatalogo(_propiedadBLL.ObtenerTodas());
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            Seleccionar();
        }

        private void dgvPropiedades_DoubleClick(object sender, EventArgs e)
        {
            Seleccionar();
        }

        private void CargarCatalogo(List<Propiedad577MC> propiedades)
        {
            dgvPropiedades.DataSource = null;
            dgvPropiedades.DataSource = propiedades;

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

        private static void SeleccionarItemPorValor(ComboBox combo, string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                combo.SelectedIndex = -1;
                return;
            }

            foreach (ItemCombo item in combo.Items)
            {
                if (item.Valor == valor)
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

            colId.HeaderText = t.Translate("SeleccionarPropiedad.colCodigo");
            colDireccion.HeaderText = t.Translate("SeleccionarPropiedad.colDireccion");
            colTipo.HeaderText = t.Translate("SeleccionarPropiedad.colTipo");
            colEstado.HeaderText = t.Translate("SeleccionarPropiedad.colEstado");
            colPrecio.HeaderText = t.Translate("SeleccionarPropiedad.colPrecio");

            CargarCombos();
            MostrarResultados();
        }
    }
}