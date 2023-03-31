namespace FuelPricingLoader.Domain.DTO;

public class FuelPriceApiResponse
{
	public long Total { get; set; }

	public string DateFormat { get; set; } = default!;

	public string Frequency { get; set; } = default!;

	public List<FuelPricePeriodInfo> Data { get; set; } = new ();
}