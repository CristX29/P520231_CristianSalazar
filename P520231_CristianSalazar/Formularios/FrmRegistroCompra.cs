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
    public partial class FrmRegistroCompra : Form
    {
        //temp para probar
       
       public Compra MiCompraLocal { get; set; }

        public DataTable ListaProductos { get; set; }

        //TODO CREAR EL OBJETO DE COMPRA LOCAL

        public FrmRegistroCompra()
        {
            InitializeComponent();

            MiCompraLocal= new Compra();
            ListaProductos= new DataTable();
        }

        private void FrmRegistroCompra_Load(object sender, EventArgs e)
        {
            MdiParent = Globales.MiFormPrincipal;
        }

        private void BtnProveedorBuscar_Click(object sender, EventArgs e)
        {
            //se abre un nuevo form de busqueda de proveedor
            //este form no es necesario definirlo en los globales

            Form FormBusquedaProveedor = new FrmProveedorBuscar();

            DialogResult respuesta = FormBusquedaProveedor.ShowDialog();

            if (respuesta == DialogResult.OK)
            {
                //TODO ver la posibilidad de asignar al objeto de compra local el valor de proveedor
                TxtProveedorNombre.Text = MiCompraLocal.MiProveedor.ProveedorNombre;

            }
        }

        private void BtnProductoAgregar_Click(object sender, EventArgs e)
        {
            Form MiFormBusquedaItem = new FrmCompraAgregarProducto();
            DialogResult respuesta = MiFormBusquedaItem.ShowDialog();
            if (respuesta == DialogResult.OK)
            {
                DgvLista.DataSource = ListaProductos;
            }
        }
    }
}
