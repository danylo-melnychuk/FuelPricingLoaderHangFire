using System.Reflection;
using DbUp;
using FuelPricingLoader.Configuration;
using Microsoft.Extensions.Configuration;

namespace FuelPricingLoader.DataAccess;

public static class DbUpConfiguration
{
	private const string CaseSensitiveCollation = "SQL_Latin1_General_CP1_CS_AS";
	
	public static void MigrateDatabase()
	{
		var configuration = SystemConfiguration.BuildConfiguration();
		var connectionString = configuration.GetConnectionString("DbConnection");
		
		EnsureDatabase.For.SqlDatabase(connectionString, CaseSensitiveCollation);

		var deployChangesEngine = DeployChanges.To
			.SqlDatabase(connectionString)
			.WithScriptsEmbeddedInAssembly(
				Assembly.GetExecutingAssembly(),
				folderName => folderName.Contains("ChangeScripts"))
			.WithTransaction()
			.LogToConsole()
			.Build();

		if (!deployChangesEngine.IsUpgradeRequired())
		{
			return;
		}

		var result = deployChangesEngine.PerformUpgrade();

		if (!result.Successful)
		{
			throw new Exception("Applying DB Scripts failed");
		}
	}
}