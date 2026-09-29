using BE;
using BLL;
using Services;
using Services.Modelos;
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
using static System.Collections.Specialized.BitVector32;

namespace Servicios
{
    public partial class MenuPrincipal : Form, IIdiomaObserver
    {
        UsuarioModelo577MC usuarioActual = Services_577MC.ServiceSessionManager577MC.getIntancia().usuarioActivo;
        BLLIdioma577MC _idiomaService = new BLLIdioma577MC();
        BLLUsuario577MC _userService = new BLLUsuario577MC();

        public MenuPrincipal()
        {
            InitializeComponent();
            configurarAcceso();
            CargarSubItemsIdioma();

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();

        }
        private void CargarSubItemsIdioma()
        {
            idiomaToolStripMenuItem.DropDownItems.Clear();

            var idiomas = _idiomaService.obtenerTodos();

            foreach (var idioma in idiomas)
            {
                var item = new ToolStripMenuItem(idioma.Nombre);
                item.Tag = idioma;

                // Marcar el idioma actual del usuario
                int idiomaActual = ServiceSessionManager577MC.getIntancia().usuarioActivo.IdIdioma;
                item.Checked = idioma.Id == idiomaActual;

                item.Click += IdiomaItem_Click;
                idiomaToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void IdiomaItem_Click(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;
            var idiomaSeleccionado = (Idioma577MC)item.Tag;

            // Guardar en BD y sesión
            _userService.GuardarIdioma(idiomaSeleccionado.Id);

            // Aplicar idioma globalmente
            string cod = idiomaSeleccionado.Id == 1 ? "es" : "en";
            ServiceSessionManager577MC.getIntancia().Idioma.CargarIdioma(cod);

            // Actualizar checks del submenú
            foreach (ToolStripMenuItem subItem in idiomaToolStripMenuItem.DropDownItems)
            {
                subItem.Checked = subItem.Tag == item.Tag;
            }
        }

        private void configurarAcceso()
        {
            cambiarClaveToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Cambiar Clave");
            cerrarSesionToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Cerrar Sesion");
            gestionUsuariosToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Gestion Usuario");
            iniciarSesionToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Iniciar Sesion");
            bitacoraEventosToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Auditoria Eventos");
            gestionRolToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Gestion Roles");
            gestionFamiliaToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Gestion Familia");
            cambiarClaveToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Cambiar Clave");
            cerrarSesionToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Cerrar Sesion");
            iniciarSesionToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Iniciar Sesion");
            idiomaToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Cambiar Idioma");
            gestionRespaldoToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Gestion Respaldo");
            clientesToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Gestion Clientes") || ServiceSessionManager577MC.getIntancia().TienePermiso("Seleccionar Propiedad") || ServiceSessionManager577MC.getIntancia().TienePermiso("Agendar Visita") || ServiceSessionManager577MC.getIntancia().TienePermiso("Actualizar Estado Visita");
            seleccionarPropiedadToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Seleccionar Propiedad");
            agendarVisitaToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Agendar Visita");
            actualizarEstadoVisitaToolStripMenuItem.Enabled = ServiceSessionManager577MC.getIntancia().TienePermiso("Actualizar Estado Visita");
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarContraseña form = new CambiarContraseña();
            form.Show();
        }

        private void gestionUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuario form = new GestionUsuario();
            form.Show();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            DialogResult resultado = MessageBox.Show(
                t.Translate("MenuPrincipal.msgConfirmarCierreSesion"),
                t.Translate("MenuPrincipal.msgTituloCierreSesion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Services_577MC.ServiceSessionManager577MC.getIntancia().Logout();

            base.OnFormClosing(e);
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login form = new Login();
            form.Show();
        }

        private void bitacoraEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuditoriaBitacora form = new AuditoriaBitacora();
            form.Show();
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("MenuPrincipal.formTitle");
            label1.Text = string.Format(t.Translate("MenuPrincipal.labelBienvenido"), usuarioActual.Nombre, usuarioActual.Apellido);
            usuarioToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuUsuario");
            cambiarClaveToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuCambiarClave");
            cerrarSesionToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuCerrarSesion");
            iniciarSesionToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuIniciarSesion");
            administradorToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuAdministrador");
            gestionUsuariosToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuGestionUsuarios");
            bitacoraEventosToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuBitacoraEventos");
            gestionFamiliaToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuGestionFamilia");
            gestionRolToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuGestionRol");
            ayudaToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuAyuda");
            idiomaToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuIdioma");
            gestionRespaldoToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuRespaldo");
            clientesToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuClientes");
            registrarClienteToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuRegistrarCliente");
            seleccionarPropiedadToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuSeleccionarPropiedad");
            agendarVisitaToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuAgendarVisita");
            actualizarEstadoVisitaToolStripMenuItem.Text = t.Translate("MenuPrincipal.menuActualizarEstadoVisita");
        }

        private void gestionFamiliaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionFamilia form = new GestionFamilia();
            form.Show();
        }

        private void gestionRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionRol form = new GestionRol();
            form.Show();
        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gestionRespaldoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionRespaldo form = new GestionRespaldo();
            form.Show();
        }

        private void registrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarCliente form = new RegistrarCliente();
            form.Show();
        }

        private void seleccionarPropiedadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!ServiceSessionManager577MC.getIntancia().TienePermiso("Seleccionar Propiedad"))
            {
                MessageBox.Show(t.Translate("MenuPrincipal.msgAccesoDenegadoSeleccionarPropiedad"), t.Translate("MenuPrincipal.titleAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SeleccionarPropiedad form = new SeleccionarPropiedad();
            form.Show();
        }

        private void agendarVisitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!ServiceSessionManager577MC.getIntancia().TienePermiso("Agendar Visita"))
            {
                MessageBox.Show(t.Translate("MenuPrincipal.msgAccesoDenegadoVisitas"), t.Translate("MenuPrincipal.titleAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RegistrarVisita form = new RegistrarVisita();
            form.Show();
        }

        private void actualizarEstadoVisitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            if (!ServiceSessionManager577MC.getIntancia().TienePermiso("Actualizar Estado Visita"))
            {
                MessageBox.Show(t.Translate("MenuPrincipal.msgAccesoDenegadoEstadoVisita"), t.Translate("MenuPrincipal.titleAccesoDenegado"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ActualizarEstadoVisita form = new ActualizarEstadoVisita();
            form.Show();
        }
    }
}
