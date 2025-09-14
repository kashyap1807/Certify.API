
using Certify.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace Certify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Configuration Services - start
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CertifyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
                
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            #endregion Configuration Services - end

            #region Configuration Middleware - start
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options => {
                    options.WithTitle("My API");
                    options.WithTheme(ScalarTheme.Default);
                    options.WithSidebar(true);
                });

                app.UseSwaggerUi(options =>
                {
                    options.DocumentPath = "openapi/v1.json"; 
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion Configuration Middleware - end
        }
    }
}
