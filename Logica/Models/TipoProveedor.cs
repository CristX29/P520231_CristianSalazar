using Logica.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class TipoProveedor
    {
        public int ProveedorTipoID { get; set; }
        public string ProveedorTipoDescripcion { get; set; }

        public DataTable Listar()
        {
            DataTable R = new DataTable();
            Conexion MiCnn = new Conexion();

            R = MiCnn.EjecutarSELECT("SPTipoProveedorListar");

            return R; 


        }




    }
}
