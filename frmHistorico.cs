
using System.Data;
using MySql.Data.MySqlClient;

namespace InverIoT
{
    public partial class frmHistorico : Form
    {

        public frmHistorico()
        {
            InitializeComponent();
            CargarDatosSensores();

            // StartPosition was set to FormStartPosition.Manual in the properties window.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            dgvHistorico.DoubleBuffered(true);

            for (int i = 1; i < dgvHistorico.Columns.Count - 1; i++)
            {
                dgvHistorico.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            dgvHistorico.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvHistorico.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

        }
        private void CargarDatosSensores()
        {
            string connectionString = "server=46.24.8.196;port=3307;uid=joseluis;pwd=UBU_tfg_23_24;database=TFG_UBU";
            string query = "SELECT * FROM sensores";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvHistorico.DataSource = dt;

                        // Personaliza los nombres de las columnas
                        dgvHistorico.Columns[0].HeaderText = "Id";
                        dgvHistorico.Columns[1].HeaderText = "Fecha";
                        dgvHistorico.Columns[2].HeaderText = "Hora";
                        dgvHistorico.Columns[3].HeaderText = "Temperatura";
                        dgvHistorico.Columns[4].HeaderText = "Humedad Ambiente";
                        dgvHistorico.Columns[5].HeaderText = "Luminosudad";
                        dgvHistorico.Columns[6].HeaderText = "Humedad Suelo";

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}");
                }
            }
        }
    }
}
