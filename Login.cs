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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Services;

namespace Servicios
{
    public partial class Login : Form, IIdiomaObserver
    {
        BLLUsuario577MC _userService = new BLLUsuario577MC();
        BLLIdioma577MC _idiomaService = new BLLIdioma577MC();
        

        public Login()
        {
            InitializeComponent();
            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);

        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            string username = txtUser.Text;
            string password = txtPassword.Text;
            try
            {
                bool usaPasswordDefault = _userService.login(username, password);

                int idiomaUsuario = ServiceSessionManager577MC.getIntancia().usuarioActivo.IdIdioma;
                string codIdiomaUsuario = idiomaUsuario == 1 ? "es" : "en";
                ServiceSessionManager577MC.getIntancia().Idioma.CargarIdioma(codIdiomaUsuario);

                var integridad = DigitoVerificador577MC.VerificarIntegridad();
                bool usuarioOk = integridad.Usuario;
                bool rolOk = integridad.Rol;
                bool familiaOk = integridad.Familia;
                bool patenteOk = integridad.Patente;


                
                if (!usuarioOk || !rolOk || !familiaOk || !patenteOk)
                {
                    if (ServiceSessionManager577MC.getIntancia().usuarioActivo.Rol.Id != 1)
                    {
                        MessageBox.Show(ServiceSessionManager577MC.getIntancia().Idioma.Translate("Login.msgInconsistencias"));
                        txtUser.Text = "";
                        txtPassword.Text = "";
                        ServiceSessionManager577MC.getIntancia().Logout();
                        return;

                    }
                    this.Hide();
                    RepararInconsistencias pantalla = new RepararInconsistencias(usuarioOk, rolOk, familiaOk, patenteOk);
                    pantalla.FormClosed += (s, args) => RestaurarIdiomaLogin();
                    pantalla.Show();
                    return;
                }


                
                
                txtUser.Text = "";
                txtPassword.Text = "";
                this.Hide();

                if (usaPasswordDefault)
                {
                    CambiarContraseña form = new CambiarContraseña();
                    form.FormClosed += (s, args) => RestaurarIdiomaLogin();
                    form.Show();
                }
                else
                {
                    MenuPrincipal menu = new MenuPrincipal();
                    menu.FormClosed += (s, args) => RestaurarIdiomaLogin();
                    menu.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RestaurarIdiomaLogin()
        {

            this.Show();
        }

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("Login.formTitle");
            label3.Text = t.Translate("Login.labelBienvenido");
            lblUsuario.Text = t.Translate("Login.lblUsuario");
            lblContrasena.Text = t.Translate("Login.lblPassword");
            btnLogin.Text = t.Translate("Login.btnLogin");
        }

        
    }
}
