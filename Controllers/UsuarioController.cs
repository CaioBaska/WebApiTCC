using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_TCC.Database;
using API_TCC.Model;
using API_TCC.Services;
using Oracle.ManagedDataAccess.Client;
using API_TCC.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(MyDbContext context, UsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _context = context;
            _usuarioService= usuarioService;
            _usuarioRepository= usuarioRepository;

        }


        // POST: api/usuarios/login
        [HttpPost("login")]
        public IActionResult Login(UsuarioModel usuarioModel)
        {
            if (usuarioModel.Senha == null || usuarioModel.Login == null)
            {
                return BadRequest("Credenciais inválidas");
            }

            bool result = _usuarioService.ValidarLogin(usuarioModel.Login,usuarioModel.Senha);

            return result ? Ok(result) : BadRequest("Credenciais inválidas");

            //var connection = (OracleConnection)_context.Database.GetDbConnection();
            //var service = new UsuarioService(connection);

            //bool? valid = await Task.FromResult(service.ValidarLogin(usuario.Login, usuario.Senha));

            //if (valid.HasValue && valid.Value)
            //{
            //    return Ok(true);
            //    //return true;
            //}
            //else
            //{
            //    return BadRequest(false);
            //    //return false;
            //}
        }

        [HttpPost("criaLogin")]
        public IActionResult CriaLogin([FromBody]UsuarioModel usuarioModel)
        {
            _usuarioService.CriaLogin(usuarioModel.NOME, usuarioModel.Login, usuarioModel.Senha);
            //return Ok();
            return CreatedAtAction(nameof(CriaLogin), null);
        }


    }



}


