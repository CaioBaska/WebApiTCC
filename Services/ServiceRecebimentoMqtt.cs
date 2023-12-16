﻿using API_TCC.Repositories;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet;
using System.Text.RegularExpressions;
using System.Text;

namespace API_TCC.Services
{
    public class ServiceRecebimentoMqtt : BackgroundService
    {
        private readonly ILogger<MeuServicoMqtt> _logger;
        private readonly MqttClientOptionsBuilder _mqttOptionsBuilder;
        private readonly ManagedMqttClientOptions _mqttClientOptions;
        private readonly IManagedMqttClient _mqttClient;
        private readonly MqttClientOptionsBuilder _mqttOptionsBuilder2;
        private readonly ManagedMqttClientOptions _mqttClientOptions2;
        private readonly IManagedMqttClient _mqttClient2;
        private readonly IMonitoramentoRepository _repository;

        public ServiceRecebimentoMqtt(ILogger<MeuServicoMqtt> logger, IMonitoramentoRepository repository)
        {
            _logger = logger;
            _repository = repository;

            var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId("smartgreen2")
                .WithTcpServer("152.67.55.77", 1883)
                .WithCredentials("master", "mqtt12345").Build();

            _mqttClientOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(60))
                .WithClientOptions(mqttClientOptions)
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

            Console.WriteLine(valores);
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
                .Where(formatted => formatted.Contains("UMIDADE") || formatted.Contains("TEMPERATURA") || formatted.Contains("POTASSIO") || formatted.Contains("PH") || formatted.Contains("NITROGENIO") || formatted.Contains("FOSFORO"))
                .ToList();

            return "{ " + string.Join(',', valores.ToArray()) + " }";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClient.StartAsync(_mqttClientOptions);

            await _mqttClient.SubscribeAsync(
                new MqttTopicFilter
                {
                    Topic = "smartgreen/topic"
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