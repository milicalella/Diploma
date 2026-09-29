using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Servicios
{
    public partial class RegistrarVisita : Form, IIdiomaObserver
    {
        BLLCliente577MC _clienteBLL = new BLLCliente577MC();
        BLLVisita577MC _visitaBLL = new BLLVisita577MC();

        Cliente577MC _cliente;
        Propiedad577MC _propiedad;
        bool? _disponibilidad;
        int? _ultimoIdVisita;

        public RegistrarVisita()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void RegistrarVisita_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;
            dtpHoraInicio.Value = DateTime.Today.AddHours(10);
            dtpHoraFin.Value = DateTime.Today.AddHours(11);
            Limpiar();
        }

        #region Cliente

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            BuscarOCrearCliente();
        }

        private void txtDNI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BuscarOCrearCliente();
            }
        }

        private void BuscarOCrearCliente()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string dni = txtDNI.Text.Trim();

            if (!ValidarDNI(dni))
            {
                txtDNI.Focus();
                return;
            }

            if (_clienteBLL.ExisteCliente(dni))
            {
                _cliente = new Cliente577MC { DNI = dni };
                RefrescarLabelsInfo();
                return;
            }

            _cliente = null;
            RefrescarLabelsInfo();

            DialogResult respuesta = MessageBox.Show(
                t.Translate("RegistrarVisita.msgPreguntaRegistrarCliente"),
                t.Translate("RegistrarVisita.msgClienteNoEncontradoTitulo"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                AbrirAltaCliente(dni);
            }
            else
            {
                txtDNI.Focus();
                txtDNI.SelectAll();
            }
        }

        private void AbrirAltaCliente(string dni)
        {
            using (RegistrarCliente form = new RegistrarCliente(dni))
            {
                if (form.ShowDialog() == DialogResult.OK && form.ClienteRegistrado != null)
                {
                    _cliente = form.ClienteRegistrado;
                    txtDNI.Text = _cliente.DNI;
                    RefrescarLabelsInfo();
                }
                else
                {
                    txtDNI.Focus();
                    txtDNI.SelectAll();
                }
            }
        }

        private bool ValidarDNI(string dni)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (dni.Length < 7 || dni.Length > 8 || dni.Any(c => !char.IsDigit(c)))
            {
                MessageBox.Show(t.Translate("RegistrarVisita.msgDNIInvalido"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion

        #region Propiedad

        private void btnSeleccionarPropiedad_Click(object sender, EventArgs e)
        {
            using (SeleccionarPropiedad form = new SeleccionarPropiedad())
            {
                if (form.ShowDialog() == DialogResult.OK && form.PropiedadSeleccionada != null)
                {
                    _propiedad = form.PropiedadSeleccionada;
                    _disponibilidad = null;
                    _ultimoIdVisita = null;
                    RefrescarLabelsInfo();
                }
            }
        }

        private string ObtenerDetallePropiedad()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            return string.Format(
                t.Translate("RegistrarVisita.msgDetallePropiedad"),
                _propiedad.Direccion,
                t.Translate("TipoPropiedad." + _propiedad.Tipo),
                t.Translate("EstadoPropiedad." + _propiedad.Estado),
                _propiedad.Precio);
        }

        #endregion

        #region Confirmación

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                if (_cliente == null)
                {
                    MessageBox.Show(t.Translate("RegistrarVisita.msgBuscarCliente"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_propiedad == null)
                {
                    MessageBox.Show(t.Translate("RegistrarVisita.msgSeleccionarPropiedad"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime fecha = dtpFecha.Value.Date;
                TimeSpan horaInicio = dtpHoraInicio.Value.TimeOfDay;
                TimeSpan horaFin = dtpHoraFin.Value.TimeOfDay;

                if (horaInicio >= horaFin)
                {
                    MessageBox.Show(t.Translate("RegistrarVisita.msgHoraInvalida"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool disponible = _visitaBLL.ConsultarDisponibilidad(_propiedad.Id, fecha, horaInicio, horaFin);

                if (!disponible)
                {
                    _disponibilidad = false;
                    RefrescarLabelsInfo();

                    MessageBox.Show(t.Translate("RegistrarVisita.msgSuperposicion"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _disponibilidad = true;
                RefrescarLabelsInfo();

                Visita577MC visita = new Visita577MC
                {
                    IdPropiedad = _propiedad.Id,
                    DNICliente = _cliente.DNI,
                    Fecha = fecha,
                    HoraInicio = horaInicio,
                    HoraFin = horaFin,
                    Estado = "Pendiente"
                };

                int idVisita = _visitaBLL.RegistrarVisita(visita);

                _ultimoIdVisita = idVisita;
                RefrescarLabelsInfo();

                MessageBox.Show(t.Translate("RegistrarVisita.msgVisitaRegistrada"), t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("RegistrarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            _cliente = null;
            _propiedad = null;
            _disponibilidad = null;
            _ultimoIdVisita = null;

            txtDNI.Clear();

            dtpFecha.Value = DateTime.Today;
            dtpHoraInicio.Value = DateTime.Today.AddHours(10);
            dtpHoraFin.Value = DateTime.Today.AddHours(11);

            RefrescarLabelsInfo();
        }

        private void RefrescarLabelsInfo()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_cliente != null)
            {
                lblClienteInfo.ForeColor = Color.Green;

                if (!string.IsNullOrEmpty(_cliente.Nombre))
                {
                    lblClienteInfo.Text = string.Format(t.Translate("RegistrarVisita.msgClienteRegistrado"), _cliente.Nombre, _cliente.Apellido, _cliente.DNI);
                }
                else
                {
                    lblClienteInfo.Text = string.Format(t.Translate("RegistrarVisita.msgClienteConfirmado"), _cliente.DNI);
                }
            }
            else if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                lblClienteInfo.ForeColor = SystemColors.ControlText;
                lblClienteInfo.Text = "-";
            }
            else
            {
                lblClienteInfo.ForeColor = Color.DarkRed;
                lblClienteInfo.Text = t.Translate("RegistrarVisita.msgClienteNoExiste");
            }

            if (_propiedad != null)
            {
                lblPropiedadInfo.ForeColor = SystemColors.ControlText;
                lblPropiedadInfo.Text = ObtenerDetallePropiedad();
            }
            else
            {
                lblPropiedadInfo.ForeColor = SystemColors.ControlText;
                lblPropiedadInfo.Text = "-";
            }

            if (_disponibilidad == null)
            {
                lblDisponibilidad.ForeColor = SystemColors.ControlText;
                lblDisponibilidad.Text = "-";
            }
            else if (_disponibilidad.Value)
            {
                lblDisponibilidad.ForeColor = Color.Green;
                lblDisponibilidad.Text = t.Translate("RegistrarVisita.msgHorarioDisponible");
            }
            else
            {
                lblDisponibilidad.ForeColor = Color.DarkRed;
                lblDisponibilidad.Text = t.Translate("RegistrarVisita.msgSuperposicion");
            }

            if (_ultimoIdVisita.HasValue)
            {
                lblResultadoRegistro.ForeColor = Color.Green;
                lblResultadoRegistro.Text = string.Format(t.Translate("RegistrarVisita.msgResultadoRegistro"), _ultimoIdVisita.Value);
            }
            else
            {
                lblResultadoRegistro.ForeColor = SystemColors.ControlText;
                lblResultadoRegistro.Text = "-";
            }
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("RegistrarVisita.formTitle");
            lblTitulo.Text = t.Translate("RegistrarVisita.titulo");
            gbCliente.Text = t.Translate("RegistrarVisita.gbCliente");
            lblDNI.Text = t.Translate("RegistrarVisita.labelDNI");
            btnBuscarCliente.Text = t.Translate("RegistrarVisita.btnBuscarCliente");
            gbPropiedad.Text = t.Translate("RegistrarVisita.gbPropiedad");
            btnSeleccionarPropiedad.Text = t.Translate("RegistrarVisita.btnSeleccionarPropiedad");
            gbHorario.Text = t.Translate("RegistrarVisita.gbHorario");
            lblFecha.Text = t.Translate("RegistrarVisita.labelFecha");
            lblInicio.Text = t.Translate("RegistrarVisita.labelInicio");
            lblFin.Text = t.Translate("RegistrarVisita.labelFin");
            btnConfirmar.Text = t.Translate("RegistrarVisita.btnConfirmar");
            btnLimpiar.Text = t.Translate("RegistrarVisita.btnLimpiar");

            RefrescarLabelsInfo();
        }
    }
}