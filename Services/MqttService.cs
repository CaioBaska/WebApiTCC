﻿using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Server;
using System.Text;
using System.Text.RegularExpressions;
using API_TCC.Services;
using API_TCC.Repositories;
using Microsoft.Extensions.Hosting;

namespace API_TCC.Services
{
    public class MeuServicoMqtt : BackgroundService
    {
        private readonly ILogger<MeuServicoMqtt> _logger;
        private readonly MqttClientOptionsBuilder _mqttOptionsBuilder;
        private readonly ManagedMqttClientOptions _mqttClientOptions;
        private readonly IManagedMqttClient _mqttClient;
        private readonly MqttClientOptionsBuilder _mqttOptionsBuilder2;
        private readonly ManagedMqttClientOptions _mqttClientOptions2;
        private readonly IManagedMqttClient _mqttClient2;
        private readonly IMonitoramentoRepository _repository;

        public MeuServicoMqtt(ILogger<MeuServicoMqtt> logger, IMonitoramentoRepository repository)
        {
            _logger = logger;
            _repository = repository;

            _mqttOptionsBuilder = new MqttClientOptionsBuilder()
                .WithClientId("agro")
                 .WithTcpServer("au1.cloud.thethings.network", 1883)
                .WithCredentials("projetos-inovfablab@ttn", "NNSXS.EZ66LGGZDIQS5RCQRBCWIWKRCYPL7IOKF7MIPOA.UYQK6SZHUG7CQPIQVUQMDXPKX3KKO5MRM6VVLFADOXXCV2TOSD7Q");

            _mqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                .WithClientOptions(_mqttOptionsBuilder.Build())
                .Build();

            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            ConfigureMqttHandlers();
        }

        private void ConfigureMqttHandlers()
        {
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            _mqttClient.UseApplicationMessageReceivedHandler(HandleReceivedMessage);
        }

        private void HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs args)
        {
            var payloadText = Encoding.UTF8.GetString(args.ApplicationMessage.Payload ?? Array.Empty<byte>());
            var valores = ObterValoresJson(payloadText);

            _repository.CadastrarDados(valores);

            Console.WriteLine(payloadText);
        }

        private void OnConnected(MqttClientConnectedEventArgs obj)
        {
            Console.WriteLine(@"Successfully connected.");
        }

        private void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            Console.WriteLine("Couldn't connect to broker.");
        }

        private void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            Console.WriteLine("Successfully disconnected.");
        }

        private string ObterValoresJson(string json)
        {
            var padrao = "(?:\\\"|\\')(?<key>[\\w\\d]+)(?:\\\"|\\')(?:\\:\\s*)(?:\\\"|\\')?(?<value>[\\w\\s.-]*)(?:\\\"|\\')?";
            var valores = Regex.Matches(json, padrao)
                .Select(match => $"{match.Groups["key"].Value}: {match.Groups["value"].Value}")
                .Where(formatted => formatted.Contains("UMIDADE") || formatted.Contains("TEMPERATURA") || formatted.Contains("POTASSIO") || formatted.Contains("PH") || formatted.Contains("NITROGENIO") || formatted.Contains("FOSFORO") || formatted.Contains("LUMINOSIDADE"))
                .ToList();

            return "{ " + string.Join(',', valores.ToArray()) + " }";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClient.StartAsync(_mqttClientOptions);

            await _mqttClient.SubscribeAsync(
                new MqttTopicFilter
                {
                    Topic = "v3/projetos-inovfablab@ttn/devices/ag-tc/up"
                }
            );

            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(1000, stoppingToken); 
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.StopAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
