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
        internal const string MensajeTerminosInvalidos = "Debe completar todos los términos comerciales correctamente";

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
            Inicializar(terminos);
        }

        private void Inicializar(TerminosContrato577MC terminos)
        {
            cboTipoContrato.Items.Clear();
            cboTipoContrato.Items.Add("Venta");
            cboTipoContrato.Items.Add("Alquiler");

            cboFormaPago.Items.Clear();
            cboFormaPago.Items.Add("Contado");
            cboFormaPago.Items.Add("Financiado");
            cboFormaPago.Items.Add("Cuotas");

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

            int indice = combo.FindStringExact(valor);
            combo.SelectedIndex = indice >= 0 ? indice : -1;
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
                TipoContrato = cboTipoContrato.SelectedItem?.ToString() ?? string.Empty,
                FormaPago = cboFormaPago.SelectedItem?.ToString() ?? string.Empty,
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
            MessageBox.Show(MensajeTerminosInvalidos, "Términos y Condiciones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void actualizarIdioma()
        {
            // Textos en español; se mantiene el observador para futuras traducciones.
        }
    }
}
