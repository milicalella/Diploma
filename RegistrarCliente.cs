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
    public partial class RegistrarCliente : Form, IIdiomaObserver
    {
        BLLCliente577MC _clienteBLL = new BLLCliente577MC();

        public Cliente577MC ClienteRegistrado { get; private set; }

        public RegistrarCliente()
        {
            InitializeComponent();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        public RegistrarCliente(string dni)
            : this()
        {
            txtDNI.Text = dni ?? "";
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

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, t.Translate("RegistrarCliente.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }
    }
}