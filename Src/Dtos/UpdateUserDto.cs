using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cat1.Src.Dtos
{
    public class UpdateUserDto
    {

        [Required]
        public string Rut { get; set; } = string.Empty; 

        [Required]
         [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Correo { get; set; } = string.Empty;

        [Required]

        public string Genero { get; set; } = string.Empty;

        [Required]
        public DateTime FechaNacimiento { get; set; }

    }
    
}