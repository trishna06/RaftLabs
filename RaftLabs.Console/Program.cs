using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RaftLabs.Application;
using RaftLabs.Application.Configuration;
using RaftLabs.Domain.Interfaces;

var builder = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ApiSettings>(context.Configuration.GetSection("ApiSettings"));
        services.AddRaftLabsApplication();
    });



var host = builder.Build();

using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IExternalUserService>();

try
{
    var users = await service.GetAllUsersAsync();
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName} - {user.Email}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.Message}");
}