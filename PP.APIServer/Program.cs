using Microsoft.EntityFrameworkCore;
using PP.APIServer.Models;

namespace PP.APIServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // agregar contexto de base de datos 

            //usamos contexto que declaramos
            builder.Services.AddDbContext<PacienteContext>(o =>
            {
                //usamos usql server y nos conectamos a una cadena de conexion declarada con anterioridad llamada
                //DefaultConnections que se encuentra en el appsettions.json
                o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var misReglasCors = "ReglasCors";
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: misReglasCors, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{

            //}
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(misReglasCors);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}