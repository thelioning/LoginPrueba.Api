using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LoginPrueba.Api.Data
{
    public class LoginDbContextFactory : IDesignTimeDbContextFactory<LoginDbContext>
    {
        public LoginDbContext CreateDbContext(string[] args)
        {
            // Establece la ruta al directorio base (en general, el directorio del proyecto)
            var basePath = Directory.GetCurrentDirectory();

            // Construye la configuración leyendo el archivo appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LoginDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura el DbContext para usar SQL Server con la cadena de conexión
            optionsBuilder.UseSqlServer(connectionString);

            return new LoginDbContext(optionsBuilder.Options);
        }
    }
}
