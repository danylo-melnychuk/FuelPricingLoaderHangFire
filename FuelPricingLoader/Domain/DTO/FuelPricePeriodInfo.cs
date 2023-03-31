using System.Text.Json.Serialization;

namespace FuelPricingLoader.Domain.DTO;

public class FuelPricePeriodInfo
{
	public DateTime Period { get; set; }
	
	[JsonPropertyName("duoarea")]
	public string DuoArea { get; set; } = default!;

	[JsonPropertyName("area-name")]
	public string AreaName { get; set; } = default!;

	public string Product { get; set; } = default!;

	[JsonPropertyName("product-name")]
	public string ProductName { get; set; } = default!;

	public string Series { get; set; } = default!;

	[JsonPropertyName("series-description")]
	public string SeriesDescription { get; set; } = default!;
	
	public decimal? Value { get; set; }

	public string Units { get; set; } = default!;
}