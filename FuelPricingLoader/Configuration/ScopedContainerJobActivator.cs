using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace FuelPricingLoader.Configuration;

public class ScopedContainerJobActivator : JobActivator
{
	private readonly IServiceProvider _serviceProvider;

    public ScopedContainerJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    
    public override object? ActivateJob(Type type) => 
		_serviceProvider.GetRequiredService(type);
}