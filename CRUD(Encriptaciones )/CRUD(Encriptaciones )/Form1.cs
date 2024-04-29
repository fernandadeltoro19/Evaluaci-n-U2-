using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CRUD_Encriptaciones__
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarDatos();
        }

        private MySqlConnection conexion;
        string cadenaConexion = "Server=localhost;Database=prueba;Uid=root;Pwd=;";

        private void CargarDatos()
        {
            conexion = new MySqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
                string consulta = "SELECT * FROM prueba2"; // Cambia prueba2 por el nombre de tu tabla
                MySqlCommand comando = new MySqlCommand(consulta, conexion);
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                dataGridView1.DataSource = tabla; // Enlazamos el DataGridView con la tabla

                // Opcional: ajustar las columnas automáticamente para que se vean todos los datos
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string correo = txtCorreo.Text;
            string contrasena = txtContrasena.Text;

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) ||
                string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Determinar qué tipo de encriptación se utilizará
            string tipoEncriptacion = "";
            if (radioButton1.Checked)
            {
                tipoEncriptacion = "MD5";
            }
            else if (radioButton2.Checked)
            {
                tipoEncriptacion = "SHA1";
            }
            else if (radioButton3.Checked)
            {
                tipoEncriptacion = "SHA256";
            }
            else
            {
                MessageBox.Show("Seleccione un tipo de encriptación.");
                return;
            }



            // Aplicar la encriptación al campo de contraseña
            string contrasenaEncriptada = EncriptarContrasena(contrasena, tipoEncriptacion);

            // Crear la conexión y realizar la inserción
            using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    string consulta = "INSERT INTO prueba2 (nombre, apellido, correo, contrasena) " +
                        "VALUES (@nombre, @apellido, @correo, @contrasena)";
                    MySqlCommand comando = new MySqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellido", apellido);
                    comando.Parameters.AddWithValue("@correo", correo);
                    comando.Parameters.AddWithValue("@contrasena", contrasenaEncriptada);
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Registro insertado correctamente.");
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show("Error al insertar el registro.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar el registro: " + ex.Message);
                }
            }
        }

        private string EncriptarContrasena(string contrasena, string tipoEncriptacion)
        {
            switch (tipoEncriptacion)
            {
                case "MD5":
                    return EncriptarMD5(contrasena);
                case "SHA1":
                    return EncriptarSHA1(contrasena);
                case "SHA256":
                    return EncriptarSHA256(contrasena);
                default:
                    throw new ArgumentException("Tipo de encriptación no válido.");
            }
        }

        private string EncriptarMD5(string texto)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        private string EncriptarSHA1(string texto)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
