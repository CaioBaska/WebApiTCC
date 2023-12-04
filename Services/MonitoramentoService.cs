using API_TCC.Repositories;
using API_TCC.Model;
using API_TCC.Database;
using Microsoft.EntityFrameworkCore;
using API_TCC.DTO;
using Newtonsoft.Json.Linq;

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
                string query = "SELECT * FROM (SELECT id, DATA, UMIDADE, TEMPERATURA, PH, NITROGENIO, FOSFORO, POTASSIO, LUMINOSIDADE FROM TCC.MONITORAMENTO ORDER BY DATA DESC ) WHERE ROWNUM = 1";

                List<MonitoramentoDTO> result = _context.MonitoramentoModel
                    .FromSqlRaw(query)
                    .Select(m => new MonitoramentoDTO
                    {
                        UMIDADE = m.UMIDADE,
                        TEMPERATURA = m.TEMPERATURA,
                        PH = m.PH,
                        NITROGENIO = m.NITROGENIO,
                        FOSFORO = m.FOSFORO,
                        POTASSIO = m.POTASSIO,
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
            try
            {
                var valores = JObject.Parse(json);

                
                var cultura = new System.Globalization.CultureInfo("pt-BR");

                
                var novoMonitoramento = new MonitoramentoModel
                {
                    DATA = DateTime.Now, 
                    UMIDADE = valores.ContainsKey("UMIDADE") ? valores["UMIDADE"].ToString() : "0",
                    TEMPERATURA = valores.ContainsKey("TEMPERATURA") ? valores["TEMPERATURA"].ToString() : "0",
                    POTASSIO = valores.ContainsKey("POTASSIO") ? valores["POTASSIO"].ToString() : "0",
                    PH = valores.ContainsKey("PH") ? valores["PH"].ToString() : "0",
                    NITROGENIO = valores.ContainsKey("NITROGENIO") ? valores["NITROGENIO"].ToString() : "0",
                    FOSFORO = valores.ContainsKey("FOSFORO") ? valores["FOSFORO"].ToString() : "0",
                };

                
                _context.MonitoramentoModel.Add(novoMonitoramento);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar dados: {ex.Message}");
            }
        }


    }
}
