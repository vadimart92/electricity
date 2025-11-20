using electricity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


static string GetEnvironmentVariable(string name) {
	return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Variable {name} is not set");
}

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();
builder.Services.AddDbContext<DatabaseContext>(optionsBuilder => {
		optionsBuilder.UseCosmos(GetEnvironmentVariable("ConnectionString"), "electricity");
	})
	.AddHttpClient()
	.AddSingleton<INotifier, TelegramNotifier>()
	.AddScoped<IStateProcessor, StateProcessor>();

builder.Build().Run();