using Microsoft.EntityFrameworkCore;
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
