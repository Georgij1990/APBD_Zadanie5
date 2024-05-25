using APBD_05.Context;
using APBD_05.Services;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        ConfigureApp(app);
        app.Run();
    }
    static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddXmlSerializerFormatters();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var connection = configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<Context>(options =>
            options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

        services.AddScoped<ITripService, TripService>();
        services.AddScoped<IClientService, ClientService>();
    }

    static void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
    }
}