using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Smart_trafic_controller_api.Data;
using Smart_trafic_controller_api.Interfaces;
using Smart_trafic_controller_api.Repositories;
using Smart_trafic_controller_api.Services;

namespace Smart_trafic_controller_api
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
                options.UseMySql(
                    conn,
                    ServerVersion.AutoDetect(conn)
                ));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITrafficEventService, TrafficEventService>();
            builder.Services.AddScoped<ITrafficEventRepository, TrafficEventRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string? jwtSecretKey = builder.Configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtSecretKey))
            {
                // Use a default key for design-time scenarios
                jwtSecretKey = "This-is-my-very-long-random-secret-key-379";
            }

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "smart trafic controller api",
                        ValidAudience = builder.Configuration["JwtSettings:Audience"] ?? "smart trafic controller api users",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSecretKey))
                    };
                });

            builder.Services.AddControllers().AddNewtonsoftJson(Options =>
            {
                Options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
