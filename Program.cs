using electricity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

static string GetEnvironmentVariable(string name) {
	return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Variable {name} is not set");
}

var host = new HostBuilder()
	.ConfigureFunctionsWorkerDefaults()
	.ConfigureServices(services => {
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();
		services.AddDbContext<DatabaseContext>(optionsBuilder => {
				optionsBuilder.UseCosmos(GetEnvironmentVariable("ConnectionString"), "electricity");
			})
			.AddHttpClient()
			.AddSingleton<INotifier, TelegramNotifier>()
			.AddScoped<IStateProcessor, StateProcessor>();
	})
	.Build();

await host.RunAsync();
