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
    public partial class FrmRegistrarTerminosCondiciones : Form, IIdiomaObserver
    {
        private class ItemCombo
        {
            public string Display { get; set; }
            public string Valor { get; set; }
        }

        internal static string MensajeTerminosInvalidos
        {
            get
            {
                return ServiceSessionManager577MC.getIntancia().Idioma
                    .Translate("FrmRegistrarTerminosCondiciones.msgTerminosInvalidos");
            }
        }

        BLLContrato577MC _contratoBLL = new BLLContrato577MC();

        public TerminosContrato577MC Terminos { get; private set; }

        public FrmRegistrarTerminosCondiciones()
            : this(null)
        {
        }

        public FrmRegistrarTerminosCondiciones(TerminosContrato577MC terminos)
        {
            InitializeComponent();
            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            FormClosed += (s, e) => ServiceSessionManager577MC.getIntancia().Idioma.Desuscribir(this);
            actualizarIdioma();
            Inicializar(terminos);
        }

        private void Inicializar(TerminosContrato577MC terminos)
        {
            if (terminos == null)
            {
                cboTipoContrato.SelectedIndex = -1;
                cboFormaPago.SelectedIndex = -1;
                dtpFechaInicio.Value = DateTime.Today;
                dtpFechaFin.Value = DateTime.Today.AddMonths(1);
                txtMontoAcordado.Clear();
                txtDepositoGarantia.Clear();
                return;
            }

            SeleccionarItem(cboTipoContrato, terminos.TipoContrato);
            SeleccionarItem(cboFormaPago, terminos.FormaPago);

            dtpFechaInicio.Value = terminos.FechaInicio == DateTime.MinValue ? DateTime.Today : terminos.FechaInicio;
            dtpFechaFin.Value = terminos.FechaFin == DateTime.MinValue ? DateTime.Today.AddMonths(1) : terminos.FechaFin;

            txtMontoAcordado.Text = terminos.MontoAcordado.ToString("0.00", CultureInfo.CurrentCulture);
            txtDepositoGarantia.Text = terminos.DepositoGarantia.ToString("0.00", CultureInfo.CurrentCulture);
        }

        private static void SeleccionarItem(ComboBox combo, string valor)
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

            combo.SelectedIndex = -1;
        }

        private void CargarCombos()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string tipoActual = ValorSeleccionado(cboTipoContrato);
            cboTipoContrato.DataSource = new BindingList<ItemCombo>
            {
                new ItemCombo { Display = t.Translate("TipoContrato.Venta"), Valor = "Venta" },
                new ItemCombo { Display = t.Translate("TipoContrato.Alquiler"), Valor = "Alquiler" }
            };
            cboTipoContrato.DisplayMember = "Display";
            cboTipoContrato.ValueMember = "Valor";
            SeleccionarItem(cboTipoContrato, tipoActual);

            string formaPagoActual = ValorSeleccionado(cboFormaPago);
            cboFormaPago.DataSource = new BindingList<ItemCombo>
            {
                new ItemCombo { Display = t.Translate("FormaPago.Contado"), Valor = "Contado" },
                new ItemCombo { Display = t.Translate("FormaPago.Financiado"), Valor = "Financiado" },
                new ItemCombo { Display = t.Translate("FormaPago.Cuotas"), Valor = "Cuotas" }
            };
            cboFormaPago.DisplayMember = "Display";
            cboFormaPago.ValueMember = "Valor";
            SeleccionarItem(cboFormaPago, formaPagoActual);
        }

        private static string ValorSeleccionado(ComboBox combo)
        {
            string valor = combo.SelectedValue as string;

            if (string.IsNullOrEmpty(valor) && combo.SelectedItem is ItemCombo item)
            {
                valor = item.Valor;
            }

            return valor;
        }

        private void btnConfirmarTerminos_Click(object sender, EventArgs e)
        {
            if (!TryParseDecimal(txtMontoAcordado.Text, out decimal monto) ||
                !TryParseDecimal(txtDepositoGarantia.Text, out decimal deposito))
            {
                MostrarMensajeTerminosInvalidos();
                return;
            }

            TerminosContrato577MC terminos = new TerminosContrato577MC
            {
                TipoContrato = ValorSeleccionado(cboTipoContrato) ?? string.Empty,
                FormaPago = ValorSeleccionado(cboFormaPago) ?? string.Empty,
                FechaInicio = dtpFechaInicio.Value.Date,
                FechaFin = dtpFechaFin.Value.Date,
                MontoAcordado = monto,
                DepositoGarantia = deposito
            };

            try
            {
                _contratoBLL.ValidarTerminos(terminos);
            }
            catch
            {
                MostrarMensajeTerminosInvalidos();
                return;
            }

            Terminos = terminos;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool TryParseDecimal(string texto, out decimal valor)
        {
            string limpio = (texto ?? string.Empty).Trim();

            return decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.CurrentCulture, out valor)
                || decimal.TryParse(limpio, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }

        private void MostrarMensajeTerminosInvalidos()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            MessageBox.Show(
                t.Translate("FrmRegistrarTerminosCondiciones.msgTerminosInvalidos"),
                t.Translate("FrmRegistrarTerminosCondiciones.formTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("FrmRegistrarTerminosCondiciones.formTitle");
            lblTitulo.Text = t.Translate("FrmRegistrarTerminosCondiciones.formTitle");
            gbTerminos.Text = t.Translate("FrmRegistrarTerminosCondiciones.gbTerminos");
            lblTipoContrato.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblTipoContrato");
            lblFormaPago.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblFormaPago");
            lblFechaInicio.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblFechaInicio");
            lblFechaFin.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblFechaFin");
            lblMontoAcordado.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblMontoAcordado");
            lblDepositoGarantia.Text = t.Translate("FrmRegistrarTerminosCondiciones.lblDepositoGarantia");
            btnConfirmarTerminos.Text = t.Translate("FrmRegistrarTerminosCondiciones.btnConfirmarTerminos");
            btnCancelar.Text = t.Translate("FrmRegistrarTerminosCondiciones.btnCancelar");

            CargarCombos();
        }
    }
}
