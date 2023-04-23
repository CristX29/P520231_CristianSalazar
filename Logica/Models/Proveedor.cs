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
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string ProveedorNombre { get; set; }
        public string ProveedorCedula { get;set; }
        public string ProveedorEmail { get; set; }
        public string ProveedorDireccion { get; set;}
        public string ProveedorNotas { get; set; }
        public bool Activo { get; set; }

        public TipoProveedor MiTipoProveedor { get; set; }

        public Proveedor()
        {
            MiTipoProveedor = new TipoProveedor();
        }


        public bool Agregar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProveedorNombre", this.ProveedorNombre));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProveedorCedula", this.ProveedorCedula));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProveedorEmail", this.ProveedorEmail));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ProveedorDireccion", this.ProveedorDireccion));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Proveedornotas", this.ProveedorNotas));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@TipoRol", this.MiTipoProveedor.ProveedorTipoID));


            int resultado = MiCnn.EjecutarInsertUpdateDelete("SPProveedorAgregar");

            if (resultado > 0)
            {
                R = true;
            }
            return R;

        }
        public bool Editar()
        {
            bool R = false;


            return R;

        }

        public bool Eliminar()
        {
            bool R = false;


            return R;

        }

        public Proveedor ConsultarPorCedula(string cedula)
        {

            Proveedor R = new Proveedor();


            return R;



        }
        public Proveedor ConsultarPorEmail(string email)
        {

            Proveedor R = new Proveedor();


            return R;



        }
        public Proveedor ConsultarPorID(int ID)
        {

            Proveedor R = new Proveedor();


            return R;



        }

        public DataTable ListarProveedor(bool VerActivos = true, string FiltroBusqueda = " ")
        {
            DataTable R= new DataTable();
            Conexion MiCnn= new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", true));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@FiltroBusqueda", FiltroBusqueda));

            R = MiCnn.EjecutarSELECT("SPProveedorListar");


            return R;


        }
        
        public DataTable ListarProveedorEnGestion(string pFiltroBusqueda)
        {

            DataTable R = new DataTable();
            Conexion MiCnn = new Conexion();
            MiCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", true));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@FiltroBusqueda", pFiltroBusqueda));

            R = MiCnn.EjecutarSELECT("SPProveedorListarProveedorGestion");





            return R;
        }








    }
}
