using API_TCC.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Newtonsoft.Json;
using API_TCC.Model;
using Dapper;

namespace API_TCC.Services
{
    public class MonitoramentoService : IMonitoramentoRepository
    {
        private readonly OracleConnection _context;

        public MonitoramentoService(OracleConnection context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAllDados()
        {
            string query = "SELECT * FROM TCC.MONITORAMENTO WHERE ROWNUM=1 ORDER BY 1 DESC";

            if (_context.State != System.Data.ConnectionState.Open)
            {
                _context.Open();
            }

            List<MonitoramentoModel> result = _context.Query<MonitoramentoModel>(query).ToList();

            return result;
        }

    }
}
