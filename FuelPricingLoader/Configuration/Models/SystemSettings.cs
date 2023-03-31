namespace FuelPricingLoader.Configuration.Models;

public class SystemSettings
{
	public int DelayMinutes { get; set; }
	
	public int RetentionPeriodDays { get; set; }

	public string FetchDataUrl { get; set; } = default!;
}