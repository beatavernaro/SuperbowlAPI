using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperbowlAPI.Auth;
using SuperbowlAPI.Context;
using SuperbowlAPI.Filters;
using SuperbowlAPI.Interfaces;
using SuperbowlAPI.Repositories;
using System.Text;

namespace SuperbowlAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); //inicializa a classe webapplication, possui proriedades que vai alterando conforme utiliza

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomLogFilter));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Informe o token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }

                });
            });

            builder.Services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase("Superbowl")); //Conexão InMemoryDatabase
            //var variavelsecreta = builder.Configuration["nomedavariavel"]; para todos os dados 
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); //Injeção de dependencia
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository)); //Injeção de dependencia

            
            var TokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration")).Configure(TokenConfiguration);
            builder.Services.AddSingleton(TokenConfiguration);
            var generateToken = new GenerateToken(TokenConfiguration);
            builder.Services.AddScoped(typeof(GenerateToken));


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = TokenConfiguration.Audience,
                    ValidIssuer = TokenConfiguration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfiguration.Secret + '\0'))
                };
            });

            builder.Services.AddAuthorization(options => options.AddPolicy("ValidateClaimModule", policy => policy.RequireClaim("module", "teste")));

            

            builder.Services.AddTransient<DataGenerator>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
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