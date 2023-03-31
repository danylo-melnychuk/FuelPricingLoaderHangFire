using Microsoft.Extensions.Configuration;

namespace FuelPricingLoader.Configuration;

public class SystemConfiguration
{
	public static IConfigurationRoot BuildConfiguration() => new ConfigurationBuilder()
		.AddJsonFile("appsettings.json", true, true)
#if DEBUG
		.AddJsonFile("appsettings.Development.json", true, true)
#else
		.AddJsonFile(
			$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
			true,
			true)
#endif
		.AddEnvironmentVariables()
		.Build();
}