using API_TCC.DTO;

namespace API_TCC.Repositories
{
    public interface IMonitoramentoRepository
    {
        public List<MonitoramentoDTO> GetAllDados();
        public void CadastrarDados(string json);
    }
}
