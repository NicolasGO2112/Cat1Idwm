using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat1.Src.Dtos;
using Cat1.Src.Models;

namespace Cat1.Src.Interfaces
{
    public interface IUserRepository
    {
        Task <bool> ExistsByRut(string rut);
        Task <User> Post(User product);
        Task <List<User>> GetAll();
        Task <User?> Put(int id, UpdateUserDto updateUserDto);
        Task <User?> Delete(int id);
    }
}