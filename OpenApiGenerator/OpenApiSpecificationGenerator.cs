using Newtonsoft.Json;

namespace OpenApiGenerator
{
    public class OpenApiSpecificationGenerator
    {
        public async Task Generate(
            string sourceOcelotConfig,
            string destOpenApiSpecification)
        {
            var ocelotConfig = File.ReadAllText(sourceOcelotConfig);

            var routs = OcelotConfigParser.GetRouts(ocelotConfig);

            var reader = new HttpSpecificationReader(
                httpClient: new HttpClient(),
                schema: "http");

            var builder = new OpenApiSpecificationBuilder(reader);
            var spec = await builder.BuildSpecification(routs) ;

            using var file = File.CreateText(destOpenApiSpecification);
            using var writer = new JsonTextWriter(file);

            spec.WriteTo(writer);
        }
    }
}
