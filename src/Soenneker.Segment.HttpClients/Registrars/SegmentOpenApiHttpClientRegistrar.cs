using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Segment.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.Segment.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class SegmentOpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="SegmentOpenApiHttpClient"/> as a singleton service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddSegmentOpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<ISegmentOpenApiHttpClient>(provider =>
                    new SegmentOpenApiHttpClient(
                        provider.GetRequiredService<IHttpClientCache>(),
                        provider.GetRequiredService<IConfiguration>(),
                        true));

        return services;
    }

    /// <summary>
    /// Adds <see cref="SegmentOpenApiHttpClient"/> as a scoped service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddSegmentOpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<ISegmentOpenApiHttpClient>(provider =>
                    new SegmentOpenApiHttpClient(
                        provider.GetRequiredService<IHttpClientCache>(),
                        provider.GetRequiredService<IConfiguration>(),
                        false));

        return services;
    }
}
