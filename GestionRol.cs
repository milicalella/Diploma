using BLL;
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

namespace Servicios
{
    public partial class GestionRol : Form, IIdiomaObserver
    {
        BLLRol577MC bllRol = new BLLRol577MC();
        BLLFamilia577MC bllFamilia = new BLLFamilia577MC();
        List<RolModelo577MC> listaRol = new List<RolModelo577MC>();
        BLLPatente577MC bllPatente = new BLLPatente577MC();

        public GestionRol()
        {
            InitializeComponent();

            cargarDatos();
            btnCancelar.Enabled = false;
            btnAplicar.Enabled = false;

            ServiceSessionManager577MC.getIntancia().Idioma.Suscribir(this);
            actualizarIdioma();
        }

        private void cargarDatos()
        {
            tvPermisosAsignados.Nodes.Clear();
            listaRol = bllRol.ObtenerRolesConJerarquia();
            dgvFamilias.DataSource = null;
            dgvFamilias.DataSource = listaRol;

            var todosLosComponentes = new List<Componente577MC>();

            todosLosComponentes.AddRange(bllFamilia.ObtenerTodos());
            todosLosComponentes.AddRange(bllPatente.obtenerTodos());

            checkListPermisosFamilias.DataSource = null;
            checkListPermisosFamilias.DataSource = todosLosComponentes;
        }

        private enum ModoOperacionFamilia
        {
            Ninguno,
            Crear,
            Asignar,
            Eliminar,
            Desasignar
        }

        private ModoOperacionFamilia modoActual = ModoOperacionFamilia.Ninguno;

        public void actualizarIdioma()
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;

            this.Text = t.Translate("GestionRol.formTitle");
            label1.Text = t.Translate("GestionRol.labelRoles");
            label2.Text = t.Translate("GestionRol.labelPermisosFamilias");
            label3.Text = t.Translate("GestionRol.labelAsignados");
            groupBox1.Text = t.Translate("GestionRol.groupBoxDatos");
            label4.Text = t.Translate("GestionRol.labelNombre");
            btnCrear.Text = t.Translate("GestionRol.btnCrear");
            btnAsignar.Text = t.Translate("GestionRol.btnAsignar");
            btnAplicar.Text = t.Translate("GestionRol.btnAplicar");
            btnEliminar.Text = t.Translate("GestionRol.btnEliminar");
            btnDesasginar.Text = t.Translate("GestionRol.btnDesasignar");
            btnCancelar.Text = t.Translate("GestionRol.btnCancelar");
            //dgvFamilias.Columns["Nombre"].HeaderText = t.Translate("GestionUsuario.colNombre");

        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Crear;

            groupBox1.Visible = true;
            txtNombre.Focus();

            btnAplicar.Enabled = true;
            btnCrear.Enabled = false;
            btnEliminar.Enabled = false;
            btnAsignar.Enabled = false;
            btnCancelar.Enabled = true;
            btnDesasginar.Enabled = false;
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Asignar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
            btnDesasginar.Enabled = false;
        }

