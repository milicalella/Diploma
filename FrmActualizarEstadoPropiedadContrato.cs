using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Servicios
{
    public partial class FrmActualizarEstadoPropiedadContrato : Form, IIdiomaObserver
    {
        private static string Titulo
        {
            get
            {
                return ServiceSessionManager577MC.getIntancia().Idioma
                    .Translate("FrmActualizarEstadoPropiedadContrato.formTitle");
            }
        }

        private class ItemCombo
        {
            public string Display { get; set; }
            public string Valor { get; set; }
        }

        BLLContrato577MC _contratoBLL = new BLLContrato577MC();

        Contrato577MC _contratoSeleccionado;
        bool _cargando;

        public FrmActualizarEstadoPropiedadContrato()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            FormClosed += (s, e) => ServiceSessionManager577MC.getIntancia().Idioma.Desuscribir(this);

            actualizarIdioma();
            CargarContratos();
        }

        private void CargarContratos()
        {
            _cargando = true;

            try
            {
                List<Contrato577MC> contratos = _contratoBLL.ObtenerContratosPendientes();

                dgvContratos.DataSource = null;
                dgvContratos.DataSource = contratos;

                LimpiarDetalle();

                if (contratos.Count > 0)
                {
                    dgvContratos.ClearSelection();
                    dgvContratos.Rows[0].Selected = true;
                }

                btnModificarEstado.Enabled = contratos.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnModificarEstado.Enabled = false;
            }
            finally
            {
                _cargando = false;
            }
        }

        private void dgvContratos_SelectionChanged(object sender, EventArgs e)
        {
            if (_cargando)
            {
                return;
            }

            LimpiarDetalle();
            btnModificarEstado.Enabled = dgvContratos.CurrentRow != null;
        }

        private void btnModificarEstado_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            Contrato577MC contrato = ContratoSeleccionado();

            if (contrato == null)
            {
                MessageBox.Show(t.Translate("FrmActualizarEstadoPropiedadContrato.msgDebeSeleccionarContrato"), Titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _contratoSeleccionado = contrato;
            MostrarDetalle(contrato);

            cboNuevoEstado.SelectedIndex = -1;
            cboNuevoEstado.Enabled = true;
            btnGuardar.Enabled = false;
        }

        private void cboNuevoEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGuardar.Enabled = _contratoSeleccionado != null && cboNuevoEstado.SelectedIndex >= 0;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                if (_contratoSeleccionado == null)
                {
                    MessageBox.Show(t.Translate("FrmActualizarEstadoPropiedadContrato.msgDebeSeleccionarContrato"), Titulo,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string decision = ValorSeleccionado(cboNuevoEstado);

                if (string.IsNullOrWhiteSpace(decision))
                {
                    MessageBox.Show(t.Translate("FrmActualizarEstadoPropiedadContrato.msgDebeSeleccionarNuevoEstado"), Titulo,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNuevoEstado.Focus();
                    return;
                }

                _contratoBLL.ActualizarEstadoContrato(_contratoSeleccionado.NroContrato, decision);

                if (string.Equals(decision, "Firmado", StringComparison.OrdinalIgnoreCase))
                {
                    _contratoSeleccionado.Estado = "Firmado";
                    ReporteRN2577MC.Emitir(_contratoSeleccionado);
                }
                else
                {
                    MessageBox.Show(t.Translate("FrmActualizarEstadoPropiedadContrato.msgContratoCancelado"), Titulo,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                CargarContratos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Contrato577MC ContratoSeleccionado()
        {
            Contrato577MC contrato = null;

            if (dgvContratos.SelectedRows.Count > 0)
            {
                contrato = dgvContratos.SelectedRows[0].DataBoundItem as Contrato577MC;
            }

            if (contrato == null && dgvContratos.CurrentRow != null)
            {
                contrato = dgvContratos.CurrentRow.DataBoundItem as Contrato577MC;
            }

            return contrato;
        }

        private void MostrarDetalle(Contrato577MC contrato)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            txtNroContrato.Text = contrato.NroContrato;
            txtFechaEmision.Text = contrato.FechaEmision.ToString("d", CultureInfo.CurrentCulture);
            txtEstadoActual.Text = t.Translate("EstadoContrato." + contrato.Estado);
            txtDNIPropietario.Text = contrato.DNIPropietario;
            txtDNICliente.Text = contrato.DNICliente;
            txtCodigoPropiedad.Text = contrato.CodigoPropiedad.ToString(CultureInfo.CurrentCulture);
            txtPropiedadDireccion.Text = contrato.PropiedadDireccion;
            txtTipoContrato.Text = t.Translate("TipoContrato." + contrato.TipoContrato);
            txtFechaInicio.Text = contrato.FechaInicio.ToString("d", CultureInfo.CurrentCulture);
            txtFechaFin.Text = contrato.FechaFin.ToString("d", CultureInfo.CurrentCulture);
            txtMontoAcordado.Text = contrato.MontoAcordado.ToString("N2", CultureInfo.CurrentCulture);
            txtFormaPago.Text = t.Translate("FormaPago." + contrato.FormaPago);
            txtDepositoGarantia.Text = contrato.DepositoGarantia.ToString("N2", CultureInfo.CurrentCulture);
        }

        private void LimpiarDetalle()
        {
            _contratoSeleccionado = null;

            txtNroContrato.Clear();
            txtFechaEmision.Clear();
            txtEstadoActual.Clear();
            txtDNIPropietario.Clear();
            txtDNICliente.Clear();
            txtCodigoPropiedad.Clear();
            txtPropiedadDireccion.Clear();
            txtTipoContrato.Clear();
            txtFechaInicio.Clear();
            txtFechaFin.Clear();
            txtMontoAcordado.Clear();
            txtFormaPago.Clear();
            txtDepositoGarantia.Clear();

            cboNuevoEstado.SelectedIndex = -1;
            cboNuevoEstado.Enabled = false;
            btnGuardar.Enabled = false;
        }

        private void dgvContratos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null)
            {
                return;
            }

            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (dgvContratos.Columns[e.ColumnIndex] == colTipoContrato)
            {
                e.Value = t.Translate("TipoContrato." + e.Value);
                e.FormattingApplied = true;
            }
            else if (dgvContratos.Columns[e.ColumnIndex] == colEstado)
            {
                e.Value = t.Translate("EstadoContrato." + e.Value);
                e.FormattingApplied = true;
            }
        }

        private void CargarComboEstado()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string estadoActual = ValorSeleccionado(cboNuevoEstado);

            cboNuevoEstado.DataSource = new BindingList<ItemCombo>
            {
                new ItemCombo { Display = t.Translate("EstadoContrato.Firmado"), Valor = "Firmado" },
                new ItemCombo { Display = t.Translate("EstadoContrato.Cancelado"), Valor = "Cancelado" }
            };
            cboNuevoEstado.DisplayMember = "Display";
            cboNuevoEstado.ValueMember = "Valor";
            SeleccionarItemPorValor(cboNuevoEstado, estadoActual);
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

        private static void SeleccionarItemPorValor(ComboBox combo, string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                combo.SelectedIndex = -1;
                return;
            }

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

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.formTitle");
            lblTitulo.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.formTitle");

            gbContratos.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.gbContratos");
            colNroContrato.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colNroContrato");
            colFechaEmision.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colFechaEmision");
            colDNICliente.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colDNICliente");
            colPropiedadDireccion.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colPropiedadDireccion");
            colTipoContrato.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colTipoContrato");
            colFechaInicio.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colFechaInicio");
            colFechaFin.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colFechaFin");
            colMontoAcordado.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colMontoAcordado");
            colEstado.HeaderText = t.Translate("FrmActualizarEstadoPropiedadContrato.colEstado");

            gbDetalle.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.gbDetalle");
            lblNroContrato.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblNroContrato");
            lblFechaEmision.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblFechaEmision");
            lblEstadoActual.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblEstadoActual");
            lblDNIPropietario.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblDNIPropietario");
            lblDNICliente.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblDNICliente");
            lblCodigoPropiedad.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblCodigoPropiedad");
            lblPropiedadDireccion.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblPropiedadDireccion");
            lblTipoContrato.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblTipoContrato");
            lblFechaInicio.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblFechaInicio");
            lblFechaFin.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblFechaFin");
            lblMontoAcordado.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblMontoAcordado");
            lblFormaPago.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblFormaPago");
            lblDepositoGarantia.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblDepositoGarantia");

            gbDecision.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.gbDecision");
            lblNuevoEstado.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.lblNuevoEstado");
            btnModificarEstado.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.btnModificarEstado");
            btnGuardar.Text = t.Translate("FrmActualizarEstadoPropiedadContrato.btnGuardar");

            CargarComboEstado();

            if (_contratoSeleccionado != null)
            {
                MostrarDetalle(_contratoSeleccionado);
            }

            dgvContratos.Invalidate();
        }
    }
}
