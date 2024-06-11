using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using AlzheimerWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Agregar servicios al contenedor
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AlzheimerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), builder =>
                builder.UseNetTopologySuite()).UseLazyLoadingProxies());

            builder.Services.AddScoped<PersonasService>();
            builder.Services.AddScoped<CuidadoresService>();
            builder.Services.AddScoped<FamiliaresService>();
            builder.Services.AddScoped<GeocercasService>();
            builder.Services.AddScoped<PacientesCuidadoresService>();
            builder.Services.AddScoped<UsuariosService>();
            builder.Services.AddScoped<AutenticacionService>(sp =>
                new AutenticacionService("770A8A65DA156D24EE2A093277530142"));
            builder.Services.AddScoped<TiposUsuariosService>();
            builder.Services.AddScoped<PacientesService>();
            builder.Services.AddScoped<MedicamentosService>();
            builder.Services.AddScoped<UbicacionesService>();
            builder.Services.AddScoped<DispositivosService>();
            builder.Services.AddScoped<PacientesFamiliaresService>();
            builder.Services.AddScoped<TiposNotificacionesService>();
            builder.Services.AddScoped<NotificacionesService>();
            builder.Services.AddHostedService<UbicacionBackgroundService>();
            builder.Services.AddHttpClient();

            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var secretKey = "770A8A65DA156D24EE2A093277530142";
            var key = Encoding.ASCII.GetBytes(secretKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "your-issuer",
                        ValidAudience = "your-audience",
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    // Configurar SignalR para usar tokens
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // Si la solicitud es para el Hub de SignalR, el token se pasa como un parámetro de consulta
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/notificationHub")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrador"));
                options.AddPolicy("RequireFamiliarRole", policy => policy.RequireRole("Familiar"));
                options.AddPolicy("RequireCuidadorRole", policy => policy.RequireRole("Cuidador"));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<AlzheimerHub>("/notificationHub");

            app.Run();

        }
    }
}
