using API_TCC.Database;
using API_TCC.DTO;
using API_TCC.Repositories;
using API_TCC.Repository;
using API_TCC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlantasController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly PlantasService _plantasService;
        private readonly IPlantasRepository _repository;

        public PlantasController(MyDbContext context, IPlantasRepository repository, PlantasService plantasService)
        {
            _context = context;
            _repository = repository;
            _plantasService = plantasService;
        }

        [HttpPost("obterDadosPlantas")]

        public IActionResult GetPlantaDados(string nomePlanta)
        {


            List<PlantasDTO> dados = _plantasService.GetDadosPlantas(nomePlanta);
            return Ok(dados);
        }
    }

}