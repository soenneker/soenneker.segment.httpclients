using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Soenneker.Dtos.HttpClientOptions;
using Soenneker.Extensions.Configuration;
using Soenneker.Segment.HttpClients.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;

namespace Soenneker.Segment.HttpClients;

/// <inheritdoc cref="ISegmentOpenApiHttpClient" />
public sealed class SegmentOpenApiHttpClient : ISegmentOpenApiHttpClient
{
    private readonly IHttpClientCache _httpClientCache;
    private readonly IConfiguration _config;
    private readonly bool _ownsCachedClient;

    private const string _prodBaseUrl = "https://api.segmentapis.com";

    public SegmentOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config) : this(httpClientCache, config, true)
    {
    }

    internal SegmentOpenApiHttpClient(IHttpClientCache httpClientCache, IConfiguration config, bool ownsCachedClient)
    {
        _httpClientCache = httpClientCache;
        _config = config;
        _ownsCachedClient = ownsCachedClient;
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        return _httpClientCache.Get(nameof(SegmentOpenApiHttpClient), (config: _config, baseUrl: _config["Segment:ClientBaseUrl"] ?? _prodBaseUrl), static state =>
        {
            var apiKey = state.config.GetValueStrict<string>("Segment:ApiToken");
            string authHeaderName = state.config["Segment:AuthHeaderName"] ?? "Authorization";
            string authHeaderValueTemplate = state.config["Segment:AuthHeaderValueTemplate"] ?? "Bearer {token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            return new HttpClientOptions
            {
                BaseAddress = new Uri(state.baseUrl),
                DefaultRequestHeaders = new Dictionary<string, string>
                {
                    {authHeaderName, authHeaderValue},
                }
            };
        }, cancellationToken);
    }

    public void Dispose()
    {
        if (_ownsCachedClient)
            _httpClientCache.RemoveSync(nameof(SegmentOpenApiHttpClient));
    }

    public ValueTask DisposeAsync()
    {
        return _ownsCachedClient
            ? _httpClientCache.Remove(nameof(SegmentOpenApiHttpClient))
            : ValueTask.CompletedTask;
    }
}
