using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Cat1.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace Cat1.Src.Data
{
    public class Seeder
    {
        public static async Task Seed(DataContext dataContext)
        {
            var faker = new Faker("es");

            // Seed de usuarios generados aleatoriamente
            var usuarios = new List<User>();

            for (int i = 1; i <= 10; i++) // Generamos 10 usuarios de ejemplo
            {
                var genero = faker.PickRandom(new[] { "masculino", "femenino", "otro", "prefiero no decirlo" });
                var user = new User
                {
                    Id = i,
                    Rut = GenerarRutUnico(faker, usuarios),
                    Nombre = faker.Name.FullName(),
                    Correo = faker.Internet.Email(),
                    Genero = genero,
                    FechaNacimiento = faker.Date.Past(80, DateTime.Now.AddYears(-18)) // Edad mínima 18 años
                };
                usuarios.Add(user);
                await dataContext.Users.AddAsync(user);
            }

            await dataContext.SaveChangesAsync();
        }

        // Genera un RUT único para cada usuario en la lista.
            private static string GenerarRutUnico(Faker faker, List<User> usuarios)
        {
            string rut;
            do
            {
                rut = faker.Random.ReplaceNumbers("########-#"); // RUT simulado
            } while (usuarios.Any(u => u.Rut == rut)); // Asegura que el RUT sea único

            return rut;
        }
    }

}