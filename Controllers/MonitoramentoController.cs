using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using API_TCC.Repositories;
using API_TCC.Model;
using API_TCC.Services;
using Microsoft.EntityFrameworkCore;
using API_TCC.Database;
using API_TCC.DTO;
using System.Globalization;
using API_TCC.Repository;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitoramentoController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly MonitoramentoService _monitoramentoService;
        private readonly IMonitoramentoRepository _repository;
        //private readonly ServiceEnvioMqtt _meuServicoEnviaMqtt;

        public MonitoramentoController(MyDbContext context, IMonitoramentoRepository repository, MonitoramentoService monitoramentoService/*, ServiceEnvioMqtt meuServicoEnviaMqtt*/)
        {
            _context = context;
            _repository = repository;
            _monitoramentoService = monitoramentoService;
            //_meuServicoEnviaMqtt = meuServicoEnviaMqtt;
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
        public IActionResult GetDadosByData(string dataInicial, string dataFinal)
        {
            DateTime dataInicialFormatada, dataFinalFormatada;

            if (DateTime.TryParseExact(dataInicial, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicialFormatada) &&
                DateTime.TryParseExact(dataFinal, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFinalFormatada))
            {
                // Agora você pode usar dataInicialFormatada e dataFinalFormatada em sua lógica.

                List<RelatorioDTO> dados = _monitoramentoService.GetDadosByData(dataInicialFormatada, dataFinalFormatada);


                return Ok(dados);
            }
            else
            {
                return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy HH:mm:ss");
            }
        }



        //[HttpGet("mandarTopicoMqtt")]
        //public async Task<IActionResult> SendDadosPorTopico(string mensagem)
        //{
        //    if (string.IsNullOrEmpty(mensagem))
        //    {
        //        return BadRequest("O parâmetro 'mensagem' não pode ser nulo ou vazio.");
        //    }

        //    // Aqui você pode chamar o serviço MQTT com o tópico fornecido.
        //    _meuServicoEnviaMqtt.PublicarMensagem(mensagem);

        //    return Ok($"Dados enviados para o tópico smartgreen: {mensagem}");
        //}

        [HttpGet("enviarRelatorioEmail")]
        public IActionResult SendEmailByData(string dataInicial, string dataFinal,string destinatario)
        {
            DateTime dataInicialFormatada, dataFinalFormatada;

            if (DateTime.TryParseExact(dataInicial, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataInicialFormatada) &&
                DateTime.TryParseExact(dataFinal, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFinalFormatada))
            {
                // Agora você pode usar dataInicialFormatada e dataFinalFormatada em sua lógica.

                List<RelatorioDTO> dados = _monitoramentoService.GetDadosByData(dataInicialFormatada, dataFinalFormatada);


                var csvContent=_monitoramentoService.GerarConteudoCSV(dados);

               _monitoramentoService.EnviarEmail(destinatario, csvContent);

                return Ok("eita");
            }
            else
            {
                return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy HH:mm:ss");
            }
        }

    }
}