        private void dgvFamilias_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFamilias.CurrentRow != null)
            {
                RolModelo577MC rolSeleccionado = (RolModelo577MC)dgvFamilias.CurrentRow.DataBoundItem;
                MostrarArbolDelRol(rolSeleccionado);
            }
        }

        private void MostrarArbolDelRol(RolModelo577MC rol)
        {
            tvPermisosAsignados.Nodes.Clear(); 
            TreeNode nodoRaiz = new TreeNode($"Rol: {rol.Nombre}");
            tvPermisosAsignados.Nodes.Add(nodoRaiz);

            foreach (Componente577MC componente in rol.Permisos)
            {
                TreeNode nodoHijo = new TreeNode(componente.Nombre);
                nodoHijo.Tag = componente;
                nodoRaiz.Nodes.Add(nodoHijo);

                if (componente is FamiliaModelo577MC familia)
                {
                    ConstruirRamasFamilia(nodoHijo, familia); // si es familia, llamamos al método recursivo para abrirla
                }
                else if (componente is PermisoModelo577MC patente)
                {
                    nodoHijo.Text = $"{patente.Nombre}";
                }
            }

            tvPermisosAsignados.ExpandAll();
        }

        private void ConstruirRamasFamilia(TreeNode nodoPadre, FamiliaModelo577MC familia)
        {
            foreach (Componente577MC hijo in familia.obtenerPermisos())
            {
                TreeNode nodoHijo = new TreeNode();
                nodoPadre.Nodes.Add(nodoHijo);

                if (hijo is FamiliaModelo577MC subFamilia)
                {
                    nodoHijo.Text = $"{subFamilia.Nombre}";
                    ConstruirRamasFamilia(nodoHijo, subFamilia);
                }
                else
                {
                    nodoHijo.Text = $"{hijo.Nombre}";
                }
            }
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;
            try
            {
                if (modoActual == ModoOperacionFamilia.Crear)
                {
                    string nombre = txtNombre.Text;

                    if (string.IsNullOrEmpty(nombre))
                    {
                        MessageBox.Show(t.Translate("GestionRol.msgNombreRequerido"));
                        return;
                    }

                    if (checkListPermisosFamilias.CheckedItems.Count == 0)
                    {
                        MessageBox.Show(t.Translate("GestionRol.msgSeleccionarComponente"));
                        return;
                    }

                    List<Componente577MC> componentesSeleccionados = new List<Componente577MC>();

                    foreach (Componente577MC componenteMarcado in checkListPermisosFamilias.CheckedItems)
                    {
                        componentesSeleccionados.Add(componenteMarcado);
                    }

                    bllRol.crearRol(nombre, componentesSeleccionados);

                    MessageBox.Show(t.Translate("GestionRol.msgRolCreado"));
                }

                else if (modoActual == ModoOperacionFamilia.Asignar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show(t.Translate("GestionRol.msgSeleccionarRol"), t.Translate("GestionRol.msgValidacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (checkListPermisosFamilias.CheckedItems.Count == 0)
                    {
                        MessageBox.Show(t.Translate("GestionRol.msgSeleccionarComponente"), t.Translate("GestionRol.msgValidacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    RolModelo577MC rolDestino = (RolModelo577MC)dgvFamilias.CurrentRow.DataBoundItem;

                    foreach (Componente577MC componenteMarcado in checkListPermisosFamilias.CheckedItems)
                    {
                        if (componenteMarcado is PermisoModelo577MC patente)
                        {
                            bllRol.AsignarPatente(rolDestino, patente);
                        }
                        else if (componenteMarcado is FamiliaModelo577MC familiaHija)
                        {
                            bllRol.AsignarFamilia(rolDestino, familiaHija);
                        }
                    }

                    MessageBox.Show(t.Translate("GestionRol.msgComponentesAsignados"));
                }

                else if (modoActual == ModoOperacionFamilia.Eliminar)
                {
                    if (dgvFamilias.CurrentRow == null)
                    {
                        MessageBox.Show(t.Translate("GestionRol.msgRolAEliminar"));
                        return;
                    }

                    RolModelo577MC rolSeleccionado = (RolModelo577MC)dgvFamilias.CurrentRow.DataBoundItem;
                    bllRol.EliminarRol(rolSeleccionado.Id);
                    MessageBox.Show(t.Translate("GestionRol.msgRolEliminado"));
                }

                else if(modoActual == ModoOperacionFamilia.Desasignar)
                {
                    RolModelo577MC rolSeleccionado = (RolModelo577MC)dgvFamilias.CurrentRow.DataBoundItem;
                    Componente577MC componenteAQuitar = (Componente577MC)tvPermisosAsignados.SelectedNode.Tag;

                    DialogResult respuesta = MessageBox.Show(t.Translate("GestionRol.msgConfirmarQuitarComponente"), "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes)
                    {
                        if (componenteAQuitar is PermisoModelo577MC patente)
                        {
                            bllRol.DesasignarPatente(rolSeleccionado.Id, patente.Id);
                        }
                        else if (componenteAQuitar is FamiliaModelo577MC familia)
                        {
                            bllRol.DesasignarFamilia(rolSeleccionado.Id, familia.Id);
                        }

                        MessageBox.Show(t.Translate("GestionRol.msgComponenteDesasignado"));
                        cargarDatos(); // Recargamos la BD para actualizar el árbol
                    }
                }

                modoActual = ModoOperacionFamilia.Ninguno;
                groupBox1.Visible = false;
                btnCancelar.Enabled = false;
                btnAplicar.Enabled = false;
                btnCrear.Enabled = true;
                btnAsignar.Enabled = true;
                btnEliminar.Enabled = true;
                btnDesasginar.Enabled = true;

                DesmarcarCheckList();
                cargarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DesmarcarCheckList()
        {
            for (int i = 0; i < checkListPermisosFamilias.Items.Count; i++)
            {
                checkListPermisosFamilias.SetItemChecked(i, false);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Ninguno;
            groupBox1.Visible = false;
            btnCancelar.Enabled = false;
            btnAplicar.Enabled = false;
            btnCrear.Enabled = true;
            btnAsignar.Enabled = true;
            btnEliminar.Enabled = true;
            btnDesasginar.Enabled = true;

            DesmarcarCheckList();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            modoActual = ModoOperacionFamilia.Eliminar;

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnAsignar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCrear.Enabled = false;
            btnDesasginar.Enabled = false;
        }

        private void btnDesasginar_Click(object sender, EventArgs e)
        {
            var t = ServiceSessionManager577MC.getIntancia().Idioma;
            modoActual = ModoOperacionFamilia.Desasignar;

            if (dgvFamilias.CurrentRow == null) return;
            if (tvPermisosAsignados.SelectedNode == null)
            {
                MessageBox.Show(t.Translate("GestionRol.msgSeleccionarComponenteDesasignar"));
                return;
            }

            if (tvPermisosAsignados.SelectedNode.Level != 1)
            {
                MessageBox.Show(t.Translate("GestionRol.msgSoloDesasignarComponentesDirectos"));
                return;
            }

            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;
            btnAsignar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
            btnDesasginar.Enabled = false;
        }
    }
}
