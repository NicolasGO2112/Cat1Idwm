using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat1.Src.Data;
using Cat1.Src.Dtos;
using Cat1.Src.Interfaces;
using Cat1.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Cat1.Src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<User?> Delete(int id)
        {
            var userModel = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(userModel == null)
            {
                throw new Exception("Product not found");
            }
            _dataContext.Users.Remove(userModel);
            await _dataContext.SaveChangesAsync();
            return userModel;
        }

        public async Task<bool> ExistsByRut(string rut)
        {
            return await _dataContext.Users.AnyAsync(x => x.Rut == rut);
        }

        public async Task<List<User>> GetAll()
        {
            return await _dataContext.Users.ToListAsync();
        }

        public async Task<User> Post(User user)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Put(int id, UpdateUserDto updateUserDto)
        {
            var userModel = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userModel == null)
            {
                throw new Exception("Product not found");
            }
            userModel.Nombre = userModel.Nombre;
            userModel.Correo = userModel.Correo;
            userModel.Rut = userModel.Rut;
            userModel.Genero = userModel.Genero;
            userModel.FechaNacimiento = userModel.FechaNacimiento;
            await _dataContext.SaveChangesAsync();
            return userModel;
        }
    }
}