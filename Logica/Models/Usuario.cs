﻿using Logica.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


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
        public Usuario_Rol MiRolTipo { get; set; }


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
           
            //pasos 1.6.1 y 1.6.2
            Conexion MiCnn= new Conexion();

            //ENCRIPTAR CONTRASENNIA
            Crypto MiEncriptador= new Crypto();
            string ContrasenniaEncriptada = MiEncriptador.EncriptarEnUnSentido(this.UsuarioContrasennia);

            MiCnn.ListaDeParametros.Add(new SqlParameter("@Correo", this.UsuarioCorreo));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Contrasennia", ContrasenniaEncriptada));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Nombre", this.UsuarioNombre));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Cedula", this.UsuarioCedula));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Telefono", this.UsuarioTelefono));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Direccion", this.UsuarioDireccion));
            //normalmente los foreign keys tienen que ver con composiciones, en este caso tenemos que extraer el valor del objeto compuesto
            //MiRolTipo
            MiCnn.ListaDeParametros.Add(new SqlParameter("@IdRol", this.MiRolTipo.UsuarioRolId));

            //pasos 1.6.3 y 1.6.4
            int resultado = MiCnn.EjecutarInsertUpdateDelete("SPUsuarioAgregar");

            //paso 1.6.5
            if (resultado > 0)
            {
                R = true;
            }


            return R;



        }

        public bool Editar()
        {
            bool R = false;

            Conexion MiCnn = new Conexion();

            //TODO ENCRIPTAR CONTRASENNIA
            //ENCRIPTAR CONTRASENNIA
            Crypto MiEncriptador = new Crypto();
            string ContrasenniaEncriptada = MiEncriptador.EncriptarEnUnSentido(this.UsuarioContrasennia);

            MiCnn.ListaDeParametros.Add(new SqlParameter("@Correo", this.UsuarioCorreo));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Contrasennia", ContrasenniaEncriptada));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Nombre", this.UsuarioNombre));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Cedula", this.UsuarioCedula));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Telefono", this.UsuarioTelefono));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Direccion", this.UsuarioDireccion));
           
             
            MiCnn.ListaDeParametros.Add(new SqlParameter("@IdRol", this.MiRolTipo.UsuarioRolId));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.UsuarioID));

            int resultado = MiCnn.EjecutarInsertUpdateDelete("SPUsuarioModificar");

            
            if (resultado > 0)
            {
                R = true;
            }




            return R;
        
        
        
        
        
        }

        public bool Eliminar()

        {

            bool R = false;
            
            Conexion MiCnn = new Conexion();
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.UsuarioID));

            int respuesta = MiCnn.EjecutarInsertUpdateDelete("SPUsuarioDesactivar");
            if (respuesta > 0)
            {
                R = true;
            }

            return R;




        }

        public bool Activar()
        {
            bool R=false;

            Conexion MiCnn = new Conexion();
            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.UsuarioID));

            int respuesta = MiCnn.EjecutarInsertUpdateDelete("SPUsuarioActivar");
            if (respuesta > 0)
            {
                R = true;
            }

            return R;
        }
        


            public bool ConsultarPorID() { 
            bool R = false;

            Conexion MiCnn = new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.UsuarioID));
            //NECESITO UN DATATABLE PARA CAPTURAR LA INFO DEL USUARIO
            DataTable dt = new DataTable();

            dt = MiCnn.EjecutarSELECT("SPUsuarioConsultarPorID");

            if (dt != null && dt.Rows.Count > 0)
            {
                R = true;
            }


            return R;

         }


        public Usuario ConsultarPorIDRetornaUsuario()
        {
            Usuario R = new Usuario();

            Conexion MiCnn= new Conexion();

            MiCnn.ListaDeParametros.Add(new SqlParameter("@ID", this.UsuarioID));
            //NECESITO UN DATATABLE PARA CAPTURAR LA INFO DEL USUARIO
            DataTable dt = new DataTable();

            dt = MiCnn.EjecutarSELECT("SPUsuarioConsultarPorID");

            if (dt != null && dt.Rows.Count > 0)
            {
                //Esta consulta deberia tener solo un registro 
                //se crea un objeto de tipo DataRow para capturar la info contenida en
                //index 0 del dt(DataTable)
                DataRow dr = dt.Rows[0];

                R.UsuarioID = Convert.ToInt32(dr["UsuarioID"]);
                R.UsuarioNombre = Convert.ToString(dr["UsuarioNombre"]);
                R.UsuarioCedula = Convert.ToString(dr["UsuarioCedula"]);
                R.UsuarioCorreo = Convert.ToString(dr["UsuarioCorreo"]);
                R.UsuarioTelefono = Convert.ToString(dr["UsuarioTelefono"]);
                R.UsuarioDireccion = Convert.ToString(dr["UsuarioDireccion"]);

                R.UsuarioContrasennia = String.Empty;

                //composiciones

                R.MiRolTipo.UsuarioRolId = Convert.ToInt32(dr["UsuarioRolID"]);
                R.MiRolTipo.UsuarioRolDescripcion = Convert.ToString(dr["UsuarioRolDescripcion"]);

            }

            return R;

        }






        public bool ConsultarPorCedula()
        {
            bool R = false;

            //paso 1.3.1 y 1.3.2
            Conexion MiCnn = new Conexion();
            //agregamos el parametro de cedula
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Cedula", this.UsuarioCedula));


            DataTable consulta = new DataTable();

            //paso 1.3.3 y 1.3.4
            consulta = MiCnn.EjecutarSELECT("SPUsuarioConsultarPorCedula");

            //paso 1-3-5
            if (consulta != null && consulta.Rows.Count > 0)
            {
                R = true;

            }

            return R;

        }

        public bool ConsultarPorEmail()
        {
            bool R = false;

            //paso 1.4.1 y 1.2.2
            Conexion MiCnn = new Conexion();
            //agregamos el parametro de correo
            MiCnn.ListaDeParametros.Add(new SqlParameter("@Correo", this.UsuarioCorreo));


            DataTable consulta = new DataTable();

            //paso 1.4.3 y 1.4.4
            consulta = MiCnn.EjecutarSELECT("SPUsuarioConsultarPorEmail");

            //paso 1-4-5
            if (consulta != null && consulta.Rows.Count > 0)
            {
                R = true;

            }

            return R;

        }




            

        

        public DataTable ListarActivos(string pFiltroBusqueda)
        {

            DataTable R = new DataTable();

            Conexion MiCnn = new Conexion();
            //en este caso como el procedimiento almacenado tiene un parametro
            //debemos definir ese parametro en la lista de parametros del objeto de conexión

            MiCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", true));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@FiltroBusqueda", pFiltroBusqueda));
            R = MiCnn.EjecutarSELECT("SPUsuariosListar"); 

            return R;

        }
        public DataTable ListarInactivos(string pFiltroBusqueda)
        {
            DataTable R = new DataTable();

            Conexion MiCnn = new Conexion();
            //en este caso como el procedimiento almacenado tiene un parametro
            //debemos definir ese parametro en la lista de parametros del objeto de conexión

            MiCnn.ListaDeParametros.Add(new SqlParameter("@VerActivos", false));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@FiltroBusqueda", pFiltroBusqueda));
            R = MiCnn.EjecutarSELECT("SPUsuariosListar");

            return R;
            return R;

        }

        public Usuario ValidarUsuarios(string pEmail, string pContrasennia)
        {

            Usuario R = new Usuario();

            Conexion MiCnn = new Conexion();

            Crypto crypto = new Crypto();
            string ContrasenniaEncriptada = crypto.EncriptarEnUnSentido(pContrasennia);



            MiCnn.ListaDeParametros.Add(new SqlParameter("@usuario", pEmail));
            MiCnn.ListaDeParametros.Add(new SqlParameter("@password", ContrasenniaEncriptada));

            //NECESITO UN DATATABLE PARA CAPTURAR LA INFO DEL USUARIO
            DataTable dt = new DataTable();

            dt = MiCnn.EjecutarSELECT("SPUsuarioValidarIngreso");

            if (dt != null && dt.Rows.Count > 0)
            {
                //Esta consulta deberia tener solo un registro 
                //se crea un objeto de tipo DataRow para capturar la info contenida en
                //index 0 del dt(DataTable)
                DataRow dr = dt.Rows[0];

                R.UsuarioID = Convert.ToInt32(dr["UsuarioID"]);
                R.UsuarioNombre = Convert.ToString(dr["UsuarioNombre"]);
                R.UsuarioCedula = Convert.ToString(dr["UsuarioCedula"]);
                R.UsuarioCorreo = Convert.ToString(dr["UsuarioCorreo"]);
                R.UsuarioTelefono = Convert.ToString(dr["UsuarioTelefono"]);
                R.UsuarioDireccion = Convert.ToString(dr["UsuarioDireccion"]);

                R.UsuarioContrasennia = String.Empty;

                //composiciones

                R.MiRolTipo.UsuarioRolId = Convert.ToInt32(dr["UsuarioRolID"]);
                R.MiRolTipo.UsuarioRolDescripcion = Convert.ToString(dr["UsuarioRolDescripcion"]);

            }

            return R;



        }

































    }
}
