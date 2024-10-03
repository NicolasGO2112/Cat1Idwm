using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat1.Src.Interfaces;
using Cat1.Src.Dtos;
using Microsoft.AspNetCore.Mvc;
using Cat1.Src.Mappers;

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

            if(exist)
            {
                return Conflict("El codigo del producto ya existe");
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
            var userModel = await _userRepository.Put(id, updateUserDto);
            if(userModel == null)
            {
                return NotFound();
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