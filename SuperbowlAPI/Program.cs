using Microsoft.EntityFrameworkCore;
using SuperbowlAPI.Context;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Repositories;

namespace SuperbowlAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); //inicializa a classe webapplication, possui proriedades que vai alterando conforme utiliza

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("Superbowl")); //Conexão InMemoryDatabase
            //var variavelsecreta = builder.Configuration["nomedavariavel"]; para todos os dados 
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); //Injeção de dependencia
            builder.Services.AddTransient<DataGenerator>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopedFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<DataGenerator>();
                service.Generate();
            }

                app.Run();
        }
    }
}