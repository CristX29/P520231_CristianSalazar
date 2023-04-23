using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P520231_CristianSalazar.Formularios
{
    public partial class FrmMDI : Form
    {
        public FrmMDI()
        {
            InitializeComponent();
        }

        private void FrmMDI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //control para que el formulario de gestion de usuarios se muestre
            //solo una vez

            if(!Globales.MiFormGestionUsuarios.Visible)
            {
                Globales.MiFormGestionUsuarios = new FrmUsuariosGestion();
                Globales.MiFormGestionUsuarios.Show();

            }
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void FrmMDI_Load(object sender, EventArgs e)
        {

            //mostrar el usuario logueado

            string InfoUsuario = string.Format("{0}-{1}({2}) ", Globales.MiUsuarioGlobal.UsuarioNombre,
                                                                Globales.MiUsuarioGlobal.UsuarioCorreo,
                                                                Globales.MiUsuarioGlobal.MiRolTipo.UsuarioRolDescripcion);
            LblUsuario.Text = InfoUsuario;

            switch (Globales.MiUsuarioGlobal.MiRolTipo.UsuarioRolId)
            {
                case 1:
                    break;
                    //seria admin no se oculta nada
                    case 2:
                    //seria usuario normal , se deben ocultar algunas opciones de menu
                    gestionDeUsuariosToolStripMenuItem.Visible = false;
                    rolesDeUsuarioToolStripMenuItem.Visible = false;
                    tipoToolStripMenuItem.Visible = false;
                    tipoDeCompraToolStripMenuItem.Visible = false;
                    break;
            }







        }

        private void registroDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Globales.MiFormRegistroCompra.Visible)
            {
                Globales.MiFormRegistroCompra = new FrmRegistroCompra();
                Globales.MiFormRegistroCompra.Show();

            }
        }

        private void gestionDeProveedorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Globales.MiFormGestionProveedor.Visible)
            {
                Globales.MiFormGestionProveedor = new FrmProveedorGestion();
                Globales.MiFormGestionProveedor.Show();

            }


        }
    }
}
