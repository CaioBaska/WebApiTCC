using API_TCC.Repositories;
using API_TCC.Model;
using API_TCC.Database;
using Microsoft.EntityFrameworkCore;
using API_TCC.DTO;

namespace API_TCC.Services
{
    public class MonitoramentoService : IMonitoramentoRepository
    {
        private readonly MyDbContext _context;

        public MonitoramentoService(MyDbContext context)
        {
            _context = context;
        }

        public List<MonitoramentoDTO> GetAllDados()
        {
            try
            {
                string query = "SELECT TEMPERATURA, PH, UMIDADE, LUMINOSIDADE FROM TCC.MONITORAMENTO WHERE ROWNUM=1 ORDER BY 1 DESC";

                List<MonitoramentoDTO> result = _context.MonitoramentoModel
                    .FromSqlRaw(query)
                    .Select(m => new MonitoramentoDTO
                    {
                        TEMPERATURA = m.TEMPERATURA,
                        PH = m.PH,
                        UMIDADE = m.UMIDADE,
                        LUMINOSIDADE = m.LUMINOSIDADE
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na consulta: {ex.Message}");
                return new List<MonitoramentoDTO>();
            }
        }

        public void CadastrarDados(string json)
        {
            return;
        }
    }
}
