using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using API_TCC.Repository;

namespace API_TCC.Services
{
    public class ServiceEnvioMqtt : IServiceEnvioMqtt
    {
        private IMqttClient _mqttClient;
        public ServiceEnvioMqtt()
        {
            Connect_Client().Wait();
        }

        public async Task Connect_Client()
        {
            var mqttFactory = new MqttFactory();

            _mqttClient = mqttFactory.CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId("smartgreen")
                .WithTcpServer("152.67.55.77", 1883)
                .WithCredentials("master", "mqtt12345").Build();

            var response = await _mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            // Use builder classes where possible in this project.


            // This will throw an exception if the server is not available.
            // The result from this message returns additional data which was sent 
            // from the server. Please refer to the MQTT protocol specification for details.

        }

        public async void PublicarMensagem(string mensagem)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("EnviaSmart")
                .WithPayload(mensagem)
                .Build();

            await _mqttClient.PublishAsync(applicationMessage);
        }

        public async void PublicarRelatorio(string mensagem)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic("EnviaRelatorio")
                .WithPayload(mensagem)
                .Build();

            await _mqttClient.PublishAsync(applicationMessage);
        }
    }
}
