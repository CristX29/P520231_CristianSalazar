using Logica.Models;
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
    public partial class FrmProveedorGestion : Form
    {

        private DataTable ListarProveedores { get; set; }
        Proveedor MiProveedorLocal { get; set; }

        public FrmProveedorGestion()
        {
            InitializeComponent();

            ListarProveedores = new DataTable();

            MiProveedorLocal = new Proveedor();
        }

        private void FrmProveedorGestion_Load(object sender, EventArgs e)
        {
            CargarListadeProveedores();
            CargarListaDeTiposProveedor();
        }

        private void CargarListaDeTiposProveedor()
        {
         Logica.Models.TipoProveedor MiProveedor = new Logica.Models.TipoProveedor();
            DataTable dt = new DataTable();

            dt = MiProveedor.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {
                CbTiposProveedor.ValueMember = "ID";
                CbTiposProveedor.DisplayMember= "Descrip";
                CbTiposProveedor.DataSource= dt;
                CbTiposProveedor.SelectedIndex = -1;
            }


        }
        private void CargarListadeProveedores()
        {
            ListarProveedores = new DataTable();


            string FiltroBusqueda = " ";
            if (!String.IsNullOrEmpty(TxtBuscar.Text.Trim()) && TxtBuscar.Text.Count() >= 3)
            {
                FiltroBusqueda = TxtBuscar.Text.Trim();
            }

            ListarProveedores = MiProveedorLocal.ListarProveedorEnGestion(FiltroBusqueda);

            DgvLista.DataSource = ListarProveedores;

        }

     

        private bool ValidarDatosDigitados()
        {
            bool R = false;

            if (!string.IsNullOrEmpty(TxtProveedorNombre.Text.Trim()))
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de nombre del proveedor no puede estar vacío.");
                TxtProveedorNombre.Focus();
                return false;
            }
            if (!string.IsNullOrEmpty(TxtProveedorCedula.Text.Trim()))
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de cedula del proveedor no puede estar vacío.");
                TxtProveedorCedula.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(TxtProveedorNotas.Text.Trim()))
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de notas del proveedor no puede estar vacío.");
                TxtProveedorNotas.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(TxtProveedorCorreo.Text.Trim()))
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de correo del proveedor no puede estar vacío.");
                TxtProveedorCorreo.Focus();
                return false;
            }

            if (!string.IsNullOrEmpty(TxtProveedorDireccion.Text.Trim()))
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de direccion del proveedor no puede estar vacío.");
                TxtProveedorDireccion.Focus();
                return false;
            }
            if (CbTiposProveedor.SelectedIndex == 0 || CbTiposProveedor.SelectedIndex == 1)
            {
                R = true;
            }
            else
            {
                MessageBox.Show("El campo de tipo del proveedor no puede estar vacío.");
                CbTiposProveedor.Focus();
                return false;
            }
















            return R;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosDigitados())
            {
                bool NombreOK;
                bool CedulaOK;

                MiProveedorLocal = new Logica.Models.Proveedor();

                MiProveedorLocal.ProveedorNombre = TxtProveedorNombre.Text.Trim();
                MiProveedorLocal.ProveedorCedula = TxtProveedorCedula.Text.Trim();
                MiProveedorLocal.ProveedorEmail = TxtProveedorCorreo.Text.Trim();
                MiProveedorLocal.ProveedorDireccion = TxtProveedorDireccion.Text.Trim();
                MiProveedorLocal.ProveedorNotas = TxtProveedorNotas.Text.Trim();
                MiProveedorLocal.MiTipoProveedor.ProveedorTipoID = Convert.ToInt32(CbTiposProveedor.SelectedValue);

                string msg = string.Format("¿Está seguro que desea agregar al Proveedor {0}?", MiProveedorLocal.ProveedorNombre);
                DialogResult respuesta = MessageBox.Show(msg, "??", MessageBoxButtons.YesNo);

                if (respuesta == DialogResult.Yes)
                {
                    bool ok = MiProveedorLocal.Agregar();

                    if (ok)
                    {
                        MessageBox.Show("Proveedor guardado correctamente!", ":)", MessageBoxButtons.OK);

                        LimpiarFormulario();
                        CargarListadeProveedores();
                        
                    }
                    else
                    {

                        MessageBox.Show("El Proveedor no se pudo agregar!", ":/", MessageBoxButtons.OK);
                    }
                }

            }






        }
        private void LimpiarFormulario()
        {
            TxtProveedorID.Clear();
            TxtProveedorNombre.Clear();
            TxtProveedorCedula.Clear();
            TxtProveedorCorreo.Clear();
            TxtProveedorDireccion.Clear();
            TxtProveedorNotas.Clear();

            CbTiposProveedor.SelectedIndex = -1;


            



        }

    }
}
