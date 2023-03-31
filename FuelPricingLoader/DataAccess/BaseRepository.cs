using System.Data;
using Dapper;
using FuelPricingLoader.DataAccess.Abstraction;
using Microsoft.Extensions.Logging;

namespace FuelPricingLoader.DataAccess;

public class BaseRepository
{
	private const int CommandTimeOut = 3000;
	private readonly IDataConnection _dataConnection;
	private readonly ILogger _logger;

	protected BaseRepository(
		IDataConnection dataConnection,
		ILogger logger)
	{
		_dataConnection = dataConnection;
		_logger = logger;
	}

	protected Task<int> ExecuteAsync(
		string procedureName,
		object? param = null)
	{
		return ExecuteAsync(
			procedureName,
			CommandType.StoredProcedure,
			param);
	}
	
	protected async Task<TEntity?> QueryFirstOrDefaultAsync<TEntity>(
			string procedureName,
			object? param = null)
		{
			var result = await _dataConnection.Connection.QueryAsync<TEntity>(
				procedureName,
				param,
				commandTimeout: CommandTimeOut,
				commandType: CommandType.StoredProcedure);

			return result.ToList().FirstOrDefault();
		}

	private async Task<int> ExecuteAsync(
		string sql,
		CommandType commandType,
		object? param = null)
	{
		_dataConnection.Begin();

		try
		{
			var result = await _dataConnection.Connection.ExecuteAsync(
				sql,
				param,
				_dataConnection.Transaction,
				CommandTimeOut,
				commandType);

			_dataConnection.Commit();

			return result;
		}
		catch (Exception ex)
		{
			_dataConnection.Rollback();
			_logger.LogCritical(
				ex,
				"Database level exception occured with SQL: {SQL}",
				sql);

			throw;
		}
	}
}