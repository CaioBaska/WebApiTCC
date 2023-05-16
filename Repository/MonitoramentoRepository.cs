using API_TCC.Model;

namespace API_TCC.Repositories
{
    public interface IMonitoramentoRepository
    {
        public List<MonitoramentoModel> GetAllDados();
    }
}
