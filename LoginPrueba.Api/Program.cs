﻿using LoginPrueba.Api.Data;
using LoginPrueba.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión a SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LoginDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔹 Soporte para controladores (si decides usarlos luego)
builder.Services.AddControllers();

var app = builder.Build();

// 🔹 Habilitar archivos estáticos (para servir recuperar.html desde wwwroot)
app.UseDefaultFiles(); // para que cargue recuperar.html por defecto si está presente
app.UseStaticFiles();

// 🔹 HTTPS redirection solo si estás en local (Render ya usa HTTPS)
app.UseHttpsRedirection();

// 🔹 (Opcional) Rutas de controlador
app.MapControllers();


// 🟦 Endpoint para solicitar recuperación de contraseña
app.MapPost("/api/recuperar", async (LoginDbContext db, RecuperarClaveRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Correo))
        return Results.BadRequest(new { error = "Debe ingresar un correo." });

    var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Correo == req.Correo);
    if (usuario == null)
        return Results.BadRequest(new { error = "El correo no está registrado." });

    var token = GenerarTokenSeguro();
    usuario.Token = token;
    usuario.TokenExpira = DateTime.UtcNow.AddHours(1);
    await db.SaveChangesAsync();

    var url = $"https://helpful-truffle-bae1e9.netlify.app/cambiar.html?token={token}";


    var remitente = "theliondjprodutions@gmail.com";
    var clave = "xeqjlfvfjrkkcahc"; // 🔐 Recuerda proteger esto en producción

    var smtp = new SmtpClient("smtp.gmail.com")
    {
        Port = 587,
        Credentials = new NetworkCredential(remitente, clave),
        EnableSsl = true
    };

    var mensaje = new MailMessage
    {
        From = new MailAddress(remitente, "Soporte PrestamoApp"),
        Subject = "Recuperación de contraseña",
        IsBodyHtml = true,
        Body = $@"<h3>Recuperación de contraseña</h3>
                  <p>Haz clic en el siguiente enlace para restablecer tu contraseña:</p>
                  <a href='{url}'>{url}</a>
                  <p>Este enlace expirará en 1 hora.</p>"
    };

    mensaje.To.Add(req.Correo);

    try
    {
        await smtp.SendMailAsync(mensaje);
        return Results.Ok(new { message = "Correo enviado con instrucciones." });
    }
    catch (Exception ex)
    {
        return Results.Problem("Error al enviar el correo: " + ex.Message);
    }
});

// 🟩 Endpoint para cambiar la clave
app.MapPost("/api/cambiar-clave", async (LoginDbContext db, CambiarClaveRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Token) || string.IsNullOrWhiteSpace(req.NuevaClave))
        return Results.BadRequest(new { error = "Token y nueva clave son obligatorios." });

    var usuario = await db.Usuarios.FirstOrDefaultAsync(u =>
        u.Token == req.Token && u.TokenExpira > DateTime.UtcNow);

    if (usuario == null)
        return Results.BadRequest(new { error = "Token inválido o expirado." });

    usuario.Clave = req.NuevaClave;
    usuario.Token = null;
    usuario.TokenExpira = null;
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "Contraseña actualizada correctamente." });
});

// 🔐 Token seguro aleatorio
string GenerarTokenSeguro(int longitud = 64)
{
    using var rng = RandomNumberGenerator.Create();
    var bytes = new byte[longitud];
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes)
        .Replace("+", "")
        .Replace("/", "")
        .Replace("=", "");
}

app.Run();
