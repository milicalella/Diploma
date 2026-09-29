using BE;
using BLL;
using Services.Modelos.Idioma;
using Services_577MC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Servicios
{
    public partial class RegistrarCliente : Form, IIdiomaObserver
    {
        BLLCliente577MC _clienteBLL = new BLLCliente577MC();

        public Cliente577MC ClienteRegistrado { get; private set; }

        private readonly bool _modoAbm;
        private string _dniSeleccionado;
        private List<Cliente577MC> _clientesDeserializados;

        public RegistrarCliente()
        {
            InitializeComponent();

            _modoAbm = true;

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();

            CargarClientes();
        }

        public RegistrarCliente(string dni)
            : this()
        {
            _modoAbm = false;
            txtDNI.Text = dni ?? "";
            btnModificar.Visible = false;
            btnEliminar.Visible = false;
            btnNuevo.Visible = false;
            gbSerializacion.Visible = false;
            this.ClientSize = new System.Drawing.Size(790, 540);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            try
            {
                if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                    string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show(t.Translate("RegistrarCliente.msgCamposObligatorios"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Cliente577MC cliente = new Cliente577MC
                {
                    DNI = txtDNI.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim()
                };

                _clienteBLL.RegistrarCliente(cliente);

                ClienteRegistrado = cliente;

                MessageBox.Show(t.Translate("RegistrarCliente.msgClienteRegistrado"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_modoAbm)
                {
                    CargarClientes();
                    LimpiarCampos();
                    return;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (string.IsNullOrEmpty(_dniSeleccionado))
            {
                MessageBox.Show(t.Translate("RegistrarCliente.msgDebeSeleccionar"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(t.Translate("RegistrarCliente.msgCamposObligatorios"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cliente577MC cliente = new Cliente577MC
                {
                    DNI = _dniSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim()
                };

                _clienteBLL.ModificarCliente(cliente);

                MessageBox.Show(t.Translate("RegistrarCliente.msgClienteModificado"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (string.IsNullOrEmpty(_dniSeleccionado))
            {
                MessageBox.Show(t.Translate("RegistrarCliente.msgDebeSeleccionar"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                string.Format(t.Translate("RegistrarCliente.msgConfirmarEliminar"), _dniSeleccionado),
                t.Translate("RegistrarCliente.title"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _clienteBLL.EliminarCliente(_dniSeleccionado);

                MessageBox.Show(t.Translate("RegistrarCliente.msgClienteEliminado"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            txtDNI.Focus();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];
            if (fila.DataBoundItem == null)
            {
                return;
            }

            Cliente577MC cliente = (Cliente577MC)fila.DataBoundItem;

            _dniSeleccionado = cliente.DNI;
            txtDNI.Text = cliente.DNI;
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtTelefono.Text = cliente.Telefono;
            txtEmail.Text = cliente.Email;
        }

        private void btnSerializar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            List<Cliente577MC> seleccion = new List<Cliente577MC>();

            foreach (DataGridViewRow fila in dgvClientes.SelectedRows)
            {
                if (fila.DataBoundItem is Cliente577MC cliente)
                {
                    seleccion.Add(cliente);
                }
            }

            if (seleccion.Count == 0)
            {
                MessageBox.Show(t.Translate("RegistrarCliente.msgDebeSeleccionarClientes"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "XML (*.xml)|*.xml",
                FileName = "clientes.xml",
                Title = t.Translate("RegistrarCliente.titleSerializar")
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Cliente577MC>));

                using (FileStream stream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    serializer.Serialize(stream, seleccion);
                }

                MessageBox.Show(t.Translate("RegistrarCliente.msgSerializacionExitosa"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(t.Translate("RegistrarCliente.msgErrorSerializar"), ex.Message), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeserializar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "XML (*.xml)|*.xml",
                Title = t.Translate("RegistrarCliente.titleDeserializar")
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                List<Cliente577MC> lista = null;

                using (FileStream stream = new FileStream(dialog.FileName, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cliente577MC>));
                    lista = serializer.Deserialize(stream) as List<Cliente577MC>;
                }

                if (lista == null || lista.Count == 0)
                {
                    MessageBox.Show(t.Translate("RegistrarCliente.msgArchivoVacio"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _clientesDeserializados = lista;
                MostrarDeserializados();

                MessageBox.Show(string.Format(t.Translate("RegistrarCliente.msgClientesRecuperados"), lista.Count), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(t.Translate("RegistrarCliente.msgErrorDeserializar"), ex.Message), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            lstClientesDeserializados.Items.Clear();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (_clientesDeserializados == null || _clientesDeserializados.Count == 0)
            {
                MessageBox.Show(t.Translate("RegistrarCliente.msgNoHayDeserializados"), t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarDeserializados();
        }

        private void MostrarDeserializados()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            lstClientesDeserializados.Items.Clear();

            foreach (Cliente577MC cliente in _clientesDeserializados)
            {
                lstClientesDeserializados.Items.Add(string.Format(t.Translate("RegistrarCliente.formatoItemLista"), cliente.Nombre, cliente.Apellido, cliente.DNI));
            }
        }

        private void CargarClientes()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = _clienteBLL.ObtenerTodos();
        }

        private void LimpiarCampos()
        {
            _dniSeleccionado = null;
            txtDNI.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            dgvClientes.ClearSelection();
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("RegistrarCliente.formTitle");
            lblDNI.Text = t.Translate("RegistrarCliente.labelDNI");
            lblNombre.Text = t.Translate("RegistrarCliente.labelNombre");
            lblApellido.Text = t.Translate("RegistrarCliente.labelApellido");
            lblTelefono.Text = t.Translate("RegistrarCliente.labelTelefono");
            lblEmail.Text = t.Translate("RegistrarCliente.labelEmail");
            btnGuardar.Text = t.Translate("RegistrarCliente.btnGuardar");
            btnModificar.Text = t.Translate("RegistrarCliente.btnModificar");
            btnEliminar.Text = t.Translate("RegistrarCliente.btnEliminar");
            btnNuevo.Text = t.Translate("RegistrarCliente.btnNuevo");
            colDNI.HeaderText = t.Translate("RegistrarCliente.colDNI");
            colNombre.HeaderText = t.Translate("RegistrarCliente.colNombre");
            colApellido.HeaderText = t.Translate("RegistrarCliente.colApellido");
            colTelefono.HeaderText = t.Translate("RegistrarCliente.colTelefono");
            colEmail.HeaderText = t.Translate("RegistrarCliente.colEmail");
            gbSerializacion.Text = t.Translate("RegistrarCliente.groupSerializacion");
            lblDeserializados.Text = t.Translate("RegistrarCliente.labelDeserializados");
            btnSerializar.Text = t.Translate("RegistrarCliente.btnSerializar");
            btnDeserializar.Text = t.Translate("RegistrarCliente.btnDeserializar");
            btnLimpiar.Text = t.Translate("RegistrarCliente.btnLimpiar");
            btnActualizar.Text = t.Translate("RegistrarCliente.btnActualizar");
            toolTip.SetToolTip(btnLimpiar, t.Translate("RegistrarCliente.ttLimpiar"));
            toolTip.SetToolTip(btnActualizar, t.Translate("RegistrarCliente.ttActualizar"));

            if (_clientesDeserializados != null)
            {
                MostrarDeserializados();
            }
        }
    }
}