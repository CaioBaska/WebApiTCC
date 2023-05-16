using API_TCC.Database;
using API_TCC.Model;
using API_TCC.Repositories;
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
            string query = "SELECT * FROM C##TCC.usuarios WHERE LOGIN = :login AND SENHA = :senha";

            if (_context.State != System.Data.ConnectionState.Open)
            {
                _context.Open();
            }

            using (OracleCommand command = new (query, _context))
            {
                command.Parameters.Add(new OracleParameter(":login", login));
                command.Parameters.Add(new OracleParameter(":senha", senha));

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }


            }
        }

     }
}