using FuelPricingLoader.Configuration;
using FuelPricingLoader.Configuration.Models;
using FuelPricingLoader.DataAccess;
using FuelPricingLoader.DataAccess.Abstraction;
using FuelPricingLoader.DataAccess.Implementation;
using FuelPricingLoader.Services.Abstraction;
using FuelPricingLoader.Services.Implementation;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = SystemConfiguration.BuildConfiguration();

LogConfiguration.ConfigureLogging();

GlobalConfiguration.Configuration
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(
		   configuration.GetConnectionString("DbConnection"),
		   new SqlServerStorageOptions
		   {
			   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
			   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
			   QueuePollInterval = TimeSpan.Zero,
			   UseRecommendedIsolationLevel = true,
			   DisableGlobalLocks = true
		   });

DbUpConfiguration.MigrateDatabase();

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging();
serviceCollection.AddTransient<IConfiguration>(_ => configuration);
serviceCollection.Configure<SystemSettings>(configuration.GetSection("SystemSettings"));
serviceCollection.AddHttpClient("FuelPriceClient").AddPolicyHandler(HttpClientConfiguration.RetryPolicy);
serviceCollection.AddTransient<IDataConnection, DataConnection>();
serviceCollection.AddTransient<IFuelRepository, FuelRepository>();
serviceCollection.AddTransient<IFuelPriceService, FuelPriceService>();
var serviceProvider = serviceCollection.BuildServiceProvider();

using var server = new BackgroundJobServer(
	new BackgroundJobServerOptions
	{
		Activator = new ScopedContainerJobActivator(serviceProvider)
	});

var delay = configuration
	.GetSection("SystemSettings")
	.GetValue<int>("DelayMinutes");

RecurringJob.AddOrUpdate<IFuelPriceService>(
	x => x.SavePrices(),
	Cron.MinuteInterval(delay));

BackgroundJob.Enqueue(() => Console.WriteLine("Hello from Hangfire!"));


Console.ReadKey();
