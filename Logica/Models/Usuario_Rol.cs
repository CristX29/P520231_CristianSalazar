using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Usuario_Rol
    {
        //Primero se digita las propiedades de la clase
        //Esta linea es lo mismo que hacer metodo get y set pero mas resumido
        public int UsuarioRolId { get; set; }

        //SERIA LO MISMO QUE HACER ESTO:
        //private int usuarioRolID;

        //public int UsuarioRolID
        //{
        //    get { return usuarioRolID; }
        //    set { usuarioRolID = value; }
        //}

        public string UsuarioRolDescripcion { get; set; }
        //Luego de escribir las propiedades simples
        //Las propiedades compuestas (en este caso no hay)

        //Por ultimo, se escriben las funciones y metodos.

        public DataTable Listar()
        {
            
            DataTable R = new DataTable();
            
            Services.Conexion MiCnn = new Services.Conexion();
            R = MiCnn.EjecutarSELECT("SPUsuarioRolListar");




            return R;




        }




    }


}
