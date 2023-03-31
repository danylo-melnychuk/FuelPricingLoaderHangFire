using System.Data;
using System.Data.SqlClient;
using FuelPricingLoader.DataAccess.Abstraction;
using Microsoft.Extensions.Configuration;

namespace FuelPricingLoader.DataAccess.Implementation;

public class DataConnection : IDataConnection, IDisposable
{
	private readonly IDbConnection _dbConnection;

	public DataConnection(IConfiguration configuration)
	{
		_dbConnection = CreateConnection(configuration.GetConnectionString("DbConnection"));
	}

	public IDbConnection Connection
	{
		get
		{
			if (_dbConnection.State != ConnectionState.Open && _dbConnection.State != ConnectionState.Connecting)
			{
				_dbConnection.Open();
			}

			return _dbConnection;
		}
	}

	public IDbTransaction? Transaction { get; private set; }

	public void Dispose()
	{
		Transaction?.Dispose();
		Transaction = null;

		if (_dbConnection.State != ConnectionState.Closed)
		{
			_dbConnection.Close();
		}

		GC.SuppressFinalize(this);
	}

	public void Begin()
	{
		Transaction = _dbConnection.BeginTransaction();
	}

	public void Commit()
	{
		Transaction?.Commit();
	}

	public void Rollback()
	{
		Transaction?.Rollback();
	}

	private static IDbConnection CreateConnection(string connectionString)
	{
		var connection = new SqlConnection(connectionString);
		connection.Open();
		
		return connection;
	}
}