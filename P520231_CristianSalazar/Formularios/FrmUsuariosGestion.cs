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
    public partial class FrmUsuariosGestion : Form
    {
        //por order es mejor crear objetos locales que permitan
        //la gestion del tema que estamos tratando
        //usar objetos individuales en las funciones puede provocar desorden y 
        //complicar mas la lectura del codigo fuente

        //objeto local para usuario

        private Logica.Models.Usuario MiUsuarioLocal { get; set; }

        //lista local de usuarios que se visualizan en el datagridview
        private DataTable ListaUsuarios { get; set; }




        public FrmUsuariosGestion()
        {
            InitializeComponent();

            MiUsuarioLocal = new Logica.Models.Usuario();
            ListaUsuarios= new DataTable();
        }

        private void FrmUsuariosGestion_Load(object sender, EventArgs e)
        {
            //Definimos el padre MDI
            MdiParent = Globales.MiFormPrincipal;
            CargarListaRoles();
            CargarListaDeUsuarios();
        }

        private void CargarListaDeUsuarios() 
        { 
        //resetear la lista de usuarios haciendo re instancia del objeto
        ListaUsuarios = new DataTable();

            if (CboxVerActivos.Checked)
            {
                ListaUsuarios = MiUsuarioLocal.ListarActivos();
            }
            else
            {
                ListaUsuarios = MiUsuarioLocal.ListarInactivos();
            }
            
            DgLista.DataSource= ListaUsuarios;
        
        }
















       private void CargarListaRoles()
        {

            Logica.Models.Usuario_Rol MiRol = new Logica.Models.Usuario_Rol();
            DataTable dt = new DataTable();
            dt = MiRol.Listar();

            if (dt != null && dt.Rows.Count > 0 )
            {
                CbRolesUsuario.ValueMember = "ID";
                CbRolesUsuario.DisplayMember= "Descrip";
                CbRolesUsuario.DataSource = dt;
                CbRolesUsuario.SelectedIndex = - 1;



            }




        }

        private void DgLista_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DgLista.ClearSelection();
        }

        private void DgLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //cuando seleccionemos una fila del Datagrid, se debe cargar la info de dicho usuario
            //en el usuario local y luego dibujar esa info en los controles graficos

            if (DgLista.SelectedRows.Count == 1)
            {
                //TO DO Limpiar el form


                //de la coleccion de filas selecionadas(que en este caso es solo una)
                //seleccionamos la fila en indice 0, osea la primera
                DataGridViewRow MiFila =  DgLista.SelectedRows[0];

                //lo que necesito es el valor del ID del usuario para realizar la consulta
                //y traer todos los dtos para llenar el objeto de usuario local

                int IDUsuario = Convert.ToInt32(MiFila.Cells["CUsuarioID"].Value );

                //para no asumir riesgos se reinstancia el usuario local
                MiUsuarioLocal = new Logica.Models.Usuario();

                //ahora le agregamos el valor obtenido por la fila al ID del usuario Local
                MiUsuarioLocal.UsuarioID= IDUsuario;

                //una vez que tengo el objeto local con el valor del ID
                //puedo ir a consular el usuario por ese ID y llenar el resto de atributos

                MiUsuarioLocal = MiUsuarioLocal.ConsultarPorIDRetornaUsuario();

                //validamos que el Usuario Local tenga Datos

                if (MiUsuarioLocal != null && MiUsuarioLocal.UsuarioID > 0 )
                {
                    // si lo cargamos correctamente el usuario local llenamos los controles
                    TxtUsuarioID.Text = Convert.ToString(MiUsuarioLocal.UsuarioID);
                    TxtUsuarioNombre.Text = MiUsuarioLocal.UsuarioNombre;
                    TxtUsuarioCedula.Text = MiUsuarioLocal.UsuarioCedula;
                    TxtUsuarioTelefono.Text = MiUsuarioLocal.UsuarioTelefono;
                    TxtUsuarioCorreo.Text = MiUsuarioLocal.UsuarioCorreo;
                    TxtUsuarioDireccion.Text = MiUsuarioLocal.UsuarioDireccion;


                    //combobox
                    CbRolesUsuario.SelectedValue = MiUsuarioLocal.MiRolTipo.UsuarioRolId;

                    // TO DO: DESACTIVAR BOTONES NO NECESARIOS EN ESTE CASO AGREGAR
                }
                



            }
        }
    }
}
