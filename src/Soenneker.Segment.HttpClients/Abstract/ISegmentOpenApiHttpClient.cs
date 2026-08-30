using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Segment.HttpClients.Abstract;

/// <summary>
/// Provides a cached HTTP client configured for Segment's Public API.
/// </summary>
public interface ISegmentOpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the shared HTTP client with its configured authentication header.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The cached HTTP client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
