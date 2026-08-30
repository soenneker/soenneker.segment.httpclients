[![](https://img.shields.io/nuget/v/soenneker.segment.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.segment.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.segment.httpclients/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.segment.httpclients/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.segment.httpclients.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.segment.httpclients/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.segment.httpclients/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.segment.httpclients/actions/workflows/codeql.yml)

# Soenneker.Segment.HttpClients

A cached `HttpClient` provider for Segment's Public API, including configurable token authentication.

## Installation

```bash
dotnet add package Soenneker.Segment.HttpClients
```

## Configuration

```json
{
  "Segment": {
    "ApiToken": "your-segment-token"
  }
}
```

The default base URL is `https://api.segmentapis.com`, and the default authentication header is `Authorization: Bearer {token}`.

Optional overrides:

```json
{
  "Segment": {
    "ClientBaseUrl": "https://api.segmentapis.com",
    "ApiToken": "your-segment-token",
    "AuthHeaderName": "Authorization",
    "AuthHeaderValueTemplate": "Bearer {token}"
  }
}
```

`ClientBaseUrl` must be absolute. The literal `{token}` in `AuthHeaderValueTemplate` is replaced with `ApiToken`; keep it in the template unless the configured header intentionally does not carry the token.

## Registration

```csharp
using Soenneker.Segment.HttpClients.Registrars;

services.AddSegmentOpenApiHttpClientAsSingleton();
```

Scoped registration is available for scoped consumers:

```csharp
services.AddSegmentOpenApiHttpClientAsScoped();
```

Both registrations retain the singleton cached transport. Disposing a scoped wrapper does not remove the shared `HttpClient`.

## Usage

```csharp
using Soenneker.Segment.HttpClients.Abstract;

public sealed class SegmentWorkspaceClient(ISegmentOpenApiHttpClient clientProvider)
{
    public async Task<string> GetWorkspaces(CancellationToken cancellationToken)
    {
        HttpClient client = await clientProvider.Get(cancellationToken);
        return await client.GetStringAsync("/workspaces", cancellationToken);
    }
}
```

The client is created lazily and then reused. Authentication configuration is applied when that cached client is first created; remove/recreate the owning singleton registration if credentials or endpoints change at runtime.
