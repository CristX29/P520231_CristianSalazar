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
            ActivarAgregar();
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
                LimpiarFormulario();




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

                    ActivarEditarEliminar();
                }




            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            DgLista.ClearSelection();
            ActivarAgregar();
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

        private void ActivarAgregar()
        {
            BtnAgregar.Enabled = true;
            BtnModificar.Enabled = false;
            BtnEliminar.Enabled = false;

        }

        private void ActivarEditarEliminar()
        {
            BtnAgregar.Enabled = false;
            BtnModificar.Enabled = true;
            BtnEliminar.Enabled = true;
        }



        private bool ValidarDatosDigitados(bool OmitirPassword = false)
        {
            //evalua que se hayan digitado los campos de texto en el formulario
            bool R = false;

            if (!string.IsNullOrEmpty(TxtUsuarioNombre.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioCedula.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioTelefono.Text.Trim()) &&
                !string.IsNullOrEmpty(TxtUsuarioCorreo.Text.Trim()) &&

                CbRolesUsuario.SelectedIndex > -1)
            {

                if (OmitirPassword)
                {
                    //PARA EDITAR Si el password se omite, ya paso la evaluacion a este punto, retorna true
                    R = true;
                }
                else
                {
                    //PARA AGREGAR en caso en el que haya que evaluar el password sebe agregar otra condicion
                    //logica
                    if (!string.IsNullOrEmpty(TxtUsuarioContrasennia.Text.Trim()))
                    {
                        R = true;
                    }
                    else
                    {

                        //en el caso en el que haga falta la contraseña, se le indica al usuario
                        //CONTRASEÑA

                        MessageBox.Show("Debe digitar una contraseña para el usuario", "Error de Validacion", MessageBoxButtons.OK);
                        TxtUsuarioContrasennia.Focus();
                        return false;


                    }


                }

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

        private void DgLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (ValidarDatosDigitados(true))
            {
                // NO ES NECESARIO CAPTURAR EL ID DESDE EL CUADRO DE TEXTO YA QUE
                //AL CONSULTARLO (CUANDO SELECCIONAMOS EL USUARIO DEL DATAGRID), YA
                //TENEMOS DATOS EN EL ID
                //ESTE ID NO PUEDE SER MODIFICADO, LOS DEMAS TRIBUTOS SI

                MiUsuarioLocal.UsuarioNombre = TxtUsuarioNombre.Text.Trim();
                MiUsuarioLocal.UsuarioCedula = TxtUsuarioCedula.Text.Trim();
                MiUsuarioLocal.UsuarioTelefono = TxtUsuarioTelefono.Text.Trim();
                MiUsuarioLocal.UsuarioCorreo = TxtUsuarioCorreo.Text.Trim();
                //Como el cuadro de texto de la contraseña tiene un caracter en blanco,
                //puedo asignar sin problema el valor del cuadro de texto al atributo en el SP
                //se evalua si tiene o no datos.
                MiUsuarioLocal.UsuarioContrasennia = TxtUsuarioContrasennia.Text.Trim();
                MiUsuarioLocal.MiRolTipo.UsuarioRolId = Convert.ToInt32(CbRolesUsuario.SelectedValue);
                MiUsuarioLocal.UsuarioDireccion = TxtUsuarioDireccion.Text.Trim();

                //segun el diagrama de casos de uso expandido y la secuencia normal para un CRUD (editar)
                //Es habitual consultar por ID el item que se va a modificar para asegurarse que
                //en el lapso de tiempo entre que se selecciono el usuario y se modificaron los datos
                //en pantalla, aun exista el registro en la BD. (existe una posibilidad de que ya no exista
                //debido a que en entornos donde hayan muchos usuarios trabajando en el sistema algun otro
                //este modificando el mismo registro, esto se llama concurrencia.

                if (MiUsuarioLocal.ConsultarPorID())
                {
                    DialogResult respuesta = MessageBox.Show("¿Está seguro de modificar el usuario?", "???",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (respuesta == DialogResult.Yes)
                    {
                        if (MiUsuarioLocal.Editar())

                        {
                            MessageBox.Show("El usuario se modifico correctamente", ":)", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            CargarListaDeUsuarios();

                        }



                    }



                }

            }

        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (MiUsuarioLocal.UsuarioID > 0 && MiUsuarioLocal.ConsultarPorID() )
            {
                //tomando en cuenta que puedo estar viendo los usuarios activos o inactivos
                //este boton podria servir tanto para activar como desactivar los usuarios
                //El checkbox de la parte superior del form me sirve para identificar esta accion
                if (CboxVerActivos.Checked)
                {
                    //DESACTIVAR USUARIO
                    DialogResult r = MessageBox.Show("¿Está seguro de eliminar el usuario?", "???",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (r == DialogResult.Yes)
                    {
                        if (MiUsuarioLocal.Eliminar())
                        {
                            MessageBox.Show("El Usuario ha sido eliminado correctamente", "!!!", MessageBoxButtons.OK);
                            LimpiarFormulario();
                            CargarListaDeUsuarios();
                        }
                    }

                }
                else
                {
                    //ACTIVAR USUARIO
                }

            }


        }
    }
}
