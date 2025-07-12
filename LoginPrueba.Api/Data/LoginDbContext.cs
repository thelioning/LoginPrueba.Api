using LoginPrueba.Api.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LoginPrueba.Api.Data
{
    public class LoginDbContext: DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options){ }
        public DbSet<Usuario> Usuarios => Set<Usuario>();

    }

}
