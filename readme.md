# OpenApi Generator

## Description

Generates Swager Open Api specification from Ocelot config.
Use when you need to create Gataway API specification from differen services according to the configiration in Ocelot.

## Usege

### Precondition

1. Ocelot config
2. Swager Open Api specifications on http

### Example

```
using OpenApiGenerator;

var pathToOcelotConfig = "ocelot.json";
var pathToDestSpecificatio = "swagger.json";

var specificationGenerator  = new OpenApiSpecificationGenerator();

await specificationGenerator.Generate(
    pathToOcelotConfig, 
    pathToDestSpecificatio);
```

### Example of source ocelot.json

```
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/Cafes",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/api/Cafes",
      "UpstreamHttpMethod": [ "Get" ],
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5102
        }
      ]
    },
    {
      "DownstreamPathTemplate": "/api/Menu",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/api/Menu",
      "UpstreamHttpMethod": [ "Get", "Post" ],
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5104
        }
      ]
    }
  ]
}
```

## Limitations

A lot) 
Lib is for mostoly for my internal use. Will be finished when there will be a time for it.
