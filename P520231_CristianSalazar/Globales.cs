﻿using System.Windows.Forms;


namespace P520231_CristianSalazar
{
    public static class Globales
    {

     //Estas propiedades al pertenecer a una clase static, se auto instancian
     //y se puede obtener acceso a ellas en la globalidad de la aplicacion.

     public static Form MiFormPrincipal = new Formularios.FrmMDI();

    public static Formularios.FrmUsuariosGestion MiFormGestionUsuarios =
         new Formularios.FrmUsuariosGestion();

    public static Formularios.FrmProveedorGestion MiFormGestionProveedor =
            new Formularios.FrmProveedorGestion();

    //debemos tener un objeto de tipo usuario que permita almacenar los datos del usuario que se haya logueado correctamente 
    public static Logica.Models.Usuario MiUsuarioGlobal = new Logica.Models.Usuario();


    public static Formularios.FrmRegistroCompra MiFormRegistroCompra = new Formularios.FrmRegistroCompra();

















    }
}
