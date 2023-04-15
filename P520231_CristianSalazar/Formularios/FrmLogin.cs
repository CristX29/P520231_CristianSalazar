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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            //cierra la app
            Application.Exit();





        }

        private void BtnVerContrasennia_MouseDown(object sender, MouseEventArgs e)
        {
            TxtContrasennia.UseSystemPasswordChar = false;

        }

        private void BtnVerContrasennia_MouseUp(object sender, MouseEventArgs e)
        {
            TxtContrasennia.UseSystemPasswordChar=true;
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            //validar que se haya digitado un usuario y contraseña

            if (!string.IsNullOrEmpty(TxtEmail.Text.Trim()) && 
                !string.IsNullOrEmpty(TxtContrasennia.Text.Trim()))
            {
                string usuario = TxtEmail.Text.Trim();
                string contrasennia = TxtContrasennia.Text.Trim();


                //tratar de validar que los datos digitados sean correctos
                //en caso que la validacion sea correcta, aplicamos los valores al usuario global 
                Globales.MiUsuarioGlobal = Globales.MiUsuarioGlobal.ValidarUsuarios(usuario, contrasennia);

                if (Globales.MiUsuarioGlobal.UsuarioID > 0)
                {
                    //si la validacion es correcta el ID deberia tener un valor mayor a 0
                    Globales.MiFormPrincipal.Show();

                    this.Hide();


                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña incorrectos...", "Error de Validacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TxtContrasennia.Focus();
                    TxtContrasennia.SelectAll();

                }



            }
            else
            {
                MessageBox.Show("Faltan datos requeridos!", "Error de Validacion", MessageBoxButtons.OK);

            }



            



        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            // al presionar cierta combinacion de teclas el boton de ingreso directo aparece
            if (e.Shift & e.KeyCode == Keys.A) {
            //si presionamos shift mas tab mas a
            btnIngresoDirecto.Visible = true;
            }
        }

        private void btnIngresoDirecto_Click(object sender, EventArgs e)
        {
            Globales.MiFormPrincipal.Show();

            this.Hide();
        }
    }
}
