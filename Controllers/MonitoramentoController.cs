using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Database;
using API_TCC.Model;
using API_TCC.Services;
using Oracle.ManagedDataAccess.Client;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitoramentoController : ControllerBase
    {
        private readonly MyDbContext _context;

        public MonitoramentoController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("monitoramento")]
        public ActionResult<List<MonitoramentoModel>> GetAllDados()
        {
            var connection = (OracleConnection)_context.Database.GetDbConnection();
            var service = new MonitoramentoService(connection);

            var dados = service.GetAllDados();

            return dados;
        }

    }
}