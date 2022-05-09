using OpenApiGenerator.Interfaces;
using OpenApiGenerator.Models;

namespace OpenApiGenerator
{
    internal class HttpSpecificationReader : ISpecificationReader
    {
        private const string SpecificationPath = "/swagger/v1/swagger.json";

        private readonly HttpClient httpClient;
        private readonly string schema;

        public HttpSpecificationReader(
            HttpClient httpClient,
            string schema)
        {
            this.httpClient = httpClient;
            this.schema = schema;
        }

        public async Task<string> Read(Route route)
        {
            var response = await httpClient
                .GetAsync($"{schema}://{route.Host}:{route.Port}{SpecificationPath}");

            var contents = await response
                .Content
                .ReadAsStringAsync();

            return contents;
        }
    }
}
