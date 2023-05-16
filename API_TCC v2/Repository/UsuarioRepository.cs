using API_TCC.Model;

namespace API_TCC.Repositories
{
    public interface IUsuarioRepository
    {
        bool ValidarLogin(string login, string senha);
    }
}
