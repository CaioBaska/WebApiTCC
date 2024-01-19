using API_TCC.DTO;
using CsvHelper;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace API_TCC.Repositories
{
    public interface IMonitoramentoRepository
    {
        public List<MonitoramentoDTO> GetAllDados();
        public void CadastrarDados(string json);

        public List<RelatorioDTO> GetDadosByData(DateTime dataInicial,DateTime dataFinal);

        public string GerarConteudoCSV(List<RelatorioDTO> dadosRelatorio);

        public void EnviarEmail(string destinatario, string anexoCsv);



    }
}
