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
                using (SeleccionarPropiedad formSel = new SeleccionarPropiedad())
                {
                    if (formSel.ShowDialog(this) == DialogResult.OK && formSel.PropiedadSeleccionada != null)
                    {
                        _propiedadSeleccionada = formSel.PropiedadSeleccionada;
                        MostrarPropiedad(_propiedadSeleccionada);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPropiedad(Propiedad577MC propiedad)
        {
            txtCodigoPropiedad.Text = propiedad.Id.ToString(CultureInfo.CurrentCulture);
            txtDireccion.Text = propiedad.Direccion;
            txtTipo.Text = propiedad.Tipo;
            txtEstado.Text = propiedad.Estado;
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
                MessageBox.Show(ex.Message, "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarTerminos()
        {
            if (_terminos == null)
            {
                LimpiarTerminos();
                return;
            }

            txtTipoContrato.Text = _terminos.TipoContrato;
            txtFormaPago.Text = _terminos.FormaPago;
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
            string dni = txtDni.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                MessageBox.Show("Ingrese el DNI antes de registrar el cliente.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            if (_contratoBLL.ClienteExiste(dni))
            {
                MessageBox.Show("El cliente ya se encuentra registrado.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string dni = txtDni.Text.Trim();

            if (_contratoBLL.ClienteExiste(dni))
            {
                return true;
            }

            DialogResult respuesta = MessageBox.Show(
                string.Format("El DNI del {0} ({1}) no se encuentra registrado en el sistema. ¿Desea registrar el cliente ahora?", rol, dni),
                "Generar Contrato",
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

            MessageBox.Show("El cliente no quedó registrado. No se puede generar el contrato.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void btnGenerarContrato_Click(object sender, EventArgs e)
        {
            try
            {
                string dniPropietario = txtDNIPropietario.Text.Trim();
                string dniCliente = txtDNICliente.Text.Trim();

                if (string.IsNullOrWhiteSpace(dniPropietario))
                {
                    MessageBox.Show("Debe ingresar el DNI del propietario.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNIPropietario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(dniCliente))
                {
                    MessageBox.Show("Debe ingresar el DNI del cliente.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDNICliente.Focus();
                    return;
                }

                if (!AsegurarCliente(txtDNIPropietario, "propietario"))
                {
                    return;
                }

                if (!AsegurarCliente(txtDNICliente, "cliente"))
                {
                    return;
                }

                dniPropietario = txtDNIPropietario.Text.Trim();
                dniCliente = txtDNICliente.Text.Trim();

                if (_propiedadSeleccionada == null)
                {
                    MessageBox.Show("Debe seleccionar una propiedad válida.", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_terminos == null)
                {
                    MessageBox.Show(FrmRegistrarTerminosCondiciones.MensajeTerminosInvalidos, "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                MessageBox.Show("Contrato generado con éxito (borrador emitido)", "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Generar Contrato", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // El formulario usa textos en español; se mantiene el observer para futuras traducciones.
        }
    }
}
