using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Smart_traffic_controller_api.BackgroundServices;
using Smart_traffic_controller_api.Data;
using Smart_traffic_controller_api.Interfaces;
using Smart_traffic_controller_api.Middleware;
using Smart_traffic_controller_api.Repositories;
using Smart_traffic_controller_api.Services;

namespace Smart_traffic_controller_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(conn))
                throw new Exception("DefaultConnection string is missing or not loaded!");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(conn, ServerVersion.AutoDetect(conn))
            );

            builder.Services.AddScoped<ISensorLogService, SensorLogService>();
            builder.Services.AddScoped<ISensorLogRepository, SensorLogRepository>();

            builder.Services.AddHostedService<MqttSubscriberBackgroundService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string jwtSecretKey =
                builder.Configuration["JwtSettings:Key"]
                ?? throw new Exception("Error while building");

            builder
                .Services.AddAuthentication("Bearer")
                .AddJwtBearer(
                    "Bearer",
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer =
                                builder.Configuration["JwtSettings:Issuer"]
                                ?? "smart traffic controller api",
                            ValidAudience =
                                builder.Configuration["JwtSettings:Audience"]
                                ?? "smart traffic controller api users",
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSecretKey)
                            ),
                        };
                    }
                );

            builder
                .Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); // CRITICAL: Must be BEFORE UseAuthorization()
            app.UseAuthorization();
            app.MapControllers();
            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application failed to start: {ex.Message}");
                Environment.Exit(1); // terminate the app if startup fails
            }
        }
    }
}
