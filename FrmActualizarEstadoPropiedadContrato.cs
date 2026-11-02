using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Servicios
{
    public partial class FrmActualizarEstadoPropiedadContrato : Form, IIdiomaObserver
    {
        private const string Titulo = "Actualizar Estado de Propiedad y Contrato";

        BLLContrato577MC _contratoBLL = new BLLContrato577MC();

        Contrato577MC _contratoSeleccionado;
        bool _cargando;

        public FrmActualizarEstadoPropiedadContrato()
        {
            InitializeComponent();

            cboNuevoEstado.Items.Clear();
            cboNuevoEstado.Items.Add("Firmado");
            cboNuevoEstado.Items.Add("Cancelado");

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
            Contrato577MC contrato = ContratoSeleccionado();

            if (contrato == null)
            {
                MessageBox.Show("Debe seleccionar un contrato pendiente de la grilla.", Titulo,
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
            try
            {
                if (_contratoSeleccionado == null)
                {
                    MessageBox.Show("Debe seleccionar un contrato pendiente de la grilla.", Titulo,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string decision = cboNuevoEstado.SelectedItem?.ToString();

                if (string.IsNullOrWhiteSpace(decision))
                {
                    MessageBox.Show("Debe seleccionar el nuevo estado del contrato.", Titulo,
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
                    MessageBox.Show("Contrato cancelado con éxito. La propiedad mantiene su disponibilidad.", Titulo,
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
            txtNroContrato.Text = contrato.NroContrato;
            txtFechaEmision.Text = contrato.FechaEmision.ToString("d", CultureInfo.CurrentCulture);
            txtEstadoActual.Text = contrato.Estado;
            txtDNIPropietario.Text = contrato.DNIPropietario;
            txtDNICliente.Text = contrato.DNICliente;
            txtCodigoPropiedad.Text = contrato.CodigoPropiedad.ToString(CultureInfo.CurrentCulture);
            txtPropiedadDireccion.Text = contrato.PropiedadDireccion;
            txtTipoContrato.Text = contrato.TipoContrato;
            txtFechaInicio.Text = contrato.FechaInicio.ToString("d", CultureInfo.CurrentCulture);
            txtFechaFin.Text = contrato.FechaFin.ToString("d", CultureInfo.CurrentCulture);
            txtMontoAcordado.Text = contrato.MontoAcordado.ToString("N2", CultureInfo.CurrentCulture);
            txtFormaPago.Text = contrato.FormaPago;
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

        public void actualizarIdioma()
        {
            // Textos en español; se mantiene el observador para futuras traducciones.
        }
    }
}
