using API_TCC.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Newtonsoft.Json;
using API_TCC.Model;

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
            string query = "SELECT * FROM C##TCC.MONITORAMENTO WHERE ROWNUM=1 ORDER BY 1 DESC";

            if (_context.State != System.Data.ConnectionState.Open)
            {
                _context.Open();
            }

            using (OracleCommand command = new OracleCommand(query, _context))
            {
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    List<MonitoramentoModel> result = new List<MonitoramentoModel>();
                    while (reader.Read())
                    {
                        int.TryParse(reader.GetString(1), out int tempValue);
                        int.TryParse(reader.GetString(2), out int phValue);
                        int.TryParse(reader.GetString(3), out int umidadeValue);
                        int.TryParse(reader.GetString(4), out int luminValue);

                        var data = new MonitoramentoModel
                        {
                            Temperatura = tempValue,
                            Ph = phValue,
                            Umidade = umidadeValue,
                            Luminosidade = luminValue
                        };
                        result.Add(data);
                    }
                    return result;
                }
            }
        }
    }
}
