using MQTTnet; // Incluye la biblioteca MQTTnet para trabajar con MQTT.
using MQTTnet.Client; // Incluye funcionalidades de cliente MQTT.
using MySql.Data.MySqlClient; // Incluye funcionalidades para trabajar con MySQL.
using System.Globalization; // Utilizado para formatear números según una cultura específica.
using System.Text; // Proporciona codificaciones de caracteres para convertir entre arreglos de bytes y cadenas de texto.

namespace InverIoT
{
    public partial class frmMain : Form
    {
        // Variables para controlar si las ventanas de histórico y gráficas están abiertas.
        private frmHistorico historicoForm = null;
        private frmGraficas graficasForm = null;

        // Constructor del formulario principal.
        public frmMain()
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz de usuario.
            inicializaMqtt(); // Inicializa la configuración y conexión MQTT.
            cargarUmbrales(); // Carga los umbrales desde la base de datos.

            // Asigna el mismo manejador de eventos para validar la entrada de los TextBox.
            txtTemperaturaMin.KeyPress += formato_txtbox_KeyPress;
            txtTemperaturaMax.KeyPress += formato_txtbox_KeyPress;
            txtHumAmbMin.KeyPress += formato_txtbox_KeyPress;
            txtHumAmbMax.KeyPress += formato_txtbox_KeyPress;
            txtLuminosidadMin.KeyPress += formato_txtbox_KeyPress;
            txtLuminosidadMax.KeyPress += formato_txtbox_KeyPress;
            txtHumSueloMin.KeyPress += formato_txtbox_KeyPress;
            txtHumSueloMax.KeyPress += formato_txtbox_KeyPress;
        }

        private IMqttClient mqttClient; // Cliente MQTT para interactuar con el broker MQTT.
        private MqttClientOptions mqttClientOptions; // Opciones de configuración para el cliente MQTT.

