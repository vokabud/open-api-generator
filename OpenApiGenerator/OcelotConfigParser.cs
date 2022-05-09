using Newtonsoft.Json.Linq;
using OpenApiGenerator.Models;

namespace OpenApiGenerator
{
    internal static class OcelotConfigParser
    {
        private const string Routes = "Routes";
        private const string UpStreamPath = "UpstreamPathTemplate";
        private const string DownStreamPath = "DownstreamPathTemplate";
        private const string Sockets = "DownstreamHostAndPorts";
        private const string Host = "Host";
        private const string Port = "Port";
        private const string Methods = "UpstreamHttpMethod";

        public static IEnumerable<Route> GetRouts(
            string configuration)
        {
            var routs = JObject.Parse(configuration)[Routes] as JArray;

            if(routs == null)
            {
                throw new InvalidDataException("Ocelot config don't have Routes configuration");
            }

            foreach (var route in routs)
            {
                var upStreamPath = route[UpStreamPath]?.ToString();
                var downStreamPath = route[DownStreamPath]?.ToString();
                var host = route[Sockets]?[0]?[Host]?.ToString();
                var port = route[Sockets]?[0]?[Port]?.ToString();
                var methods = route[Methods] as JArray;

                yield return new Route
                {
                    DownStreamPath = downStreamPath ?? string.Empty,
                    UpStreamPath = upStreamPath ?? string.Empty,
                    Host = host ?? string.Empty,
                    Port = port ?? string.Empty,
                    Metods = methods != null
                        ? methods.Select(m => m.ToString()).ToArray()
                        : Enumerable.Empty<string>()
                };
            }
        }
    }
}
