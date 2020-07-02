using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monarchy.Authentication.Extensibility.Model;
using System;
using System.IO;

namespace Monarchy.Authentication.Domain.Context
{
    public class AuthenticationDbContext: IdentityDbContext<UserModel>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
                if (!string.IsNullOrEmpty(environment))
                {
                    environment += ".";
                }
                var path = Path.Combine($"appsettings.{environment}json");
                Console.WriteLine(path);
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json")
                    .AddJsonFile($"appsettings.{environment}json").Build();
                var builder = new DbContextOptionsBuilder<AuthenticationDbContext>();
                connectionString = configuration.GetConnectionString("DatabaseConnection");
            }
            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly("Monarchy.Authentication.Domain"));
        }
    }
}
