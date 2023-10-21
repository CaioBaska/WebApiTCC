using API_TCC.Database;
using API_TCC.Model;
using API_TCC.Repositories;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;

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
                string query = "SELECT 1 FROM TCC.usuarios WHERE LOGIN = :login AND SENHA = :senha";

                if (_context.State != System.Data.ConnectionState.Open)
                {
                    _context.Open();
                }

                int result = _context.QueryFirstOrDefault<int>(query, new { login, senha });

                return result == 1;
            }
            catch (Exception ex)
            {
                // Tratar exceções aqui (registrar ou lançar uma exceção personalizada, se necessário)
                throw new Exception("Ocorreu um erro durante a validação do login.", ex);
            }
        }
    }
}
