namespace FuelPricingLoader.Domain.Entities;

public class JobExecutionLog
{
	public DateTimeOffset LastExecutionTime { get; set; }

	public string JobId { get; set; } = default!;
}