using MQTTnet;
using MQTTnet.Client;
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
        }

        private IMqttClient mqttClient;
        private MqttClientOptions mqttClientOptions;

        private void InitializeMqtt()
        {
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("46.24.8.196", 1883) // Asegúrate de que el puerto es correcto
                .Build();

        }

        private void UpdateLabel(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateLabel), message);
                return;
            }

            lblMqtt.Text = message;
        }


        private async void btnRecibirMqtt_Click(object sender, EventArgs e)
        {
            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();

            // Configura el manejador de mensajes recibidos
            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceived;

            try
            {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                await mqttClient.SubscribeAsync("invernadero/sensores");
                MessageBox.Show("Conectado y suscrito a 'invernadero/sensores'. Esperando mensajes...");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
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
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/t_ON")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                await mqttClient.DisconnectAsync();
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
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/ha_ON")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                await mqttClient.DisconnectAsync();
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
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/l_ON")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                await mqttClient.DisconnectAsync();
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
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/ordenes")
                    .WithPayload("/hs_ON")
                    .Build();

                await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

                // Considera mantener la conexión abierta si planeas enviar/recibir más mensajes
                await mqttClient.DisconnectAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}");
            }
        }

        
    }
}
