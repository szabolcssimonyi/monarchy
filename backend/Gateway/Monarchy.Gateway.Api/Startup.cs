using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Monarchy.Core.Extensibility.Extension;
using Monarchy.Gateway.Api.Filters;
using Monarchy.Gateway.Extensibility.Profile;
using Serilog;
using System.Reflection;
using System.Text;
using GatewayConfiguration = Monarchy.Gateway.Extensibility.Configuration;

namespace Monarchy.Gateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore(options => options.Filters.Add(typeof(GlobalExceptionFilter)));
            services.AddAutoMapper(typeof(GatewayProfile));

            services.Configure<GatewayConfiguration>(Configuration);
            var configuration = Configuration.Get<GatewayConfiguration>();

            services.AddCors(options => options.AddPolicy(name: "CorsPolicy", b =>
              {
                  b.AllowAnyMethod()
                  .AllowAnyHeader()
                  //.AllowCredentials()
                  .AllowAnyOrigin();
              }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Monarchy Gateway", Version = "v1" });
            });

            var secret = Encoding.ASCII.GetBytes(configuration.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                foreach (var permission in configuration.Permissions)
                {
                    options.AddPolicy(permission, policy =>
                        policy.Requirements.Add(new AuthorizationRequirement(permission)));
                }
            });

            services.AddOptions();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var configuration = Configuration.Get<GatewayConfiguration>();
            builder.InitializeMassTransit(Assembly.GetExecutingAssembly(), configuration.GatewayBus);
            builder.RegisterModule(new AutofacModule(configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            loggerFactory.AddSerilog();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
