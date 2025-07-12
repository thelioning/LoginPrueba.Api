using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LoginPrueba.Api.Models
{
    public class CambiarClaveRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
    }
}