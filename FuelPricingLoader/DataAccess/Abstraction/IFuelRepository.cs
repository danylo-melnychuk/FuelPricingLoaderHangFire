using FuelPricingLoader.Domain.Entities;

namespace FuelPricingLoader.DataAccess.Abstraction;

public interface IFuelRepository
{
	Task SaveFuelPrice(IEnumerable<FuelPrice> prices);
}