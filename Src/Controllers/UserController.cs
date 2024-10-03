using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat1.Src.Interfaces;
using Cat1.Src.Dtos;
using Microsoft.AspNetCore.Mvc;
using Cat1.Src.Mappers;
using System.ComponentModel.DataAnnotations;

namespace Cat1.Src.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
         private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateUserDto createUserDto)
        {
            bool exist = await _userRepository.ExistsByRut(createUserDto.Rut);
            EmailAddressAttribute emailAttribute = new EmailAddressAttribute();
            List<string> generosValidos = new List<string> { "masculino", "femenino", "otro", "prefiero no decirlo" };

            if(exist)
            {
                return Conflict("El Rut del usuario ya existe");
            }
            else if(createUserDto.Nombre.Length <= 3 || createUserDto.Nombre.Length >= 100)
            {
                return BadRequest("El Nombre debe tener entre 3 y 100 caracteres");
            }
            else if(!emailAttribute.IsValid(createUserDto.Correo))
            {
                return BadRequest("Debe ingresar un correo valido");
            }
            else if(!generosValidos.Contains(createUserDto.Genero.ToLower()))
            {
                return BadRequest("Debe ingresar un genero valido");
            }
            else if(createUserDto.FechaNacimiento > DateTime.Now)
            {
                return BadRequest("Debe ingresar una fecha valida");
            }
            else
            {
                var userModel = createUserDto.ToUserFromCreatedDto();
                await _userRepository.Post(userModel);
                return Created();
            }

        }

        [HttpPut]
        [Route("{id}")]

        public async Task<IActionResult> Put([FromRoute] int id , [FromBody] UpdateUserDto updateUserDto)
        { 
            EmailAddressAttribute emailAttribute = new EmailAddressAttribute();
            List<string> generosValidos = new List<string> { "masculino", "femenino", "otro", "prefiero no decirlo" };
            bool exist = await _userRepository.ExistsByRut(updateUserDto.Rut);
            var userModel = await _userRepository.Put(id, updateUserDto);
            if(userModel == null)
            {
                return NotFound();
            }
            if(exist) 
            {
                return BadRequest("El Rut del usuario ya existe");
            }
            else if(updateUserDto.Nombre.Length <= 3 || updateUserDto.Nombre.Length >= 100)
            {
                return BadRequest("El Nombre debe tener entre 3 y 100 caracteres");
            }
            else if(!emailAttribute.IsValid(updateUserDto.Correo))
            {
                return BadRequest("Debe ingresar un correo valido");
            }
            else if(!generosValidos.Contains(updateUserDto.Genero.ToLower()))
            {
                return BadRequest("Debe ingresar un genero valido");
            }
            else if(updateUserDto.FechaNacimiento > DateTime.Now)
            {
                return BadRequest("Debe ingresar una fecha valida");
            }
            return Ok(userModel.ToUserDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAll();
            var userDto = users.Select(p => p.ToUserDto());
            return Ok(userDto);
        
        }


        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.Delete(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}