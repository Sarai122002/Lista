using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListaMascota
{
    public partial class FrmDatos : Form
    {
        List<ClsLista> MiLista = new List<ClsLista>();

        public FrmDatos()
        {
            InitializeComponent();
            tslConsultar.Enabled = false;
            tslEliminar.Enabled = false;

        }
        private void tslRegistrar_Click(object sender, EventArgs e)
        {
            if (ValidarNombre() == false)
            {
                return;
            }
            if (ValidarRaza() == false)
            {
                return;
            }
            if (ValidarEdad() == false)
            {
                return;
            }
            if (Existe(txtNombre.Text))
            {
                erpError.SetError(txtNombre, "La mascota con ese nombre ya esta registrada");
                txtNombre.Focus();
                LimpiarCrontroles();
                return;
            }
            erpError.SetError(txtNombre, "");

          //creamos el objeto de la clas lista
            ClsLista miMascota = new ClsLista();
            miMascota.Nombre = txtNombre.Text;
            miMascota.Raza = cmbRaza.SelectedItem.ToString();
            miMascota.Edad = int.Parse(txtEdad.Text);
            MiLista.Add(miMascota);
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = MiLista;
            LimpiarCrontroles();
            txtNombre.Focus();
            tslConsultar.Enabled = true;


        }

        // Metodo que valida que no ingrsen con el mismo nombre
        private bool Existe(string nombre)
        {
            foreach (ClsLista miMascota in MiLista) 
            {
                if (miMascota.Nombre==nombre)
                {
                    return true;
                }
            }
            return false; 
        }

        private bool ValidarEdad()
        {
            int Edad;
            if (!int.TryParse(txtEdad.Text, out Edad) || txtEdad.Text == "")
            {
                erpError.SetError(txtEdad, "Debe ingresar un valor numerico para la edad");
                txtEdad.Clear();
                txtEdad.Focus();
                return false;

            }
            else
            {
                erpError.SetError(txtEdad, "");
                return true;
            }
        }

        private bool ValidarNombre()
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                erpError.SetError(txtNombre, "Debe ingresar un nombre");
                return false;
            }
            else
            {
                erpError.SetError(txtNombre, "");
                return true;
            }
        }

        //Validar la raza
        private bool ValidarRaza()
        {
            if (string.IsNullOrEmpty(cmbRaza.Text))
            {
                erpError.SetError(cmbRaza, "Debe Seleccionar una raza");
                return false;
            }
            else
            {
                erpError.SetError(cmbRaza, "");
                return true;
            }
        }
        //Metodo para limpiar los controles
        private void LimpiarCrontroles()
        {
            txtNombre.Clear();
            txtEdad.Clear();
            cmbRaza.SelectedIndex = 0;


        }

        private void tslSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Evento de la opcion consultar
        private void tslConsultar_Click(object sender, EventArgs e)
        {
            if (ValidarNombre() == false)
            {
                return;
            }
            ClsLista miMascota = GetMascota(txtNombre.Text);
            if (miMascota == null)
            {
                erpError.SetError(txtNombre, "La mascota no se encuentra registrada en la lista");
                LimpiarCrontroles();
                txtNombre.Focus();
                return;
            }
            else
            {
                erpError.SetError(txtNombre, "");
                txtNombre.Text = miMascota.Nombre;
                cmbRaza.SelectedItem = miMascota.Raza;
                txtEdad.Text = miMascota.Edad.ToString();
                tslEliminar.Enabled = true;


            }

        }

        //Metodo obtener o consultar mascota
        private ClsLista GetMascota(string nombre)
        {
            return MiLista.Find(mascota => mascota.Nombre.Contains(nombre));
        }

        //Evento de la opcion eliminar
        private void tslEliminar_Click(object sender, EventArgs e)
        {

            if (txtEdad.Text == "")
            {
                erpError.SetError(txtNombre, "Debe  consultar la mascota a eliminar");
                LimpiarCrontroles();
                txtNombre.Focus();
                tslEliminar.Enabled = false;
                return;
            }
            else
            {
                erpError.SetError(txtNombre, "");
                DialogResult Respuesta = MessageBox.Show("Esta seguro de eliminar el registro?", "Confirmacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (Respuesta == DialogResult.Yes)
                {
                    foreach (ClsLista miMascota in MiLista)
                    {
                        if (miMascota.Nombre == txtNombre.Text) 
                        {
                            MiLista.Remove(miMascota);
                            break;
                        }
                    }
                    LimpiarCrontroles();
                    dgvDatos.DataSource = null;
                    dgvDatos.DataSource = MiLista;
                }

            }
        }
    }
}

