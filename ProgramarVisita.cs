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
    public partial class ProgramarVisita : Form, IIdiomaObserver
    {
        BLLCliente577MC _clienteBLL = new BLLCliente577MC();
        BLLVisita577MC _visitaBLL = new BLLVisita577MC();

        Cliente577MC _cliente;
        Propiedad577MC _propiedad;
        bool? _disponibilidad;
        int? _ultimoIdVisita;
        Visita577MC _ultimaVisita;
        string _direccionUltimaVisita;

        public ProgramarVisita()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void ProgramarVisita_Load(object sender, EventArgs e)
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
                t.Translate("ProgramarVisita.msgPreguntaRegistrarCliente"),
                t.Translate("ProgramarVisita.msgClienteNoEncontradoTitulo"),
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
                MessageBox.Show(t.Translate("ProgramarVisita.msgDNIInvalido"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                t.Translate("ProgramarVisita.msgDetallePropiedad"),
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
                    MessageBox.Show(t.Translate("ProgramarVisita.msgBuscarCliente"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_propiedad == null)
                {
                    MessageBox.Show(t.Translate("ProgramarVisita.msgSeleccionarPropiedad"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime fecha = dtpFecha.Value.Date;
                TimeSpan horaInicio = dtpHoraInicio.Value.TimeOfDay;
                TimeSpan horaFin = dtpHoraFin.Value.TimeOfDay;

                if (horaInicio >= horaFin)
                {
                    MessageBox.Show(t.Translate("ProgramarVisita.msgHoraInvalida"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool disponible = _visitaBLL.ConsultarDisponibilidad(_propiedad.Id, fecha, horaInicio, horaFin);

                if (!disponible)
                {
                    _disponibilidad = false;
                    RefrescarLabelsInfo();

                    MessageBox.Show(t.Translate("ProgramarVisita.msgSuperposicion"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                int idVisita = _visitaBLL.ProgramarVisita(visita);

                _ultimoIdVisita = idVisita;
                RefrescarLabelsInfo();

                MessageBox.Show(t.Translate("ProgramarVisita.msgVisitaProgramada"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                _ultimaVisita = visita;
                _direccionUltimaVisita = _propiedad == null ? null : _propiedad.Direccion;

                Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void MostrarComprobanteRN1(Visita577MC visita, string direccionPropiedad)
        {
            Cliente577MC cliente = _cliente != null && !string.IsNullOrEmpty(_cliente.Nombre)
                ? _cliente
                : _clienteBLL.ObtenerPorDNI(visita.DNICliente);

            if (cliente == null)
            {
                cliente = new Cliente577MC { DNI = visita.DNICliente };
            }

            ReporteRN1577MC.Emitir(visita, cliente, direccionPropiedad);
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_ultimaVisita == null)
            {
                MessageBox.Show(t.Translate("ProgramarVisita.msgDebeConfirmarReporte"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MostrarComprobanteRN1(_ultimaVisita, _direccionUltimaVisita);
        }

        private void btnConsultarDisponibilidad_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_propiedad == null)
            {
                MessageBox.Show(t.Translate("ProgramarVisita.msgSeleccionarPropiedad"), t.Translate("ProgramarVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (ConsultarDisponibilidad form = new ConsultarDisponibilidad(_propiedad))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                dtpFecha.Value = form.FechaSeleccionada;
                dtpHoraInicio.Value = form.FechaSeleccionada.Add(form.HoraInicioSeleccionada);
                dtpHoraFin.Value = form.FechaSeleccionada.Add(form.HoraFinSeleccionada);

                _disponibilidad = form.DisponibilidadConfirmada;
                _ultimoIdVisita = null;
                RefrescarLabelsInfo();
            }
        }

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
                    lblClienteInfo.Text = string.Format(t.Translate("ProgramarVisita.msgClienteRegistrado"), _cliente.Nombre, _cliente.Apellido, _cliente.DNI);
                }
                else
                {
                    lblClienteInfo.Text = string.Format(t.Translate("ProgramarVisita.msgClienteConfirmado"), _cliente.DNI);
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
                lblClienteInfo.Text = t.Translate("ProgramarVisita.msgClienteNoExiste");
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
                lblDisponibilidad.Text = t.Translate("ProgramarVisita.msgHorarioDisponible");
            }
            else
            {
                lblDisponibilidad.ForeColor = Color.DarkRed;
                lblDisponibilidad.Text = t.Translate("ProgramarVisita.msgSuperposicion");
            }

            if (_ultimoIdVisita.HasValue)
            {
                lblResultadoRegistro.ForeColor = Color.Green;
                lblResultadoRegistro.Text = string.Format(t.Translate("ProgramarVisita.msgResultadoRegistro"), _ultimoIdVisita.Value);
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

            this.Text = t.Translate("ProgramarVisita.formTitle");
            lblTitulo.Text = t.Translate("ProgramarVisita.titulo");
            gbCliente.Text = t.Translate("ProgramarVisita.gbCliente");
            lblDNI.Text = t.Translate("ProgramarVisita.labelDNI");
            btnBuscarCliente.Text = t.Translate("ProgramarVisita.btnBuscarCliente");
            gbPropiedad.Text = t.Translate("ProgramarVisita.gbPropiedad");
            btnSeleccionarPropiedad.Text = t.Translate("ProgramarVisita.btnSeleccionarPropiedad");
            gbHorario.Text = t.Translate("ProgramarVisita.gbHorario");
            lblFecha.Text = t.Translate("ProgramarVisita.labelFecha");
            lblInicio.Text = t.Translate("ProgramarVisita.labelInicio");
            lblFin.Text = t.Translate("ProgramarVisita.labelFin");
            btnConfirmar.Text = t.Translate("ProgramarVisita.btnConfirmar");
            btnLimpiar.Text = t.Translate("ProgramarVisita.btnLimpiar");
            btnReporte.Text = t.Translate("ProgramarVisita.btnReporte");
            btnConsultarDisponibilidad.Text = t.Translate("ProgramarVisita.btnConsultarDisponibilidad");

            RefrescarLabelsInfo();
        }
    }
}