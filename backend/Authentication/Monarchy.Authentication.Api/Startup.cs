using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Monarchy.Authentication.Domain.Context;
using Monarchy.Authentication.Extensibility.Interface;
using Monarchy.Authentication.Extensibility.Model;
using Monarchy.Authentication.Extensibility.Profile;
using Monarchy.Core.Extensibility.Extension;
using Monarchy.Core.Extensibility.Interface;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;
using AuthConfiguration = Monarchy.Authentication.Extensibility.Configuration;

namespace Monarchy.Authentication.Api
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<AuthConfiguration>(Configuration);
            services.AddAutoMapper(typeof(AuthenticationProfile));

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<AuthenticationDbContext>();
            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Monarchy Authentication", Version = "v1" });
            });

            services.AddOptions();

            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var configuration = Configuration.Get<AuthConfiguration>();
            builder.InitializeMassTransit(Assembly.GetExecutingAssembly(), configuration.AuthenticationBus);
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            loggerFactory.AddSerilog();

            MigrateAndSeedAsync(serviceProvider).GetAwaiter().GetResult();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task MigrateAndSeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AuthenticationDbContext>();
            context.Database.Migrate();
            var seeder = serviceProvider.GetRequiredService<IDomainSeeder>();
            await seeder.SeedAsync(context);
        }
    }
}
