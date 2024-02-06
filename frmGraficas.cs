using MySql.Data.MySqlClient; // Proporciona las clases necesarias para la interacción con MySQL.
using System.Data; // Utilizado para trabajar con datos en formas tabulares como DataTable.
using System.Windows.Forms.DataVisualization.Charting; // Proporciona clases para crear gráficos.

namespace InverIoT
{
    public partial class frmGraficas : Form
    {
        public frmGraficas()
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario generados por el diseñador.

            // Ajusta el tamaño y la posición del formulario basado en el área de trabajo de la pantalla principal.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            cargarDatosGrafico(); // Llama al método para cargar datos del gráfico al inicializar el formulario.
        }

        private async void cargarDatosGrafico()
        {
            // Define la cadena de conexión y la consulta SQL.
            string connectionString = Funciones.conexionMySQL;
            string query = "SELECT fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo FROM sensores WHERE fecha = @fechaActual";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Abre la conexión a la base de datos.
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Asigna el valor del parámetro para la consulta.
                        cmd.Parameters.AddWithValue("@fechaActual", DateTime.Now.Date);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt); // Llena el DataTable con los resultados de la consulta.
                            actualizaDatosGrafica(dt); // Actualiza el gráfico con los datos obtenidos.
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}"); // Muestra un mensaje de error si la operación falla.
                }
            }
        }

        private void actualizaDatosGrafica(DataTable datos)
        {
            // Limpia las series y áreas de gráficos existentes.
            chartSensores.Series.Clear();
            chartSensores.ChartAreas.Clear();
            chartSensores.ChartAreas.Add(new ChartArea());

            // Configura y añade la serie de temperatura al gráfico.
            Series serieTemperatura = chartSensores.Series.Add("Temperatura");
            serieTemperatura.ChartType = SeriesChartType.Line; // Tipo de gráfico de línea.
            serieTemperatura.XValueType = ChartValueType.DateTime; // Tipo de valor en el eje X.
            serieTemperatura.Color = Color.Blue; // Color de la línea.
            serieTemperatura.BorderWidth = 3; // Grosor de la línea.

            // Configura y añade la serie de humedad ambiente al gráfico.
            Series serieHumedad = chartSensores.Series.Add("Humedad Ambiente");
            serieHumedad.ChartType = SeriesChartType.Line;
            serieHumedad.XValueType = ChartValueType.DateTime;
            serieHumedad.Color = Color.Red;
            serieHumedad.BorderWidth = 3;

            // Configura y añade la serie de luminosidad al gráfico.
            Series serieLuminosidad = chartSensores.Series.Add("Luminosidad");
            serieLuminosidad.ChartType = SeriesChartType.Line;
            serieLuminosidad.XValueType = ChartValueType.DateTime;
            serieLuminosidad.Color = Color.Green;
            serieLuminosidad.BorderWidth = 3;

            // Configura y añade la serie de humedad del suelo al gráfico.
            Series serieHumedadSuelo = chartSensores.Series.Add("Humedad Suelo");
            serieHumedadSuelo.ChartType = SeriesChartType.Line;
            serieHumedadSuelo.XValueType = ChartValueType.DateTime;
            serieHumedadSuelo.Color = Color.Orange;
            serieHumedadSuelo.BorderWidth = 3;

            // Rellena las series con los datos del DataTable.
            // Rellenar las series con datos
            foreach (DataRow row in datos.Rows)
            {
                string fechaStr = row["fecha"].ToString(); // Solo la fecha
                string horaStr = row["hora"].ToString(); // Solo la hora
                string fechaHoraStr = fechaStr.Split(' ')[0] + " " + horaStr; // Combinar fecha y hora

                if (DateTime.TryParse(fechaHoraStr, out DateTime fechaHora))
                {
                    double temperatura = Convert.ToDouble(row["temperatura"]);
                    double humedad = Convert.ToDouble(row["humedad"]);
                    double luminosidad = Convert.ToDouble(row["intensidad_luz"]);
                    double humedadSuelo = Convert.ToDouble(row["humedad_suelo"]);

                    serieTemperatura.Points.AddXY(fechaHora, temperatura);
                    serieHumedad.Points.AddXY(fechaHora, humedad);
                    serieLuminosidad.Points.AddXY(fechaHora, luminosidad);
                    serieHumedadSuelo.Points.AddXY(fechaHora, humedadSuelo);
                }
                else
                {
                    // Manejar el caso en que la fecha y hora no sean válidas
                    MessageBox.Show("Formato de fecha y hora no válido: " + fechaHoraStr);
                }
            }
            chartSensores.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy\nHH:mm:ss";
            chartSensores.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            chartSensores.ChartAreas[0].AxisX.Interval = 30; // Establecer intervalos de 30 minutos

            // Configura el formato de los labels en el eje X y establece intervalos.
            chartSensores.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy\nHH:mm:ss";
            chartSensores.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            chartSensores.ChartAreas[0].AxisX.Interval = 30; // Establecer intervalos de 30 minutos
        }

        // Implementación similar a cargarDatosGrafico, pero con un rango de fechas definido por el usuario.
        // Utiliza los valores de los DateTimePickers dtpInicio y dtpFin para filtrar los datos.
        // Después de definir y ejecutar la consulta con los nuevos parámetros, llama a actualizaDatosGrafica con los resultados.
        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime fechaInicio = dtpInicio.Value.Date;
            DateTime fechaFin = dtpFin.Value.Date;

            // Validación: la fecha de inicio no debe ser posterior a la fecha de fin
            if (fechaInicio > fechaFin)
            {
                MessageBox.Show("La fecha de inicio no puede ser posterior a la fecha de fin.", "Error en la Selección de Fechas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = Funciones.conexionMySQL;
            string query = "SELECT fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo FROM sensores WHERE fecha >= @fechaInicio AND fecha <= @fechaFin";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            actualizaDatosGrafica(dt); // Método para actualizar la gráfica
                        }
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
