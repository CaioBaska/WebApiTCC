using API_TCC.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;


namespace API_TCC.Services
{
    public class UsuarioService : IUsuarioRepository
    {
        private readonly OracleConnection _context;

        public UsuarioService(OracleConnection context)
        {
            _context = context;
        }

        public bool ValidarLogin(string login, string senha)
        {
            try
            {
                string query = $@"SELECT 1 FROM TCC.usuarios WHERE LOGIN = '{login}' AND SENHA = '{senha}'";

                if (_context.State != System.Data.ConnectionState.Open)
                {
                    _context.Open();
                }

                bool result = _context.QueryFirstOrDefault<bool>(query, new { login, senha });

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro durante a validação do login.", ex);
            }
        }
    }
}
