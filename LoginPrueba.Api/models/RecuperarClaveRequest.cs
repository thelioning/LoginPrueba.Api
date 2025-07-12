using System.Net;
using System.Net.Mail;


namespace LoginPrueba.Api.Models
{
    public class RecuperarClaveRequest
    {
        public string Correo { get; set; } = string.Empty;
    }
}
