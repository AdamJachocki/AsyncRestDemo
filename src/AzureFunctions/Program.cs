using AzureFunctions;
using DAL.CosmosDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddUserSecrets(Assembly.GetExecutingAssembly());
    })
    .ConfigureServices((host, services) =>
    {
        services.AddSingleton<Randomizer>();
        services.AddCosmosDb(host.Configuration.GetSection("CosmosDb"));
    })
    .Build();

host.Run();
