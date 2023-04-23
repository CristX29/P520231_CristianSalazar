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
            CargarTiposDeCompra();
            LimpiarForm();


        }

        private void CargarTiposDeCompra()
        {
            DataTable dtTiposCompra = new DataTable();

            dtTiposCompra = MiCompraLocal.MiTipoCompra.Listar();

            CboxCompraTipo.ValueMember = "id";
            CboxCompraTipo.DisplayMember= "descripcion";

            CboxCompraTipo.DataSource = dtTiposCompra;
            CboxCompraTipo.SelectedIndex = -1;




        }

        private void LimpiarForm()
        {
            TxtProveedorNombre.Clear();
            TxtNotas.Clear();
            TxtTotal.Text = "0";
            TxtTotalCantidad.Text= "0";
            CboxCompraTipo.SelectedIndex = -1;

            //se debe cargar un esquema en el datatable del detalle (ListaProductos)
            //esto es importante para saber como se llaman los campos, que tipos tienen
            // y que pueda servir de datasource del dgvlista sin que elimine las columnas
            //hechas en tiempo de diseño
                
            ListaProductos = new DataTable();

            ListaProductos = MiCompraLocal.CargarEsquemaDetalle();

            DgvLista.DataSource = ListaProductos;
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

                Totalizar();
            }
        }

        private void Totalizar()
        {
            // se usa para mostrar los totales en la parte inferior del form
            if (ListaProductos.Rows.Count > 0 )
            {
                //se recorre la lista de filas del datatable de detalle y se
                //realizan las operaciones matematicas para sumarizar

                decimal totalItems = 0;
                decimal totalMonto = 0;

                foreach (DataRow row in ListaProductos.Rows)
                {
                    totalItems += Convert.ToDecimal(row["Cantidad"]);
                    //totalItems = totalItems + algo

                    totalMonto += Convert.ToDecimal(row["PrecioVentaUnitario"]) * Convert.ToDecimal(row["Cantidad"]); ;


                }
                TxtTotalCantidad.Text = totalItems.ToString();
                //este formato sirve para representar un valor monetario,
                //existen muchisimos mas formatos personalizados, por favor investigar
                // {0:N2}
                TxtTotal.Text= string.Format("{0:C2}", totalMonto.ToString());



            }


        }

        private void BtnCrearCompra_Click(object sender, EventArgs e)
        {
            //primero se valida que se haya seleccionado un proveedor, un tipo de compra y que haya como minimo una lidea en el detalle
            if (ValidarCompra())
            {

                
                //los pasos para magregar un encabezado-detalle son:
                //1. realizar insert en el encabezado y recolectar el ID recien creado,
                //teniendo claro que ese ID se genera a nivel de bd.

                //2. Con ese ID mas el ID del producto tenemos las dos FK para hacer el insert en
                //la tabla de detalle

                //se agregan los datos de encabezado que hacen falta (de proveedor ya estaban listos)
                MiCompraLocal.MiTipoCompra.CompraTipoID = Convert.ToInt32(CboxCompraTipo.SelectedValue);

                MiCompraLocal.CompraNotas = TxtNotas.Text.Trim();

                //COMO estoy ingresando con un boton de ingreso rapido en el login, no tengo datos en el usuario global
                //por lo pronto el ID sera quemado
                MiCompraLocal.MiUsuario.UsuarioID = 1;

                TrasladoListaVisualAObjetoCompra();

                //a este punto tenemos armado completamente el objeto de compra local
                //se puede proceder a la funcion de agregar
                if (MiCompraLocal.Agregar())
                {
                    MessageBox.Show("Compra creada correctamente", ":)", MessageBoxButtons.OK);
                    //TODO. crear reporte de la compra.
                    LimpiarForm();

                }



            }



        }
        private void TrasladoListaVisualAObjetoCompra()
        {
            //pasamos los datos del datatable que se usa graficamente a la list del objeto
            //MiCompraLocal

            foreach (DataRow fila in ListaProductos.Rows)
            {
                CompraDetalle nuevodetalle = new CompraDetalle();
                nuevodetalle.MiProducto.ProductoID = Convert.ToInt32(fila["ProductoID"]);
                nuevodetalle.Cantidad = Convert.ToDecimal(fila["Cantidad"]);
                nuevodetalle.PrecioUnitario = Convert.ToDecimal(fila["PrecioVentaUnitario"]);

                //una vez tenemos los datos en el nuevodetalle, se agrega ese objeto a la lista
                //de detalles de la compra local

                MiCompraLocal.ListaDetalles.Add(nuevodetalle);


            }

        }



        private bool ValidarCompra()
        { 
            bool R = false;
            if (!string.IsNullOrEmpty(TxtProveedorNombre.Text.Trim()) && 
                CboxCompraTipo.SelectedIndex >= 0 &&
                ListaProductos.Rows.Count > 0)
            {
                R = true;

            }
            else
            {
                if (string.IsNullOrEmpty(TxtProveedorNombre.Text.Trim()))
                {
                    MessageBox.Show("Se debe seleccionar un proveedor", "Error de Validacion", MessageBoxButtons.OK);

                    return false;
                }
                if (CboxCompraTipo.SelectedIndex == -1)
                {
                    MessageBox.Show("Se debe seleccionar un tipo de compra", "Error de Validacion", MessageBoxButtons.OK);

                    return false;
                }
                if (ListaProductos.Rows.Count == 0)
                {
                    MessageBox.Show("Debe haber almenos una fila en el detalle", "Error de Validacion", MessageBoxButtons.OK);

                    return false;
                }


            }






            return R;








        }



















    }
}
