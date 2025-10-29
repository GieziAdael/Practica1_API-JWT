using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;
using Practica_API_JWT.Repositories;
using Practica_API_JWT.Repositories.IRepositories;
using Practica_API_JWT.Services;

namespace Practica_API_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin,modd")]
    public class ObjetoController : ControllerBase
    {
        private readonly IObjetoRepository _objetoRepository;
        private readonly IMapper _mapper;

        public ObjetoController(IObjetoRepository objetoRepository, IMapper mapper)
        {
            _objetoRepository = objetoRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetObjetos()
        {
            var accion = await _objetoRepository.GetObjetos();
            var registrar = _mapper.Map<List<ObjetoDto>>(accion);
            return Ok(registrar);
        }

        [HttpGet("Get/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserById(int id)
        {
            var accion = await _objetoRepository.GetObjeto(id);
            if (accion == null) return NotFound($"No se encontro el objeto con el Id {id}");
            var registro = _mapper.Map<ObjetoDto>(accion);
            return Ok(registro);
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostObjeto([FromBody] ObjetoCreateDto objetoCreateDto)
        {
            //Comprobaciones
            if (objetoCreateDto == null) return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(objetoCreateDto.Name)) return BadRequest("Se requiere un nombre");
            if (await _objetoRepository.ExistName(EmailNormalized.NormalizarEmail(objetoCreateDto.Name)))
                return BadRequest($"Ya se encuentra registrado el objeto '{objetoCreateDto.Name}', usa otro!");

            //Ingresar los datos a la entidad
            var nameNormalized = EmailNormalized.NormalizarEmail(objetoCreateDto.Name);

            var newRegistro = new Objeto()
            {
                Name = nameNormalized,  
                DateObjectCreated = DateTime.Now
            };

            var registro = _mapper.Map<Objeto>(newRegistro);

            //Registrarlo a la DB
            var accion = await _objetoRepository.CreateObjeto(registro);
            //Validar si se creo el registro
            if (!await _objetoRepository.ExistName(nameNormalized))
                return BadRequest("Algo salio mal al registrar el objeto");

            return Ok(registro);
        }

        [HttpPatch("ActualizarNombre/{id:int}/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchName(int id, string name)
        {
            //Comprobaciones
            if (id <= 0)
                return BadRequest($"No existe el id {id}");
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("No se acepta este formato");
            if (await _objetoRepository.ExistName(EmailNormalized.NormalizarEmail(name)))
                return BadRequest("Este nombre ya se encuentra en uso");

            var registro = await _objetoRepository.GetObjeto(id);
            if (registro == null)
                return NotFound($"No se encontro el objeto con el id {id}");

            //Actualizacion
            if (!await _objetoRepository.PatchObjeto(id, name))
            {
                return BadRequest("Algo salio mal al actualizar");
            }
            registro = await _objetoRepository.GetObjeto(id);

            return Ok(registro);

        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteObjeto(int id)
        {
            // Comprobaciones
            if (id <= 0)
                return BadRequest($"No existe el id {id}");
            var registro = await _objetoRepository.GetObjeto(id);
            if (registro == null)
                return NotFound($"No se encontro el usuario con el id {id}");
            //Eliminacion y comprobacion
            if (!await _objetoRepository.DeleteObjeto(registro))
                return BadRequest("No se logro eliminar el registro");
            return NoContent();
        }
    }
}
