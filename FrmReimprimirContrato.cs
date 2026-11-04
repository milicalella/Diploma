using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Servicios
{
    public partial class FrmReimprimirContrato : Form, IIdiomaObserver
    {
        private static string Titulo
        {
            get
            {
                return ServiceSessionManager577MC.getIntancia().Idioma
                    .Translate("FrmReimprimirContrato.formTitle");
            }
        }

        BLLContrato577MC _contratoBLL = new BLLContrato577MC();

        bool _cargando;

        public FrmReimprimirContrato()
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
                List<Contrato577MC> contratos = _contratoBLL.ObtenerContratosFirmados();

                dgvContratos.DataSource = null;
                dgvContratos.DataSource = contratos;

                if (contratos.Count > 0)
                {
                    dgvContratos.ClearSelection();
                    dgvContratos.Rows[0].Selected = true;
                }

                btnGenerarPdf.Enabled = contratos.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGenerarPdf.Enabled = false;
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

            btnGenerarPdf.Enabled = dgvContratos.CurrentRow != null;
        }

        private void dgvContratos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                GenerarPdf();
            }
        }

        private void btnGenerarPdf_Click(object sender, EventArgs e)
        {
            GenerarPdf();
        }

        private void GenerarPdf()
        {
            Contrato577MC contrato = ContratoSeleccionado();

            if (contrato == null)
            {
                var t = ServiceSessionManager577MC.getIntancia().Idioma;

                MessageBox.Show(t.Translate("FrmReimprimirContrato.msgDebeSeleccionarContrato"), Titulo,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ReporteRN2577MC.Emitir(contrato, false);
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

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("FrmReimprimirContrato.formTitle");
            lblTitulo.Text = t.Translate("FrmReimprimirContrato.formTitle");

            gbContratos.Text = t.Translate("FrmReimprimirContrato.gbContratos");
            colNroContrato.HeaderText = t.Translate("FrmReimprimirContrato.colNroContrato");
            colFechaEmision.HeaderText = t.Translate("FrmReimprimirContrato.colFechaEmision");
            colDNICliente.HeaderText = t.Translate("FrmReimprimirContrato.colDNICliente");
            colPropiedadDireccion.HeaderText = t.Translate("FrmReimprimirContrato.colPropiedadDireccion");
            colTipoContrato.HeaderText = t.Translate("FrmReimprimirContrato.colTipoContrato");
            colMontoAcordado.HeaderText = t.Translate("FrmReimprimirContrato.colMontoAcordado");
            colEstado.HeaderText = t.Translate("FrmReimprimirContrato.colEstado");

            btnGenerarPdf.Text = t.Translate("FrmReimprimirContrato.btnGenerarPdf");
            lblAyuda.Text = t.Translate("FrmReimprimirContrato.lblAyuda");

            dgvContratos.Invalidate();
        }
    }
}
