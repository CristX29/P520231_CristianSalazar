using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Models
{
    public class Usuario
    {
        //propiedades simples
        public int UsuarioID { get; set; }
        public string UsuarioCorreo { get; set; }
        public string UsuarioContrasennia { get; set; }
        public string UsuarioNombre { get;set; }
        public string UsuarioCedula { get; set;}
        public string UsuarioTelefono{ get;set;}
        public string UsuarioDireccion { get; set; }
        public bool Activo { get; set; }

        //propiedades compuestas
        Usuario_Rol MiRolTipo { get; set; }
        //Normalmente cuando tenemos propiedades compuestas con tipos que
        //hemos programado nosotros mismos, debemos instanciar dichas propiedades
        //ya que son objetos. Para esto recomiendo hacerlo en el
        //constructor de la clase

        //CTOR para crear el constructor
        public Usuario()
        {
         //al crear una nueva instancia de la clase Usuario,
         //se ejecuta el codigo de ete constructor, y
         //tambien se crea una nueva instancia de la clase
         //usuario_rol para el objeto MiRolTipo.
         MiRolTipo= new Usuario_Rol();
        }
        //FUNCIONES Y METODOS

        public Boolean Agregar()
        {
        //Cuando la funcion devuelve un booleano, me gusta
        //iniciarlizar la variable de retorno en 
        //False(tiende a negativo el resultado de la fn)
        //y SOLO si la operacion( en este caso insert) sale correcta
        //se cambia el valor de R a true

         bool R = false;
            //aca va el codigo de la fn que invoca a un procedimiento almacenado
            //que contiene el DML insert.






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

        public bool ConsultarPorID()
        {
            bool R = false;


            return R;

         }


        public bool ConsultarPorCedula()
        {
            bool R = false;


            return R;

        }

        public bool ConsultarPorEmail()
        {
            bool R = false;


            return R;

        }

        public DataTable ListarActivos()
        {
            DataTable R = new DataTable();



            return R;

        }
        public DataTable ListarInactivos()
        {
            DataTable R = new DataTable();


            return R;

        }

        public Usuario ValidarUsuarios(string pEmail, string pContrasennia)
        {

            Usuario R = new Usuario();


            return R;



        }

































    }
}
