
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Database;
using API_TCC.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_TCC.Services;
using Oracle.ManagedDataAccess.Client;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UsuariosController(MyDbContext context)
        {
            _context = context;
        }


        // POST: api/usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login(UsuarioModel usuario)
        {
            if (usuario == null || usuario.Login == null || usuario.Senha == null)
            {
                return BadRequest("Credenciais inválidas");
            }

            var connection = (OracleConnection)_context.Database.GetDbConnection();
            var service = new UsuarioService(connection);

            bool? valid = await Task.FromResult(service.ValidarLogin(usuario.Login, usuario.Senha));

            if (valid.HasValue && valid.Value)
            {
                // Login válido
                return Ok(true);
            }
            else
            {
                // Login inválido
                return BadRequest("Credenciais inválidas");
            }
        }



    }



}


