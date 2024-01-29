using MQTTnet;
using MQTTnet.Client;
using System;
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

        private async void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("invernadero/sensores")
                    .WithPayload("19.5")
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
    }
}
