using API_TCC.Repositories;
using Oracle.ManagedDataAccess.Client;
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
            try
            {
                string query = "SELECT * FROM TCC.MONITORAMENTO WHERE ROWNUM=1 ORDER BY 1 DESC";

                if (_context.State != System.Data.ConnectionState.Open)
                {
                    _context.Open();
                }

                List<MonitoramentoModel> result = _context.Query<MonitoramentoModel>(query).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro durante a consulta do valores.", ex);
            }
        }

    }
}
