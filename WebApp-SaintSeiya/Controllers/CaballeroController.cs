using Microsoft.AspNetCore.Mvc;
using WebApp_SaintSeiya.CasosDeUso;
using WebApp_SaintSeiya.Dtos;
using WebApp_SaintSeiya.Repositories;

namespace WebApp_SaintSeiya.Controllers
{
    [ApiController]    // Atributo -> Métodos que se pueden enlazar con métodos u otras clases
    [Route("api/[controller]")]  //Atributo para la URL
    public class CaballeroController : Controller
    {

        // Especifico la Base de Datos y hago inyección de dependencia
        private readonly CaballerosDatabaseContext _caballerosDatabaseContext;
        
        // Inyecto la dependencia de la interface en el controller 
        private readonly IUpdateCaballeroUseCase _updateCaballeroUseCase;
        
        //constructor del controller
        // Inyecto el Context en el Repositorio
        public CaballeroController(
                                CaballerosDatabaseContext caballerosDatabaseContext,
                                IUpdateCaballeroUseCase updateCaballeroUseCase
                                )
        {
            _caballerosDatabaseContext = caballerosDatabaseContext;
            _updateCaballeroUseCase = updateCaballeroUseCase;
        }
        
        // input: nada
        // output: Lista con todos los CaballeroDto

        //api/caballero/
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CaballeroDto>))]
        public async Task<IActionResult> GetCaballero()
        {
            //toma cada caballero, lo convierte a caballeroDto y lo lista
            var result = _caballerosDatabaseContext
                                .Caballeros
                                .Select(knight => knight.ToDto())
                                .ToList();

            return new OkObjectResult(result);

        }

        // input: id del caballero
        // output: CaballeroDto

        // Como input le paso IActionResult para que en el return devuelva un status. En este caso, si OK => Status: 200, si no la encontró Status:404

        //api/caballero/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CaballeroDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCaballero(int id)
        {
            CaballeroEntity? result = await _caballerosDatabaseContext.Get(id);

            // Luego, con mapper convierto result a Dto

            //Cuando no tengo el id, devolver un NotFoud -> 404
            
            // Si result es null, significa que no lo encontré. Entonces mando NotFoundResult
            if (result == null)
                return new NotFoundResult();

            return new OkObjectResult(result.ToDto());
        }

        // input: id del caballero
        // output: bool  -> true si se eliminó, false si no lo hizo

        //api/caballero/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCaballero(int id)
        {
            var result = await _caballerosDatabaseContext.Delete(id);

            return new OkObjectResult(result);

        }

        // CaballeroDto incluye el id.
        // CreateCaballero no incluye el id.
        // input: CreateCaballeroDto
        // output: CaballeroDto

        // Como input le paso IActionResult para que en el return devuelva un status. En este caso, si Created => Status: 201

        //api/caballero/agregar
        [HttpPost("agregar")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CaballeroDto))]
        public async Task<IActionResult> PostCaballero(CreateCaballeroDto caballero)
        {
            CaballeroEntity result = await _caballerosDatabaseContext.Add(caballero);

            return new CreatedResult($"http://localhost:7097/caballero/{result.Id}", null);
        }

        // CaballeroDto incluye el id.
        // input: CaballeroDto con id
        // output: CaballeroDto con id

        // Como input le paso IActionResult para que en el return devuelva un status. En este caso, si OK => Status: 200

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCaballero(CaballeroDto caballero)
        {
            //Delego toda la responsabilidad/lógica de negocio dentro del caso de uso
            // Lo cual permite testear de forma más sencilla
            //El "?" implica que puede ser null
            CaballeroDto? result = await _updateCaballeroUseCase.Execute(caballero);

            // Si result es null, significa que no lo encontré. Entonces mando NotFoundResult
            if (result == null)
                return new NotFoundResult();

            return new OkObjectResult(result);
        }

    }
}
