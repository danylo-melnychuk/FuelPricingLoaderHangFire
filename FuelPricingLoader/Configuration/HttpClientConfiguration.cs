using Polly;
using Polly.Extensions.Http;

namespace FuelPricingLoader.Configuration;

public static class HttpClientConfiguration
{
	public static IAsyncPolicy<HttpResponseMessage> RetryPolicy => 
		HttpPolicyExtensions
			.HandleTransientHttpError()
			.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
			.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			.WaitAndRetryAsync(
				3,
				retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}