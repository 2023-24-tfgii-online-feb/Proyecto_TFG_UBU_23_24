using System.Data; // Se utiliza para trabajar con datos en formas tabulares.
using MySql.Data.MySqlClient; // Proporciona las clases necesarias para trabajar con MySQL.

namespace InverIoT
{
    public partial class frmHistorico : Form
    {
        // Constructor del formulario de histórico.
        public frmHistorico()
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario definidos en el diseñador.

            cargarDatosSensores(); // Llama al método para cargar los datos de los sensores en el DataGridView.

            // Ajusta el tamaño y posición del formulario según el área de trabajo de la pantalla principal.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            dgvHistorico.DoubleBuffered(true); // Habilita el doble buffer para el DataGridView para reducir el parpadeo.

            // Configura el modo de autoajuste de las columnas del DataGridView.
            for (int i = 1; i < dgvHistorico.Columns.Count - 1; i++)
            {
                dgvHistorico.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Rellena el espacio disponible.
            }
            dgvHistorico.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells; // Ajusta al contenido de las celdas.
            dgvHistorico.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader; // Ajusta al tamaño del encabezado de columna.
        }

        // Método para cargar los datos de los sensores desde la base de datos y mostrarlos en el DataGridView.
        private void cargarDatosSensores()
        {
            string connectionString = Funciones.conexionMySQL; // Obtiene la cadena de conexión a la base de datos.
            string query = "SELECT * FROM sensores ORDER by id DESC"; // Consulta SQL para obtener todos los registros ordenados por ID de forma descendente.

            using (var connection = new MySqlConnection(connectionString)) // Crea una nueva conexión a la base de datos.
            {
                try
                {
                    connection.Open(); // Abre la conexión a la base de datos.
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection)) // Crea un adaptador de datos para la consulta.
                    {
                        DataTable dt = new DataTable(); // Crea una tabla de datos en memoria.
                        adapter.Fill(dt); // Llena la tabla de datos con los resultados de la consulta.
                        dgvHistorico.DataSource = dt; // Asigna la tabla de datos como fuente de datos del DataGridView.

                        // Personaliza los nombres de las columnas en el DataGridView.
                        dgvHistorico.Columns[0].HeaderText = "Id";
                        dgvHistorico.Columns[1].HeaderText = "Fecha";
                        dgvHistorico.Columns[2].HeaderText = "Hora";
                        dgvHistorico.Columns[3].HeaderText = "Temperatura";
                        dgvHistorico.Columns[4].HeaderText = "Humedad Ambiente";
                        dgvHistorico.Columns[5].HeaderText = "Luminosidad";
                        dgvHistorico.Columns[6].HeaderText = "Humedad Suelo";
                    }
                }
                catch (Exception ex) // Captura cualquier excepción que ocurra durante la carga de datos.
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}"); // Muestra un mensaje de error.
                }
            }
        }
    }
}