        // Método para cargar los umbrales de la base de datos y mostrarlos en el formulario.
        private void cargarUmbrales()
        {
            string connectionString = Funciones.conexionMySQL; // Cadena de conexión a la base de datos.

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open(); // Abre la conexión a la base de datos.
                    string query = "SELECT * FROM umbrales ORDER BY fecha_actualizacion DESC LIMIT 1"; // Consulta SQL para obtener el último registro de umbrales.

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Si hay un registro disponible, asigna los valores a los controles del formulario.
                            {
                                // Asignación de los valores leídos a los TextBox correspondientes.
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
                                MessageBox.Show("No se encontraron registros."); // Informa al usuario si no hay registros de umbrales.
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar a la base de datos: {ex.Message}"); // Muestra un mensaje de error si falla la conexión.
                }
            }
        }

        // Método para validar la entrada de los TextBox, permitiendo solo números y un punto decimal.
        private void formato_txtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true; // No acepta el carácter si no es un número, control o punto.
            }

            TextBox textBox = sender as TextBox;
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
            {
                e.Handled = true; // No acepta un segundo punto decimal.
            }
        }

        // Método para enviar comandos a través de MQTT.
        private async Task mandarOrdenMqtt(string payload)
        {
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes") // Establece el tópico al que se publicará el mensaje.
                    .WithPayload(payload) // Establece el payload del mensaje usando el parámetro.
                    .Build(); // Construye el mensaje MQTT.

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None); // Publica el mensaje en el broker MQTT.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje MQTT: {ex.Message}"); // Muestra un mensaje de error si falla la publicación.
            }
        }

        // Método para actualizar el color de fondo de los TextBox según los valores de los umbrales.
        private void cambiaColotTextBox(TextBox textBox, TextBox minTextBox, TextBox maxTextBox, string unit)
        {
            string valueStr = textBox.Text.EndsWith(unit) ? textBox.Text.Substring(0, textBox.Text.Length - unit.Length) : textBox.Text;
            string minStr = minTextBox.Text.EndsWith(unit) ? minTextBox.Text.Substring(0, minTextBox.Text.Length - unit.Length) : minTextBox.Text;
            string maxStr = maxTextBox.Text.EndsWith(unit) ? maxTextBox.Text.Substring(0, maxTextBox.Text.Length - unit.Length) : maxTextBox.Text;

            if (float.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float value) &&
                float.TryParse(minStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float minValue) &&
                float.TryParse(maxStr, NumberStyles.Any, CultureInfo.InvariantCulture, out float maxValue))
            {
                if (value < minValue)
                {
                    textBox.BackColor = SystemColors.GradientActiveCaption; // Fondo azul si el valor está por debajo del mínimo.
                }
                else if (value > maxValue)
                {
                    textBox.BackColor = Color.Coral; // Fondo coral si el valor está por encima del máximo.
                }
                else
                {
                    textBox.BackColor = SystemColors.ButtonFace; // Fondo por defecto si el valor está dentro de los umbrales.
                }
            }
            else
            {
                textBox.BackColor = SystemColors.ButtonFace; // Fondo por defecto si el valor no es válido.
            }
        }

        // Inicializa la configuración y conexión MQTT.
        private async void inicializaMqtt()
        {
            var mqttFactory = new MqttFactory(); // Crea una instancia de la fábrica MQTT.
            mqttClient = mqttFactory.CreateMqttClient(); // Crea el cliente MQTT.

            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("46.24.8.196", 1883) // Configura el servidor y puerto del broker MQTT.
                .Build(); // Construye las opciones de configuración del cliente MQTT.

            mqttClient.ApplicationMessageReceivedAsync += recibeMqtt; // Suscribe al evento de mensaje recibido.

            try
            {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None); // Intenta conectar con el broker MQTT.
                await mqttClient.SubscribeAsync("invernadero/sensores"); // Se suscribe al tópico para recibir mensajes de los sensores.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}"); // Muestra un mensaje si falla la conexión.
            }
        }

        // Actualiza los controles del formulario con los datos recibidos de MQTT.
        private void actualizalblUmbrales(string message)
        {
            if (InvokeRequired) // Verifica si la llamada debe hacerse en el hilo de la interfaz de usuario.
            {
                Invoke(new Action<string>(actualizalblUmbrales), message); // Realiza la llamada en el hilo correcto.
                return;
            }

            // Divide el mensaje recibido y actualiza los TextBox correspondientes.
            var parts = message.Split(',');
            if (parts.Length >= 6) // Verifica que el mensaje tenga todas las partes necesarias.
            {
                txtTemperatura.Text = parts[2] + " °C";
                txtHumAmb.Text = parts[3] + " %";
                txtLuminosidad.Text = parts[4] + " lux";
                txtHumSuelo.Text = parts[5] + " %";
            }
        }

        // Maneja los mensajes MQTT recibidos.
        private Task recibeMqtt(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload); // Decodifica el mensaje recibido.
            actualizalblUmbrales(message); // Actualiza los controles del formulario con el mensaje.
            return Task.CompletedTask; // Completa la tarea.
        }

        // Los siguientes métodos responden a los eventos de clic en los botones, enviando órdenes MQTT correspondientes.
        private async void btnTempOff_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/t_OFF"); // Envía la orden para apagar el mecanismo de temperatura.
        }

        private async void btnTemp_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/t_ON"); // Envía la orden para encender el mecanismo de temperatura.
        }

        private async void btnHumAmb_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/ha_ON"); // Envía la orden para encender el mecanismo de humedad ambiente.
        }

        private async void btnLuminosidad_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/l_ON"); // Envía la orden para encender el mecanismo de luminosidad.
        }

        private async void btnHumSuelo_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/hs_ON"); // Envía la orden para encender el mecanismo de humedad del suelo.
        }

        private async void btnHumAmbOff_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/ha_OFF"); // Envía la orden para apagar el mecanismo de humedad ambiente.
        }

        private async void btnLuminosidadOff_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/l_OFF"); // Envía la orden para apagar el mecanismo de luminosidad.
        }

        private async void btnHumSueloOff_Click(object sender, EventArgs e)
        {
            await mandarOrdenMqtt("/hs_OFF"); // Envía la orden para apagar el mecanismo de humedad del suelo.
        }

        // Métodos para abrir los formularios de histórico y gráficas, asegurándose de que solo exista una instancia abierta.
        private void btnHistorico_Click(object sender, EventArgs e)
        {
            if (historicoForm != null && !historicoForm.IsDisposed)
            {
                historicoForm.BringToFront(); // Si el formulario de histórico está abierto, lo trae al frente.
            }
            else
            {
                historicoForm = new frmHistorico(); // Crea una nueva instancia del formulario de histórico y lo muestra.
                historicoForm.Show();
            }
        }

        private void btnGraficas_Click(object sender, EventArgs e)
        {
            if (graficasForm != null && !graficasForm.IsDisposed)
            {
                graficasForm.BringToFront(); // Si el formulario de gráficas está abierto, lo trae al frente.
            }
            else
            {
                graficasForm = new frmGraficas(); // Crea una nueva instancia del formulario de gráficas y lo muestra.
                graficasForm.Show();
            }
        }
        // Actualizo los umbrales insertando los valores en la db, la Raspberry también los actualiza con los valores de su config
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
        // Muestra información sobre el autor y el proyecto al hacer clic en el botón correspondiente.
        private void btnAutor_Click(object sender, EventArgs e)
        {
            string mensaje = "* Inver IoT * v2.2. Aplicación creada por:\n" +
                "José Luis Caballero Martínez-Quintanilla.\n" +
                "© 2023 - 2024 - TFG - UBU.\n\n" +
                "Contacto: jlcaballeromq@gmail.com\n" +
                "Aplicación para monitorizar invernadero de cultivo de cannabis medicinal.\n" +
                "Desarrollada y diseñada como parte de Trabajo Fin de Grado - Ingeniería Informática.";
            string titulo = "Información del Autor";
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information); // Muestra los detalles en un MessageBox.
        }

        // Métodos que comprueban al cambiar el valor de los TextBoxs si deben o no cambiar de color.
        private void txtTemperatura_TextChanged(object sender, EventArgs e)
        {
            cambiaColotTextBox(txtTemperatura, txtTemperaturaMin, txtTemperaturaMax, " °C");
        }

        private void txtHumAmb_TextChanged(object sender, EventArgs e)
        {
            cambiaColotTextBox(txtHumAmb, txtHumAmbMin, txtHumAmbMax, " %");
        }

        private void txtLuminosidad_TextChanged(object sender, EventArgs e)
        {
            cambiaColotTextBox(txtLuminosidad, txtLuminosidadMin, txtLuminosidadMax, " lux");
        }

        private void txtHumSuelo_TextChanged(object sender, EventArgs e)
        {
            cambiaColotTextBox(txtHumSuelo, txtHumSueloMin, txtHumSueloMax, " %");
        }
    }
}
