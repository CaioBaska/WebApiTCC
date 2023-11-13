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

        public MonitoramentoController(MyDbContext context, IMonitoramentoRepository repository, MonitoramentoService monitoramentoService)
        {
            _context = context;
            _repository = repository;
            _monitoramentoService = monitoramentoService;
        }

        [HttpGet("obterDados")]
        public IActionResult GetAllDados()
        {
            //var connection = (OracleConnection)_context.Database.GetDbConnection();
            //var service = new MonitoramentoService(connection);

            List<MonitoramentoDTO> dados = _monitoramentoService.GetAllDados();

            return  Ok(dados);
        }

    }
}
