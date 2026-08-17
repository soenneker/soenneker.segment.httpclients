using Soenneker.Segment.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Segment.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SegmentOpenApiHttpClientTests : HostedUnitTest
{
    private readonly ISegmentOpenApiHttpClient _httpclient;

    public SegmentOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<ISegmentOpenApiHttpClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
