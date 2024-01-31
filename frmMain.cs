using MQTTnet;
using MQTTnet.Client;
using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Text;

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

        //private void UpdateTextBoxColor(TextBox textBox, TextBox minTextBox, TextBox maxTextBox)
        private void UpdateTextBoxColor(TextBox textBox, TextBox minTextBox, TextBox maxTextBox, string unit)
        {
            // Remover la unidad del final de cada valor de TextBox
            string valueStr = textBox.Text.EndsWith(unit) ? textBox.Text.Substring(0, textBox.Text.Length - unit.Length) : textBox.Text;
            string minStr = minTextBox.Text.EndsWith(unit) ? minTextBox.Text.Substring(0, minTextBox.Text.Length - unit.Length) : minTextBox.Text;
            string maxStr = maxTextBox.Text.EndsWith(unit) ? maxTextBox.Text.Substring(0, maxTextBox.Text.Length - unit.Length) : maxTextBox.Text;

            if (float.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float value) &&
                float.TryParse(minStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float minValue) &&
                float.TryParse(maxStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float maxValue))
            {
                if (value < minValue)
                {
                    textBox.BackColor = SystemColors.GradientActiveCaption;
                }
                else if (value > maxValue)
                {
                    textBox.BackColor = Color.Coral;
                }
                else
                {
                    textBox.BackColor = SystemColors.ButtonFace;
                }
            }
            else
            {
                textBox.BackColor = SystemColors.ButtonFace; // Valor no válido
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

        private async void btnActualizaUmb_Click(object sender, EventArgs e)
        {
            // Mostrar el MessageBox con las opciones Sí y No
            var result = MessageBox.Show("¿Actualizar los umbrales?", "Actualizar los umbrales", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verificar cuál botón fue presionado y actuar en consecuencia
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection("server=46.24.8.196;port=3307;uid=joseluis;pwd=UBU_tfg_23_24;database=TFG_UBU"))
                    {
                        await connection.OpenAsync();

                        var query = $@"UPDATE umbrales SET 
                                temperatura_minima = @temperaturaMinima, 
                                temperatura_maxima = @temperaturaMaxima, 
                                humedad_ambiente_minima = @humedadAmbMin, 
                                humedad_ambiente_maxima = @humedadAmbMax, 
                                luminosidad_minima = @luminosidadMin, 
                                luminosidad_maxima = @luminosidadMax, 
                                humedad_suelo_minima = @humedadSueloMin, 
                                humedad_suelo_maxima = @humedadSueloMax";

                        using (var cmd = new MySqlCommand(query, connection))
                        {
                            // Asegúrate de que los nombres de los parámetros coincidan con los nombres en tu consulta SQL
                            cmd.Parameters.AddWithValue("@temperaturaMinima", Convert.ToSingle(txtTemperaturaMin.Text));
                            cmd.Parameters.AddWithValue("@temperaturaMaxima", Convert.ToSingle(txtTemperaturaMax.Text));
                            cmd.Parameters.AddWithValue("@humedadAmbMin", Convert.ToSingle(txtHumAmbMin.Text));
                            cmd.Parameters.AddWithValue("@humedadAmbMax", Convert.ToSingle(txtHumAmbMax.Text));
                            cmd.Parameters.AddWithValue("@luminosidadMin", Convert.ToSingle(txtLuminosidadMin.Text));
                            cmd.Parameters.AddWithValue("@luminosidadMax", Convert.ToSingle(txtLuminosidadMax.Text));
                            cmd.Parameters.AddWithValue("@humedadSueloMin", Convert.ToSingle(txtHumSueloMin.Text));
                            cmd.Parameters.AddWithValue("@humedadSueloMax", Convert.ToSingle(txtHumSueloMax.Text));

                            await cmd.ExecuteNonQueryAsync();
                        }

                        MessageBox.Show("Los umbrales han sido actualizados correctamente.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar los umbrales: {ex.Message}");
                }
            }
            else if (result == DialogResult.No)
            {
                // Acción cancelada por el usuario
                MessageBox.Show("No se han actualizado los umbrales.");
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

        private void txtTemperatura_TextChanged(object sender, EventArgs e)
        {
            //UpdateTextBoxColor(txtTemperatura, txtTemperaturaMin, txtTemperaturaMax);
            UpdateTextBoxColor(txtTemperatura, txtTemperaturaMin, txtTemperaturaMax, " °C");
        }

        private void txtHumAmb_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBoxColor(txtHumAmb, txtHumAmbMin, txtHumAmbMax, " %");
            //UpdateTextBoxColor(txtHumAmb, txtHumAmbMin, txtHumAmbMax);
        }

        private void txtLuminosidad_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBoxColor(txtLuminosidad, txtLuminosidadMin, txtLuminosidadMax, " lux");
            //UpdateTextBoxColor(txtLuminosidad, txtLuminosidadMin, txtLuminosidadMax);
        }

        private void txtHumSuelo_TextChanged(object sender, EventArgs e)
        {
            UpdateTextBoxColor(txtHumSuelo, txtHumSueloMin, txtHumSueloMax, " %");
            //UpdateTextBoxColor(txtHumSuelo, txtHumSueloMin, txtHumSueloMax);
        }

        private async void btnTempOff_Click(object sender, EventArgs e)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/t_OFF") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private async void btnHumAmbOff_Click(object sender, EventArgs e)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/ha_OFF") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private async void btnLuminosidadOff_Click(object sender, EventArgs e)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/l_OFF") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        private async void btnHumSueloOff_Click(object sender, EventArgs e)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/hs_OFF") // Manda esta orden y ya la procesa la raspberry
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }
    }
}
