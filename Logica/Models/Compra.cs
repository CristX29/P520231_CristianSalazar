using Logica.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Compra
    {
        //propiedades
        public int CompraID { get; set; }
        public DateTime CompraFecha { get; set; }
        public int CompraNumero { get; set; }
        public string CompraNotas { get; set; }
        public bool Activo { get; set; }



        //composiciones simples
        public Usuario MiUsuario { get; set; }
        public Proveedor MiProveedor{ get; set; }
        public TipoCompra MiTipoCompra { get; set; }    

        //composicion en lista o complejas
        public List<CompraDetalle> ListaDetalles { get; set; }

        //constructor
        public Compra()
        {
            MiUsuario = new Usuario();
            MiProveedor= new Proveedor();
            MiTipoCompra = new TipoCompra();
            ListaDetalles= new List<CompraDetalle>();
        }
        //funciones

        public DataTable CargarEsquemaDetalle()
        {
            DataTable R = new DataTable();
            Conexion MiCnn = new Conexion();

            R = MiCnn.EjecutarSELECT("SPCompraDetalleEsquema", true);
            // como estamos cargando el esquema tambien viene la indicacion del pk
            //se debe anular esa opcion
            R.PrimaryKey = null;


            return R;



        }

        //funcion de agregar en una estructura encabezado-detalle
        public bool Agregar()
        {
            bool R = false;
            Conexion MiCnn = new Conexion();

            //lista de parametros

            MiCnn.ListaDeParametros.Add(new SqlParameter("@IDProveedor", this.MiProveedor.ProveedorID));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@IDTipoCompra", this.MiTipoCompra.CompraTipoID));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@IDUsuario", this.MiUsuario.UsuarioID));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Notas", this.CompraNotas));

            Object retorno = MiCnn.EjecutarSELECTEscalar("SPCompraAgregar");
            //como la DEVOLUCION PUEDE SER CUALQUIER TIPO(STRING,DECIMAL,INT,ETC)
            //se captura la respuesta en un object y luego se hace la conversion al tipo correcto
            //en este caso es un INT

            int IDCreada;
            if (retorno != null)
            {
                try
                {
                    IDCreada = Convert.ToInt32(retorno.ToString());
                    this.CompraID = IDCreada;
                    //hasta este punto se puede asegurar el insert en el encabezado salio
                    //correctamente
                    //se procede con los insert en el detalle 

                    foreach (CompraDetalle item in this.ListaDetalles)
                    {
                        Conexion MiCnnDetalle = new Conexion();
                        //lista de parametros del SP de insert a detalle
                        MiCnnDetalle.ListaDeParametros.Add(new SqlParameter("@IDCompra", IDCreada));
                        MiCnnDetalle.ListaDeParametros.Add(new SqlParameter("@IDProducto", item.MiProducto.ProductoID));
                        MiCnnDetalle.ListaDeParametros.Add(new SqlParameter("@Cantidad", item.Cantidad));
                        MiCnnDetalle.ListaDeParametros.Add(new SqlParameter("@Precio", item.PrecioUnitario));

                        MiCnnDetalle.EjecutarInsertUpdateDelete("SPCompraDetalleAgregar");



                    }
                    R = true;


                }
                catch (Exception)
                {

                    throw;
                }


            }

            return R;

        }




    }
}
