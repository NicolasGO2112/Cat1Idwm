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
                return Conflict("El RUT ya existe");
            }
            else if(createUserDto.Nombre.Length <= 3 || createUserDto.Nombre.Length >= 100)
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(!emailAttribute.IsValid(createUserDto.Correo))
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(!generosValidos.Contains(createUserDto.Genero.ToLower()))
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(createUserDto.FechaNacimiento > DateTime.Now)
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else
            {
                var userModel = createUserDto.ToUserFromCreatedDto();
                await _userRepository.Post(userModel);
                var uri = Url.Action("GetUser", new { id = userModel.Id });

                var response = new
                {
                    Message = "Usuario creado exitosamente",
                    User = userModel.ToUserDto() 
                };
                return Created(uri, response);
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
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(updateUserDto.Nombre.Length <= 3 || updateUserDto.Nombre.Length >= 100)
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(!emailAttribute.IsValid(updateUserDto.Correo))
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(!generosValidos.Contains(updateUserDto.Genero.ToLower()))
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            else if(updateUserDto.FechaNacimiento > DateTime.Now)
            {
                return BadRequest("Alguna validación no fue cumplida");
            }
            var response = new
            {
                Message = "Usuario actualizado exitosamente",
                User = userModel
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string order = "", [FromQuery] string gender = "")
        {
            var users = await _userRepository.GetAll();
            var userDto = users.Select(p => p.ToUserDto());
            if (order.ToLower() == "asc"){userDto = userDto.OrderByDescending(u => u.Nombre);}
            else if (order.ToLower() == "desc"){userDto = userDto.OrderBy(u => u.Nombre);}
           
            if(gender.ToLower() == "masculino"){userDto = userDto.Where(u => u.Genero.ToLower() == gender).ToList();}
            else if(gender.ToLower() == "femenino"){userDto = userDto.Where(u => u.Genero.ToLower() == gender).ToList();}
            else if(gender.ToLower() == "otro"){userDto = userDto.Where(u => u.Genero.ToLower() == gender).ToList();}
            else if(gender.ToLower() == "prefiero no decirlo"){userDto = userDto.Where(u => u.Genero.ToLower() == gender).ToList();}
            
            var response = new
            {
                Message = "Usuarios obtenidos exitosamente",
                Users = userDto
            };
            return Ok(response);
        
        }


        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            try{
                var user = await _userRepository.Delete(id);
            }catch
            {
                return NotFound("Usuario no encontrado");
            }
            return Ok("Usuario eliminado exitosamente");
        }
    }
}