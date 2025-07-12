using System.Net;
using System.Net.Mail;



namespace LoginPrueba.Api.Models
{
    public class CambiarClaveRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NuevaClave { get; set; } = string.Empty;
    }
}
