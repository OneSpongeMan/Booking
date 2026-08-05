using Booking.BLL.Services;
using Booking.DAL.Repos;
using Booking.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text;


namespace Booking
{
    public class StartUp
    {
        WebApplicationBuilder _builder;

        public StartUp(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public void ConfigureServices()
        {
            _builder.Services.AddControllers();
            _builder.Services.AddEndpointsApiExplorer();
            //_builder.Services.AddAuthorization();

            AddDataStorageConfiguration();
        }

        private void AddDataStorageConfiguration()
        {
            var bookingsRelativePath = _builder.Configuration["DataStorage:BookingsPath"];
            var guestsRelativePath = _builder.Configuration["DataStorage:GuestsPath"];
            var tablesRelativePath = _builder.Configuration["DataStorage:TablesPath"];

            //var bookingsAbsolutePath = Path.Combine(_builder.Environment.ContentRootPath, bookingsRelativePath);
            var dataDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            var bookingsAbsolutePath = Path.Combine(dataDirectory, bookingsRelativePath);
            var guestsAbsolutePath = Path.Combine(dataDirectory, guestsRelativePath);
            var tablesAbsolutePath = Path.Combine(dataDirectory, tablesRelativePath);

            //var dataDirectory = Path.GetDirectoryName(bookingsAbsolutePath);
            //var dataDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            if (!Directory.Exists(Path.GetDirectoryName(bookingsAbsolutePath)))                
                Directory.CreateDirectory(Path.GetDirectoryName(bookingsAbsolutePath));

            if (!Directory.EnumerateFiles(Path.GetDirectoryName(bookingsAbsolutePath)).Any())
            {
                File.WriteAllText(bookingsAbsolutePath, "[]");
                File.WriteAllText(guestsAbsolutePath, "[]");
                File.WriteAllText(tablesAbsolutePath, "[]");
            }

            //_builder.Services.AddAutoMapper(typeof(MappingProfiles));
            _builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });

            _builder.Services.AddSingleton<IReservationRepository, ReservationRepository>(provider => new ReservationRepository(bookingsAbsolutePath));
            _builder.Services.AddSingleton<IGuestRepository, GuestRepository>(provider => new GuestRepository(guestsAbsolutePath));
            _builder.Services.AddSingleton<ITableRepository, TableRepository>(provider => new TableRepository(tablesAbsolutePath));

            _builder.Services.AddScoped<IReservationService, ReservationService>();
            _builder.Services.AddScoped<IGuestService, GuestService>();
            _builder.Services.AddScoped<ITableService, TableService>();
        }
    }
}
