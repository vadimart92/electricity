using electricity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

static string GetEnvironmentVariable(string name) {
    return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Variable {name} is not set");
}

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((context, builder) => {
        builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {
            optionsBuilder.UseCosmos(GetEnvironmentVariable("ConnectionString"), "electricity");
        }).AddHttpClient()
            .AddSingleton<INotifier, TelegramNotifier>()
            .AddScoped<IStateProcessor, StateProcessor>();
    })
    .Build();

host.Run();
