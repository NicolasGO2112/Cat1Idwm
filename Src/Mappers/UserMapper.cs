using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat1.Src.Dtos;
using Cat1.Src.Models;

namespace Cat1.Src.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User userModel)
        {
            return new UserDto
            {
                Id = userModel.Id,
                Rut = userModel.Rut,
                Nombre = userModel.Nombre,
                Correo = userModel.Correo,
                FechaNacimiento = userModel.FechaNacimiento
            };
        }

        public static User ToUserFromCreatedDto(this CreateUserDto createUserDto)
        {
            return new User
            {
                Rut = createUserDto.Rut,
                Nombre = createUserDto.Nombre,
                Correo = createUserDto.Correo,
                FechaNacimiento = createUserDto.FechaNacimiento
            };
        }
    }
}