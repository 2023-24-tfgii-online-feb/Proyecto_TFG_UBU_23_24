using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace InverIoT
{
    public partial class frmGraficas : Form
    {
        public frmGraficas()
        {
            InitializeComponent();

            // StartPosition was set to FormStartPosition.Manual in the properties window.
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int w = Width >= screen.Width ? screen.Width : (screen.Width + Width) / 2;
            int h = Height >= screen.Height ? screen.Height : (screen.Height + Height) / 2;
            this.Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
            this.Size = new Size(w, h);

            cargarDatosGrafico();
        }

        private async void cargarDatosGrafico()
        {
            string connectionString = Funciones.conexionMySQL;
            string query = "SELECT fecha, hora, temperatura, humedad, intensidad_luz, humedad_suelo FROM sensores WHERE fecha = @fechaActual";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@fechaActual", DateTime.Now.Date);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            // Aquí llamarías a la función que actualiza tu gráfico con 'dt' como parámetro
                            actualizaDatosGrafica(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}");
                }
            }
        }

        private void actualizaDatosGrafica(DataTable datos)
        {
            chartSensores.Series.Clear();
            chartSensores.ChartAreas.Clear();
            chartSensores.ChartAreas.Add(new ChartArea());

            // Crear una serie para cada variable
            Series serieTemperatura = chartSensores.Series.Add("Temperatura");
            serieTemperatura.ChartType = SeriesChartType.Line;
            serieTemperatura.XValueType = ChartValueType.DateTime;
            serieTemperatura.Color = Color.Blue;
            serieTemperatura.BorderWidth = 3;

            Series serieHumedad = chartSensores.Series.Add("Humedad Ambiente");
            serieHumedad.ChartType = SeriesChartType.Line;
            serieHumedad.XValueType = ChartValueType.DateTime;
            serieHumedad.Color = Color.Red;
            serieHumedad.BorderWidth = 3;

            Series serieLuminosidad = chartSensores.Series.Add("Luminosidad");
            serieLuminosidad.ChartType = SeriesChartType.Line;
            serieLuminosidad.XValueType = ChartValueType.DateTime;
            serieLuminosidad.Color = Color.Green;
            serieLuminosidad.BorderWidth = 3;

            Series serieHumedadSuelo = chartSensores.Series.Add("Humedad Suelo");
            serieHumedadSuelo.ChartType = SeriesChartType.Line;
            serieHumedadSuelo.XValueType = ChartValueType.DateTime;
            serieHumedadSuelo.Color = Color.Orange;
            serieHumedadSuelo.BorderWidth = 3;

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

            // Configuraciones adicionales de la gráfica
            /*chartSensores.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy HH:mm:ss";
            chartSensores.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Hours;
            chartSensores.ChartAreas[0].AxisX.Interval = 1;*/
            chartSensores.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy HH:mm:ss";
            chartSensores.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            chartSensores.ChartAreas[0].AxisX.Interval = 30; // Establecer intervalos de 30 minutos
        }

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
