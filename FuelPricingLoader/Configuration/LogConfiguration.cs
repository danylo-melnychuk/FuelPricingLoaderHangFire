using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace FuelPricingLoader.Configuration;

public class LogConfiguration
{
	public static void ConfigureLogging()
	{
		var isProduction = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") != "Development";
		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithMachineName()
			.Enrich.WithEnvironmentName()
			.Enrich.WithExceptionDetails()
			.WriteTo.Console(
				isProduction ? LogEventLevel.Warning : LogEventLevel.Information, 
				theme: AnsiConsoleTheme.Code)
			.WriteTo.Debug(isProduction ? LogEventLevel.Warning : LogEventLevel.Information)
			.WriteTo.File(
				 "logs.txt",
				 rollingInterval: RollingInterval.Day,
				 restrictedToMinimumLevel: isProduction ? LogEventLevel.Error : LogEventLevel.Warning)
			.CreateLogger();
	}
}