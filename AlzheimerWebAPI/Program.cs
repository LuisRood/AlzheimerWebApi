
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using AlzheimerWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Base de datos
            builder.Services.AddDbContext<AlzheimerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),builder =>
                builder.UseNetTopologySuite()).UseLazyLoadingProxies());

            // Aquí agregamos los servicios necesarios
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
            builder.Services.AddHostedService<UbicacionBackgroundService>();
            builder.Services.AddHttpClient();

            //Add SignalR
            builder.Services.AddSignalR();
            //CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use CORS with the specified policy
            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();


            app.MapControllers();

            //Notification Hub
            app.MapHub<AlzheimerHub>("/notificationHub");

            app.Run();
        }
    }
}
