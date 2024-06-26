using ClaimsPlugin.Api.Configurations;
using ClaimsPlugin.Application.Handlers.AuthHandlers;
using ClaimsPlugin.Application.Services;
using ClaimsPlugin.Application.Services.Interfaces;
using ClaimsPlugin.Domain.Interfaces;
using ClaimsPlugin.Domain.Models;
using ClaimsPlugin.Infrastructure;
using ClaimsPlugin.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using AutoMapper;


[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace ClaimsPlugin.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(builder);
            ConfigureServices(builder);

            var app = builder.Build();
            ConfigureMiddleware(app);
            ConfigureEndpoints(app);

            MigrateDatabase(app);

            app.Run();
        }

        private static void ConfigureLogging(WebApplicationBuilder builder)
        {
            Log.Information("Claims Plugin API Microservice Booting Up...");
            builder.Host.UseSerilog(
                (_, services, configuration) =>
                    configuration
                        .ReadFrom.Configuration(builder.Configuration)
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
            );
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.AddConfigurations(builder.Environment);

            builder.Services.AddControllers();
            builder.Services.AddMediatR(typeof(LoginCommandHandler).Assembly);

            // Register AutoMapper - Ensure this comes after AddControllers()
            //builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // Register application services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Register your DbContext here, before the WebApplication is built
            builder.Services.AddDbContext<DatabaseContext>(
                options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("BackendDb"))
            );

            builder.Services.AddSwaggerGen(
                c =>
                    c.SwaggerDoc(
                        "v1",
                        new OpenApiInfo { Title = "Claims Plugin API", Version = "v1" }
                    )
            );
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Claims Plugin API v1")
                );
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static void ConfigureEndpoints(WebApplication app)
        {
            app.MapControllers();
        }

        private static void MigrateDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<DatabaseContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DatabaseContext>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
