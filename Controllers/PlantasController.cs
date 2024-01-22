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

        [HttpGet("obterDadosPlantas")]

        public IActionResult GetPlantaDados(string nomePlanta)
        {


            List<PlantasDTO> dados = _plantasService.GetDadosPlantas(nomePlanta);
            return Ok(dados);
        }

        [HttpGet("cadastrarPlanta")]
        public IActionResult CreatePlantaDados(string nomePlanta, string temperatura, string umidade,string nitrogenio, string fosforo, string PH, string potassio, string luminosidade)
        {
            if (string.IsNullOrEmpty(nomePlanta) || string.IsNullOrEmpty(temperatura) || string.IsNullOrEmpty(umidade) ||
                string.IsNullOrEmpty(nitrogenio) || string.IsNullOrEmpty(fosforo) || string.IsNullOrEmpty(PH) || string.IsNullOrEmpty(potassio) || string.IsNullOrEmpty(luminosidade))
            {
                return BadRequest("Todos os parâmetros devem ser fornecidos.");
            }

            _plantasService.CreateDadosPlantas(nomePlanta, temperatura, umidade, nitrogenio, fosforo, PH, potassio, luminosidade);

            return Ok();
        }


        [HttpGet("obterTodasPlantas")]

        public IActionResult GetAllPlantas()
        {


            List<string> dados = _plantasService.GetAllPlantas();
            return Ok(dados);
        }

    }

}