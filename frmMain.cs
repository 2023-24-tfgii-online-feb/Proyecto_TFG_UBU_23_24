using MQTTnet;
using MQTTnet.Client;
using MySql.Data.MySqlClient;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InverIoT
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
            InitializeMqtt();
            cargarUmbrales();
        }

        private IMqttClient mqttClient;
        private MqttClientOptions mqttClientOptions;

        private void cargarUmbrales()
        {
            string connectionString = "server=46.24.8.196;port=3307;uid=joseluis;pwd=UBU_tfg_23_24;database=TFG_UBU";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM umbrales ORDER BY fecha_actualizacion DESC LIMIT 1";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtTemperaturaMin.Text = reader["temperatura_minima"].ToString();
                                txtTemperaturaMax.Text = reader["temperatura_maxima"].ToString();
                                txtHumAmbMin.Text = reader["humedad_ambiente_minima"].ToString();
                                txtHumAmbMax.Text = reader["humedad_ambiente_maxima"].ToString();
                                txtLuminosidadMin.Text = reader["luminosidad_minima"].ToString();
                                txtLuminosidadMax.Text = reader["luminosidad_maxima"].ToString();
                                txtHumSueloMin.Text = reader["humedad_suelo_minima"].ToString();
                                txtHumSueloMax.Text = reader["humedad_suelo_maxima"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron registros.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar a la base de datos: {ex.Message}");
                }
            }
        }


        private async void InitializeMqtt()
        {
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("46.24.8.196", 1883) // Asegúrate de que el puerto es correcto
                .Build();


            // Configura el manejador de mensajes recibidos
            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceived;

            try
            {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                await mqttClient.SubscribeAsync("invernadero/sensores");
                //MessageBox.Show("Conectado y suscrito a 'invernadero/sensores'. Esperando mensajes...");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }

        }

        private void UpdateLabel(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateLabel), message);
                return;
            }

            // Dividir el mensaje en partes
            var parts = message.Split(',');
            if (parts.Length >= 6) // Asegúrate de que el mensaje tenga suficientes partes
            {
                // Asignar cada parte a su TextBox correspondiente, agregando las unidades adecuadas
                txtTemperatura.Text = parts[2] + " °C";     // Temperatura
                txtHumAmb.Text = parts[3] + " %";           // Humedad Ambiente
                txtLuminosidad.Text = parts[4] + " lux";    // Luminosidad
                txtHumSuelo.Text = parts[5] + " %";         // Humedad del Suelo
            }
        }

        private Task MqttClient_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            UpdateLabel(message);
            return Task.CompletedTask;
        }

        private async void btnTemp_Click(object sender, EventArgs e)
        {
            try
            {
                //await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/t_ON") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                //await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private async void btnHumAmb_Click(object sender, EventArgs e)
        {
            try
            {
                //await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/ha_ON") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                //await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        // ACTIVA MECANISMO, en este caso, enciende led verde luminosidad
        private async void btnLuminosidad_Click(object sender, EventArgs e)
        {
            try
            {
                //await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/l_ON") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                //await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private async void btnHumSuelo_Click(object sender, EventArgs e)
        {
            try
            {
                //await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/hs_ON")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                //await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private void btnActualizaUmb_Click(object sender, EventArgs e)
        {
            // Mostrar el MessageBox con las opciones Sí y No
            var result = MessageBox.Show("¿Actualizar los umbrales?", "Actualizar los umbrales", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar cuál botón fue presionado y actuar en consecuencia
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Sí");
            }
            else if (result == DialogResult.No)
            {
                MessageBox.Show("No");
            }
        }

        private void txtTemperaturaMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtTemperaturaMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtHumAmbMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtHumAmbMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtLuminosidadMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtLuminosidadMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtHumSueloMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }

        private void txtHumSueloMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir números, control de backspace, y un punto decimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No aceptar el carácter
            }

            // Solo permitir un punto decimal
            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No aceptar un segundo punto decimal
            }
        }
    }
}
