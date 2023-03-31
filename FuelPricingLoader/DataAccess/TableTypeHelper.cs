using System.ComponentModel;
using System.Data;
using FuelPricingLoader.Domain.Entities;

namespace FuelPricingLoader.DataAccess;

public static class TableTypeHelper
{
	public static DataTable CreateFuelPriceTable(
		IEnumerable<FuelPrice> price) => 
		price.ToDataTable("[dbo].[FuelPrice]");

	private static DataTable ToDataTable<T>(this IEnumerable<T> list, string name)
	{
		var props = TypeDescriptor.GetProperties(typeof(T));
		var table = new DataTable(name);

		for (var i = 0; i < props.Count; i++)
		{
			var prop = props[i];
			table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
		}

		var values = new object[props.Count];

		foreach (var item in list)
		{
			for (var i = 0; i < values.Length; i++)
			{
				values[i] = props[i].GetValue(item) ?? DBNull.Value;
			}
			
			table.Rows.Add(values);
		}

		return table;
	}
}