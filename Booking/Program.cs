
using Booking.Middleware;

namespace Booking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var startUp = new StartUp(builder);
            startUp.ConfigureServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("AllowReactApp");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
