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
            ListaUsuarios = new DataTable();
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

            DgLista.DataSource = ListaUsuarios;

        }
















        private void CargarListaRoles()
        {

            Logica.Models.Usuario_Rol MiRol = new Logica.Models.Usuario_Rol();
            DataTable dt = new DataTable();
            dt = MiRol.Listar();

            if (dt != null && dt.Rows.Count > 0)
            {
                CbRolesUsuario.ValueMember = "ID";
                CbRolesUsuario.DisplayMember = "Descrip";
                CbRolesUsuario.DataSource = dt;
                CbRolesUsuario.SelectedIndex = -1;



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
                DataGridViewRow MiFila = DgLista.SelectedRows[0];

                //lo que necesito es el valor del ID del usuario para realizar la consulta
                //y traer todos los dtos para llenar el objeto de usuario local

                int IDUsuario = Convert.ToInt32(MiFila.Cells["CUsuarioID"].Value);

                //para no asumir riesgos se reinstancia el usuario local
                MiUsuarioLocal = new Logica.Models.Usuario();

                //ahora le agregamos el valor obtenido por la fila al ID del usuario Local
                MiUsuarioLocal.UsuarioID = IDUsuario;

                //una vez que tengo el objeto local con el valor del ID
                //puedo ir a consular el usuario por ese ID y llenar el resto de atributos

                MiUsuarioLocal = MiUsuarioLocal.ConsultarPorIDRetornaUsuario();

                //validamos que el Usuario Local tenga Datos

                if (MiUsuarioLocal != null && MiUsuarioLocal.UsuarioID > 0)
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

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
        private void LimpiarFormulario()
        {
            TxtUsuarioID.Clear();
            TxtUsuarioNombre.Clear();
            TxtUsuarioCedula.Clear();
            TxtUsuarioTelefono.Clear();
            TxtUsuarioCorreo.Clear();
            TxtUsuarioContrasennia.Clear();



            CbRolesUsuario.SelectedIndex = -1;
            TxtUsuarioDireccion.Clear();


        }
        private bool ValidarDatosDigitados()
        {
            //evalua que se hayan digitado los campos de texto en el formulario
            bool R = false;

            if (!string.IsNullOrEmpty(TxtUsuarioNombre.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioCedula.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioTelefono.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioCorreo.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioContrasennia.Text.Trim()) &&
                CbRolesUsuario.SelectedIndex > -1)
            {
                R = true;

            }
            else
            {
                //QUE PASA CUANDO ALGO FALTA
                //NOMBRE
                if (string.IsNullOrEmpty(TxtUsuarioNombre.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar un nombre para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    TxtUsuarioNombre.Focus();
                    return false;

                }
                //CEDULA
                if (string.IsNullOrEmpty(TxtUsuarioCedula.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar una cedula para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    TxtUsuarioCedula.Focus();
                    return false;

                }
                //TELEFONO
                if (string.IsNullOrEmpty(TxtUsuarioTelefono.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar un telefono para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    TxtUsuarioTelefono.Focus();
                    return false;

                }
                //CORREO
                if (string.IsNullOrEmpty(TxtUsuarioCorreo.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar un correo para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    TxtUsuarioCorreo.Focus();
                    return false;

                }
                //CONTRASEÑA
                if (string.IsNullOrEmpty(TxtUsuarioContrasennia.Text.Trim()))
                {
                    MessageBox.Show("Debe digitar una contraseña para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    TxtUsuarioContrasennia.Focus();
                    return false;

                }
                //ROL DE USUARIO
                if (CbRolesUsuario.SelectedIndex == -1)
                {
                    MessageBox.Show("Debe seleccionar un rol para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                    CbRolesUsuario.Focus();
                    return false;

                }

            }


            return R;
        }



        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosDigitados())
            {



                //estas variables almacenan el resultado de lasa consultas por correo y cedula
                bool CedulaOk;
                bool EmailOK;

                MiUsuarioLocal = new Logica.Models.Usuario();





                //llenar los valores de los atributos con los datos digitados en el form
                MiUsuarioLocal.UsuarioNombre = TxtUsuarioNombre.Text.Trim();
                MiUsuarioLocal.UsuarioCedula = TxtUsuarioCedula.Text.Trim();
                MiUsuarioLocal.UsuarioTelefono = TxtUsuarioTelefono.Text.Trim();
                MiUsuarioLocal.UsuarioCorreo = TxtUsuarioCorreo.Text.Trim();
                MiUsuarioLocal.UsuarioContrasennia = TxtUsuarioContrasennia.Text.Trim();
                //COMPOSICION DEL ROL
                MiUsuarioLocal.MiRolTipo.UsuarioRolId = Convert.ToInt32(CbRolesUsuario.SelectedValue);
                MiUsuarioLocal.UsuarioDireccion = TxtUsuarioDireccion.Text.Trim();

                //realizar las consultas por email y por cedula
                //pasos 1.3 y 1.3.6
                CedulaOk = MiUsuarioLocal.ConsultarPorCedula();

                //pasos 1.4 y 1.4.6
                EmailOK = MiUsuarioLocal.ConsultarPorEmail();

                //paso 1.5
                if (CedulaOk == false && EmailOK == false)
                {
                    //se puede agregar el usuario  ya que no existe un usuario con la cedula y el email digitados
                    //se solicita al usuario confirmacion de que si quiere agregar o no al usuario
                    string msg = string.Format("¿Está seguro que desea agregar al Usuario {0}?", MiUsuarioLocal.UsuarioNombre);
                    DialogResult respuesta = MessageBox.Show(msg, "??", MessageBoxButtons.YesNo);

                    if (respuesta == DialogResult.Yes)
                    {
                        bool ok = MiUsuarioLocal.Agregar();

                        if (ok)
                        {
                            MessageBox.Show("Usuario guardado correctamente!", ":)", MessageBoxButtons.OK);

                            LimpiarFormulario();

                            CargarListaDeUsuarios();
                        }

                        else
                        {
                            MessageBox.Show("El Usuario no se pudo agregar!", ":/", MessageBoxButtons.OK);
                        }

                    }

                }
                else
                {
                    //indicar al usuario si falla alguna consulta
                    if (CedulaOk)
                    {
                        MessageBox.Show("Ya existe un Usuario previamente almacenado con esa cedula!", "Error de Validación", MessageBoxButtons.OK);
                        return;
                    }
                    if (EmailOK)
                    {
                        MessageBox.Show("Ya existe un Usuario previamente almacenado con ese correo!", "Error de Validación", MessageBoxButtons.OK);
                        return;
                    }
                }
















            }












        }

    }
}
