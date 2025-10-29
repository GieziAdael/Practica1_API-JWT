using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;
using Practica_API_JWT.Repositories.IRepositories;
using Practica_API_JWT.Services;

namespace Practica_API_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers()
        {
            var accion = await _userRepository.GetUsers();
            var registrar = _mapper.Map<List<UserDto>>(accion);
            return Ok(registrar);
        }

        [HttpGet("Get/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var accion = await _userRepository.GetUser(id);
            if(accion == null) return NotFound($"No se encontro usuario con el Id {id}");
            var registro = _mapper.Map<UserDto>(accion);
            return Ok(registro);
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> PostUser([FromBody] UserCreateDto userCreateDto)
        {
            //Comprobaciones
            if (userCreateDto == null) return BadRequest(ModelState);
            if(string.IsNullOrWhiteSpace(userCreateDto.Email)) return BadRequest("Se requiere un email");
            if (await _userRepository.ExistEmail(EmailNormalized.NormalizarEmail(userCreateDto.Email)))
                return BadRequest($"Ya se encuentra registrado el correo {userCreateDto.Email}, usa otro!");
            if (string.IsNullOrWhiteSpace(userCreateDto.Password)) return BadRequest("Se requiere un password");
            if (userCreateDto.Password.Length <= 7)
                return BadRequest("El password requiere una longitud minima de 8");

            //Ingresar los datos a la entidad


            var entidad = new User()
            {
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.Password,
                AccountCreated = DateTime.Now,
                Role = userCreateDto.Role
            };

            var registro = _mapper.Map<User>(entidad);
            //Registrarlo a la DB
            var accion = await _userRepository.CreateUser(registro);

            //Validar si se creo el registro
            var emailNormalized = EmailNormalized.NormalizarEmail(userCreateDto.Email);
            if (! await _userRepository.ExistEmail(emailNormalized))
                return BadRequest("Algo salio mal al registrar el usuario");

            return Ok(registro);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
        {
            if (userLoginDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.LoginUser(userLoginDto);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(user);
        }


        [HttpPatch("ActualizarEmail/{id:int}/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> PatchEmail(int id, string email)
        {
            //Comprobaciones
            if (id <= 0)
                return BadRequest($"No existe el id {id}");
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("No se acepta este formato de email");
            if (await _userRepository.ExistEmail(EmailNormalized.NormalizarEmail(email)))
                return BadRequest("Este email ya se encuentra en uso");

            var registro = await _userRepository.GetUser(id);
            if (registro == null)
                return NotFound($"No se encontro el usuario con el id {id}");

            //Actualizacion
            if(!await _userRepository.PatchUser(id, email))
            {
                return BadRequest("Algo salio mal al actualizar");
            }
            registro = await _userRepository.GetUser(id);

            return Ok(registro);

        }

        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Comprobaciones
            if (id <= 0)
                return BadRequest($"No existe el id {id}");
            var registro = await _userRepository.GetUser(id);
            if (registro == null)
                return NotFound($"No se encontro el usuario con el id {id}");
            //Eliminacion y comprobacion
            if (!await _userRepository.DeleteUser(registro))
                return BadRequest("No se logro eliminar el registro");
            return NoContent();
        }

    }
}
