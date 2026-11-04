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
    public partial class FrmGenerarContrato : Form, IIdiomaObserver
    {
        BLLContrato577MC _contratoBLL = new BLLContrato577MC();
        Propiedad577MC _propiedadSeleccionada;
        TerminosContrato577MC _terminos;

        public FrmGenerarContrato()
        {
            InitializeComponent();
            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            FormClosed += (s, e) => ServiceSessionManager577MC.getIntancia().Idioma.Desuscribir(this);
            Inicializar();
            actualizarIdioma();
        }

        private void Inicializar()
        {
            dtpFechaEmision.Value = DateTime.Now;
            LimpiarPropiedad();
            LimpiarTerminos();
        }

        private void btnSeleccionarPropiedad_Click(object sender, EventArgs e)
        {
            try
            {
                using (SeleccionarPropiedad formSel = new SeleccionarPropiedad(soloOperables: true))
                {
                    if (formSel.ShowDialog(this) == DialogResult.OK && formSel.PropiedadSeleccionada != null)
                    {
                        _propiedadSeleccionada = formSel.PropiedadSeleccionada;
                        MostrarPropiedad(_propiedadSeleccionada);

                        try
                        {
                            _contratoBLL.ValidarOperabilidadPropiedad(_propiedadSeleccionada.Id);
                        }
                        catch
                        {
                            LimpiarPropiedad();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;
                MessageBox.Show(ex.Message, t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPropiedad(Propiedad577MC propiedad)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            txtCodigoPropiedad.Text = propiedad.Id.ToString(CultureInfo.CurrentCulture);
            txtDireccion.Text = propiedad.Direccion;
            txtTipo.Text = t.Translate("TipoPropiedad." + propiedad.Tipo);
            txtEstado.Text = t.Translate("EstadoPropiedad." + propiedad.Estado);
            txtPrecio.Text = propiedad.Precio.ToString("N2", CultureInfo.CurrentCulture);
            txtSuperficie.Text = propiedad.SuperficieM2.ToString("N2", CultureInfo.CurrentCulture);
            txtAmbientes.Text = propiedad.Ambientes.ToString(CultureInfo.CurrentCulture);
            txtDormitorios.Text = propiedad.Dormitorios.ToString(CultureInfo.CurrentCulture);
            txtBanios.Text = propiedad.Banios.ToString(CultureInfo.CurrentCulture);
        }

        private void LimpiarPropiedad()
        {
            _propiedadSeleccionada = null;
            txtCodigoPropiedad.Clear();
            txtDireccion.Clear();
            txtTipo.Clear();
            txtEstado.Clear();
            txtPrecio.Clear();
            txtSuperficie.Clear();
            txtAmbientes.Clear();
            txtDormitorios.Clear();
            txtBanios.Clear();
        }

        private void btnTerminosCondiciones_Click(object sender, EventArgs e)
        {
            try
            {
                using (FrmRegistrarTerminosCondiciones form = new FrmRegistrarTerminosCondiciones(_terminos))
                {
                    if (form.ShowDialog(this) == DialogResult.OK && form.Terminos != null)
                    {
                        _terminos = form.Terminos;
                        MostrarTerminos();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;
                MessageBox.Show(ex.Message, t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarTerminos()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_terminos == null)
            {
                LimpiarTerminos();
                return;
            }

            txtTipoContrato.Text = t.Translate("TipoContrato." + _terminos.TipoContrato);
            txtFormaPago.Text = t.Translate("FormaPago." + _terminos.FormaPago);
            txtFechaInicio.Text = _terminos.FechaInicio.ToString("d", CultureInfo.CurrentCulture);
            txtFechaFin.Text = _terminos.FechaFin.ToString("d", CultureInfo.CurrentCulture);
            txtMontoAcordado.Text = _terminos.MontoAcordado.ToString("N2", CultureInfo.CurrentCulture);
            txtDepositoGarantia.Text = _terminos.DepositoGarantia.ToString("N2", CultureInfo.CurrentCulture);
        }

        private void LimpiarTerminos()
        {
            _terminos = null;
            txtTipoContrato.Clear();
            txtFormaPago.Clear();
            txtFechaInicio.Clear();
            txtFechaFin.Clear();
            txtMontoAcordado.Clear();
            txtDepositoGarantia.Clear();
        }

        private void btnRegistrarPropietario_Click(object sender, EventArgs e)
        {
            RegistrarClienteDesdeBoton(txtDNIPropietario);
        }

        private void btnRegistrarCliente_Click(object sender, EventArgs e)
        {
            RegistrarClienteDesdeBoton(txtDNICliente);
        }

        private void RegistrarClienteDesdeBoton(TextBox txtDni)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string dni = txtDni.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                MessageBox.Show(t.Translate("FrmGenerarContrato.msgIngresarDni"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (_contratoBLL.ClienteExiste(dni))
            {
                MessageBox.Show(t.Translate("FrmGenerarContrato.msgClienteYaRegistrado"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (RegistrarCliente formReg = new RegistrarCliente(dni))
            {
                if (formReg.ShowDialog(this) == DialogResult.OK && formReg.ClienteRegistrado != null)
                {
                    txtDni.Text = formReg.ClienteRegistrado.DNI;
                }
            }
        }

        private bool AsegurarCliente(TextBox txtDni, string rol)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string dni = txtDni.Text.Trim();

            if (_contratoBLL.ClienteExiste(dni))
            {
                return true;
            }

            DialogResult respuesta = MessageBox.Show(
                string.Format(t.Translate("FrmGenerarContrato.msgDniNoRegistrado"), rol, dni),
                t.Translate("FrmGenerarContrato.formTitle"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return false;
            }

            using (RegistrarCliente formReg = new RegistrarCliente(dni))
            {
                if (formReg.ShowDialog(this) == DialogResult.OK && formReg.ClienteRegistrado != null)
                {
                    txtDni.Text = formReg.ClienteRegistrado.DNI;
                }
            }

            if (_contratoBLL.ClienteExiste(txtDni.Text.Trim()))
            {
                return true;
            }

            MessageBox.Show(t.Translate("FrmGenerarContrato.msgClienteNoRegistrado"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void btnGenerarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;

                string dniPropietario = txtDNIPropietario.Text.Trim();
                string dniCliente = txtDNICliente.Text.Trim();

                if (string.IsNullOrWhiteSpace(dniPropietario))
                {
                    MessageBox.Show(t.Translate("FrmGenerarContrato.msgDebeIngresarDniPropietario"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNIPropietario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(dniCliente))
                {
                    MessageBox.Show(t.Translate("FrmGenerarContrato.msgDebeIngresarDniCliente"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNICliente.Focus();
                    return;
                }

                if (_propiedadSeleccionada == null)
                {
                    MessageBox.Show(t.Translate("FrmGenerarContrato.msgDebeSeleccionarPropiedad"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _contratoBLL.ValidarOperabilidadPropiedad(_propiedadSeleccionada.Id);

                if (!AsegurarCliente(txtDNIPropietario, t.Translate("FrmGenerarContrato.rolPropietario")))
                {
                    return;
                }

                if (!AsegurarCliente(txtDNICliente, t.Translate("FrmGenerarContrato.rolCliente")))
                {
                    return;
                }

                dniPropietario = txtDNIPropietario.Text.Trim();
                dniCliente = txtDNICliente.Text.Trim();

                if (_terminos == null)
                {
                    MessageBox.Show(FrmRegistrarTerminosCondiciones.MensajeTerminosInvalidos, t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Contrato577MC contrato = new Contrato577MC
                {
                    FechaEmision = dtpFechaEmision.Value.Date,
                    DNIPropietario = dniPropietario,
                    DNICliente = dniCliente,
                    CodigoPropiedad = _propiedadSeleccionada.Id,
                    TipoContrato = _terminos.TipoContrato,
                    FechaInicio = _terminos.FechaInicio,
                    FechaFin = _terminos.FechaFin,
                    MontoAcordado = _terminos.MontoAcordado,
                    FormaPago = _terminos.FormaPago,
                    DepositoGarantia = _terminos.DepositoGarantia,
                    Estado = "Pendiente"
                };

                _contratoBLL.ValidarYGenerarContrato(contrato);

                MessageBox.Show(t.Translate("FrmGenerarContrato.msgContratoGenerado"), t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                Limpiar();
            }
            catch (Exception ex)
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;
                MessageBox.Show(ex.Message, t.Translate("FrmGenerarContrato.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtDNIPropietario.Clear();
            txtDNICliente.Clear();
            LimpiarPropiedad();
            LimpiarTerminos();

            dtpFechaEmision.Value = DateTime.Now;
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("FrmGenerarContrato.formTitle");
            lblTitulo.Text = t.Translate("FrmGenerarContrato.formTitle");

            gbClientes.Text = t.Translate("FrmGenerarContrato.gbClientes");
            lblDNIPropietario.Text = t.Translate("FrmGenerarContrato.lblDNIPropietario");
            btnRegistrarPropietario.Text = t.Translate("FrmGenerarContrato.btnRegistrarPropietario");
            lblDNICliente.Text = t.Translate("FrmGenerarContrato.lblDNICliente");
            btnRegistrarCliente.Text = t.Translate("FrmGenerarContrato.btnRegistrarCliente");

            gbPropiedad.Text = t.Translate("FrmGenerarContrato.gbPropiedad");
            lblCodigoPropiedad.Text = t.Translate("FrmGenerarContrato.lblCodigoPropiedad");
            btnSeleccionarPropiedad.Text = t.Translate("FrmGenerarContrato.btnSeleccionarPropiedad");
            lblDireccion.Text = t.Translate("FrmGenerarContrato.lblDireccion");
            lblTipo.Text = t.Translate("FrmGenerarContrato.lblTipo");
            lblEstado.Text = t.Translate("FrmGenerarContrato.lblEstado");
            lblPrecio.Text = t.Translate("FrmGenerarContrato.lblPrecio");
            lblSuperficie.Text = t.Translate("FrmGenerarContrato.lblSuperficie");
            lblAmbientes.Text = t.Translate("FrmGenerarContrato.lblAmbientes");
            lblDormitorios.Text = t.Translate("FrmGenerarContrato.lblDormitorios");
            lblBanios.Text = t.Translate("FrmGenerarContrato.lblBanios");

            gbTerminos.Text = t.Translate("FrmGenerarContrato.gbTerminos");
            lblFechaEmision.Text = t.Translate("FrmGenerarContrato.lblFechaEmision");
            btnTerminosCondiciones.Text = t.Translate("FrmGenerarContrato.btnTerminosCondiciones");
            lblTipoContrato.Text = t.Translate("FrmGenerarContrato.lblTipoContrato");
            lblFormaPago.Text = t.Translate("FrmGenerarContrato.lblFormaPago");
            lblFechaInicio.Text = t.Translate("FrmGenerarContrato.lblFechaInicio");
            lblFechaFin.Text = t.Translate("FrmGenerarContrato.lblFechaFin");
            lblMontoAcordado.Text = t.Translate("FrmGenerarContrato.lblMontoAcordado");
            lblDepositoGarantia.Text = t.Translate("FrmGenerarContrato.lblDepositoGarantia");

            btnGenerarContrato.Text = t.Translate("FrmGenerarContrato.btnGenerarContrato");
            btnLimpiar.Text = t.Translate("FrmGenerarContrato.btnLimpiar");

            if (_propiedadSeleccionada != null)
            {
                MostrarPropiedad(_propiedadSeleccionada);
            }

            if (_terminos != null)
            {
                MostrarTerminos();
            }
        }
    }
}
