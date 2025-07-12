using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LoginPrueba.Api.models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? TokenExpira { get; set; }
    }
}
