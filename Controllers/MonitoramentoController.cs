using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using API_TCC.Repositories;
using API_TCC.Model;
using API_TCC.Services;
using Microsoft.EntityFrameworkCore;
using API_TCC.Database;
using API_TCC.DTO;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitoramentoController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly MonitoramentoService _monitoramentoService;
        private readonly IMonitoramentoRepository _repository;
        private readonly MeuServicoMqtt _meuServicoMqtt;

        public MonitoramentoController(MyDbContext context, IMonitoramentoRepository repository, MonitoramentoService monitoramentoService, MeuServicoMqtt meuServicoMqtt)
        {
            _context = context;
            _repository = repository;
            _monitoramentoService = monitoramentoService;
            _meuServicoMqtt = meuServicoMqtt;
        }

        [HttpGet("obterDados")]
        public IActionResult GetAllDados()
        {
            //var connection = (OracleConnection)_context.Database.GetDbConnection();
            //var service = new MonitoramentoService(connection);

            List<MonitoramentoDTO> dados = _monitoramentoService.GetAllDados();

            return  Ok(dados);
        }

        [HttpGet("obterDadosByData")]
        public IActionResult GetDadosByData(DateTime dataInicial,DateTime dataFinal)
        {
            //var connection = (OracleConnection)_context.Database.GetDbConnection();
            //var service = new MonitoramentoService(connection);

            List<MonitoramentoDTO> dados = _monitoramentoService.GetDadosByData(dataInicial,dataFinal);

            return Ok(dados);
        }

        [HttpGet("mandarTopicoMqtt")]
        public async Task<IActionResult> GetDadosPorTopico(string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
            {
                return BadRequest("O parâmetro 'mensagem' não pode ser nulo ou vazio.");
            }

            // Aqui você pode chamar o serviço MQTT com o tópico fornecido.
            await _meuServicoMqtt.SendMessageToTopicAsync("www.malu.recriart.online", 1883, "master", "mqtt12345", "smartgreen", mensagem);

            return Ok($"Dados enviados para o tópico smartgreen: {mensagem}");
        }


    }
}
