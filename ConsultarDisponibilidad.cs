using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Servicios
{
    public partial class ConsultarDisponibilidad : Form, IIdiomaObserver
    {
        private readonly Propiedad577MC _propiedad;
        private readonly BLLVisita577MC _visitaBLL = new BLLVisita577MC();

        private bool _consultado;
        private bool _disponible;

        public ConsultarDisponibilidad(Propiedad577MC propiedad)
        {
            if (propiedad == null)
            {
                throw new ArgumentNullException("propiedad");
            }

            InitializeComponent();

            _propiedad = propiedad;

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        public DateTime FechaSeleccionada
        {
            get { return dtpFecha.Value.Date; }
        }

        public TimeSpan HoraInicioSeleccionada
        {
            get { return dtpHoraInicio.Value.TimeOfDay; }
        }

        public TimeSpan HoraFinSeleccionada
        {
            get { return dtpHoraFin.Value.TimeOfDay; }
        }

        public bool DisponibilidadConfirmada
        {
            get { return _consultado && _disponible; }
        }

        private void ConsultarDisponibilidad_Load(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Today;
            dtpHoraInicio.Value = DateTime.Today.AddHours(10);
            dtpHoraFin.Value = DateTime.Today.AddHours(11);

            ConsultarVisitasDelDia();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            DateTime fecha = dtpFecha.Value.Date;
            TimeSpan inicio = dtpHoraInicio.Value.TimeOfDay;
            TimeSpan fin = dtpHoraFin.Value.TimeOfDay;

            if (inicio >= fin)
            {
                MessageBox.Show(t.Translate("VisitaException.msgHoraInvalida"), t.Translate("ConsultarDisponibilidad.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _disponible = _visitaBLL.ConsultarDisponibilidad(_propiedad.Id, fecha, inicio, fin);
                _consultado = true;

                MostrarResultado();
                ConsultarVisitasDelDia();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("ConsultarDisponibilidad.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            ConsultarVisitasDelDia();
        }

        private void dgvVisitas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvVisitas.Columns[e.ColumnIndex] == colEstado && e.Value != null)
            {
                e.Value = ServiceSessionManager577MC.getIntancia().Idioma.Translate("VisitaEstado." + e.Value);
                e.FormattingApplied = true;
            }
        }

        private void MostrarResultado()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_disponible)
            {
                lblResultado.ForeColor = Color.Green;
                lblResultado.Text = t.Translate("ConsultarDisponibilidad.msgDisponible");
            }
            else
            {
                lblResultado.ForeColor = Color.Red;
                lblResultado.Text = t.Translate("ConsultarDisponibilidad.msgNoDisponible");
            }
        }

        private void ConsultarVisitasDelDia()
        {
            try
            {
                List<Visita577MC> visitas = _visitaBLL.ObtenerVisitasPorPropiedadYFecha(_propiedad.Id, dtpFecha.Value.Date);

                dgvVisitas.DataSource = null;
                dgvVisitas.DataSource = visitas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(ServiceSessionManager577MC.getIntancia().Idioma.Translate("ConsultarDisponibilidad.msgErrorVisitas"), ex.Message),
                    ServiceSessionManager577MC.getIntancia().Idioma.Translate("ConsultarDisponibilidad.formTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!_consultado)
            {
                MessageBox.Show(t.Translate("ConsultarDisponibilidad.msgDebeConsultar"), t.Translate("ConsultarDisponibilidad.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_disponible)
            {
                MessageBox.Show(t.Translate("ConsultarDisponibilidad.msgNoDisponibleAceptar"), t.Translate("ConsultarDisponibilidad.formTitle"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("ConsultarDisponibilidad.formTitle");
            lblTitulo.Text = t.Translate("ConsultarDisponibilidad.formTitle");
            gbPropiedad.Text = t.Translate("ConsultarDisponibilidad.gbPropiedad");
            gbHorario.Text = t.Translate("ConsultarDisponibilidad.gbHorario");
            lblFecha.Text = t.Translate("ConsultarDisponibilidad.labelFecha");
            lblInicio.Text = t.Translate("ConsultarDisponibilidad.labelInicio");
            lblFin.Text = t.Translate("ConsultarDisponibilidad.labelFin");
            btnConsultar.Text = t.Translate("ConsultarDisponibilidad.btnConsultar");
            btnAceptar.Text = t.Translate("ConsultarDisponibilidad.btnAceptar");
            btnCancelar.Text = t.Translate("ConsultarDisponibilidad.btnCancelar");
            lblTituloVisitas.Text = t.Translate("ConsultarDisponibilidad.labelVisitasDelDia");
            colIdVisita.HeaderText = t.Translate("ActualizarEstadoVisita.colIdVisita");
            colHorario.HeaderText = t.Translate("ConsultarDisponibilidad.colHorario");
            colEstado.HeaderText = t.Translate("ConsultarDisponibilidad.colEstado");

            lblPropiedadInfo.Text = ObtenerDetallePropiedad();

            if (_consultado)
            {
                MostrarResultado();
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
    }
}