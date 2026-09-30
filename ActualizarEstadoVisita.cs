using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Servicios
{
    public partial class ActualizarEstadoVisita : Form, IIdiomaObserver
    {
        private class ItemCombo
        {
            public string Display { get; set; }
            public string Valor { get; set; }
        }

        BLLVisita577MC _visitaBLL = new BLLVisita577MC();

        bool _cargando;

        public ActualizarEstadoVisita()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void ActualizarEstadoVisita_Load(object sender, EventArgs e)
        {
            _cargando = true;
            try
            {
                cboFiltroEstado.SelectedIndex = 0;
                cboNuevoEstado.SelectedIndex = 0;
                CargarConFiltro();
            }
            finally
            {
                _cargando = false;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarConFiltro();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cboFiltroEstado.SelectedIndex = 0;
            CargarConFiltro();
        }

        private void dgvVisitas_SelectionChanged(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_cargando)
            {
                return;
            }

            Visita577MC visita = VisitaSeleccionada();

            if (visita == null)
            {
                return;
            }

            if (visita.EsFinalizada)
            {
                gbActualizar.Enabled = false;
                MessageBox.Show(
                    string.Format(t.Translate("ActualizarEstadoVisita.msgFinalizada"), t.Translate("VisitaEstado." + visita.Estado)),
                    t.Translate("ActualizarEstadoVisita.title"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                gbActualizar.Enabled = true;
                cboNuevoEstado.SelectedIndex = 0;
                txtObservaciones.Clear();
            }
        }

        private void dgvVisitas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvVisitas.Columns[e.ColumnIndex] == colEstado && e.Value != null)
            {
                e.Value = ServiceSessionManager577MC.getIntancia().Idioma.Translate("VisitaEstado." + e.Value);
                e.FormattingApplied = true;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                Visita577MC visita = VisitaSeleccionada();

                if (visita == null)
                {
                    MessageBox.Show(t.Translate("ActualizarEstadoVisita.msgDebeSeleccionar"), t.Translate("ActualizarEstadoVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _visitaBLL.ActualizarEstadoVisita(visita.IdVisita, ValorSeleccionado(cboNuevoEstado), txtObservaciones.Text);

                MessageBox.Show(t.Translate("ActualizarEstadoVisita.msgEstadoActualizado"), t.Translate("ActualizarEstadoVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarConFiltro();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("ActualizarEstadoVisita.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Visita577MC VisitaSeleccionada()
        {
            Visita577MC visita = null;

            if (dgvVisitas.SelectedRows.Count > 0)
            {
                visita = dgvVisitas.SelectedRows[0].DataBoundItem as Visita577MC;
            }

            if (visita == null && dgvVisitas.CurrentRow != null)
            {
                visita = dgvVisitas.CurrentRow.DataBoundItem as Visita577MC;
            }

            return visita;
        }

        private void CargarConFiltro()
        {
            _cargando = true;
            try
            {
                string estadoFiltro = ValorSeleccionado(cboFiltroEstado);

                List<Visita577MC> visitas = _visitaBLL.ObtenerVisitas(string.IsNullOrEmpty(estadoFiltro) ? null : estadoFiltro);

                dgvVisitas.DataSource = null;
                dgvVisitas.DataSource = visitas;

                gbActualizar.Enabled = false;

                if (visitas.Count > 0)
                {
                    dgvVisitas.ClearSelection();
                    dgvVisitas.Rows[0].Selected = true;
                }
            }
            finally
            {
                _cargando = false;
            }
        }

        private void CargarCombos()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            string filtroActual = ValorSeleccionado(cboFiltroEstado);
            var itemsFiltro = new List<ItemCombo>
            {
                new ItemCombo { Display = t.Translate("Comun.todos"), Valor = "" }
            };
            foreach (var valor in new[] { "Pendiente", "Realizada", "Cancelada" })
            {
                itemsFiltro.Add(new ItemCombo { Display = t.Translate("VisitaEstado." + valor), Valor = valor });
            }
            cboFiltroEstado.DataSource = new BindingList<ItemCombo>(itemsFiltro);
            cboFiltroEstado.DisplayMember = "Display";
            cboFiltroEstado.ValueMember = "Valor";
            SeleccionarItemPorValor(cboFiltroEstado, filtroActual);

            string nuevoActual = ValorSeleccionado(cboNuevoEstado);
            var itemsNuevo = new List<ItemCombo>();
            foreach (var valor in new[] { "Realizada", "Cancelada" })
            {
                itemsNuevo.Add(new ItemCombo { Display = t.Translate("VisitaEstado." + valor), Valor = valor });
            }
            cboNuevoEstado.DataSource = new BindingList<ItemCombo>(itemsNuevo);
            cboNuevoEstado.DisplayMember = "Display";
            cboNuevoEstado.ValueMember = "Valor";
            SeleccionarItemPorValor(cboNuevoEstado, nuevoActual);
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

            valor = valor.Trim();

            foreach (ItemCombo item in combo.Items)
            {
                if (string.Equals(item.Valor, valor, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("ActualizarEstadoVisita.formTitle");
            lblFiltroEstado.Text = t.Translate("ActualizarEstadoVisita.labelFiltroEstado");
            btnBuscar.Text = t.Translate("ActualizarEstadoVisita.btnBuscar");
            btnLimpiar.Text = t.Translate("ActualizarEstadoVisita.btnLimpiar");
            gbActualizar.Text = t.Translate("ActualizarEstadoVisita.gbActualizar");
            lblEstadoNuevo.Text = t.Translate("ActualizarEstadoVisita.labelEstadoNuevo");
            lblObservaciones.Text = t.Translate("ActualizarEstadoVisita.labelObservaciones");
            btnGuardar.Text = t.Translate("ActualizarEstadoVisita.btnGuardar");

            colIdVisita.HeaderText = t.Translate("ActualizarEstadoVisita.colIdVisita");
            colDireccion.HeaderText = t.Translate("ActualizarEstadoVisita.colDireccion");
            colDNI.HeaderText = t.Translate("ActualizarEstadoVisita.colDNICliente");
            colFecha.HeaderText = t.Translate("ActualizarEstadoVisita.colFecha");
            colRango.HeaderText = t.Translate("ActualizarEstadoVisita.colHorario");
            colEstado.HeaderText = t.Translate("ActualizarEstadoVisita.colEstado");

            CargarCombos();
        }
    }
}