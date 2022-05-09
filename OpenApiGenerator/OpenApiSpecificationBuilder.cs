using Newtonsoft.Json.Linq;
using OpenApiGenerator.Interfaces;
using OpenApiGenerator.Models;

namespace OpenApiGenerator
{
    internal class OpenApiSpecificationBuilder
    {
        private ISpecificationReader specificationReader;

        public OpenApiSpecificationBuilder(
            ISpecificationReader specificationReader)
        {
            this.specificationReader = specificationReader;
        }

        public async Task<JObject> BuildSpecification(
            IEnumerable<Route> routes)
        {
            var paths = await BuildPaths(routes);
            var schemas = await BuildSchemas(routes);

            return BuildSpecification(paths, schemas);
        }

        private async Task<JObject> BuildPaths(
            IEnumerable<Route> routes)
        {
            var paths = new JObject();

            foreach (var route in routes)
            {
                var specification = await specificationReader
                    .Read(route);

                var sourcePaths = JObject
                    .Parse(specification)["paths"];

                if(sourcePaths == null)
                {
                    continue;
                }

                var a = sourcePaths.Children<JProperty>().Select(a => a.Name);

                var b = a.Contains(route.DownStreamPath);

                var sourceDownStreamPath = sourcePaths[route.DownStreamPath];

                if (sourceDownStreamPath == null)
                {
                    continue;
                }

                var sourceDownStreamPathMethods = sourceDownStreamPath
                    .Children<JProperty>();

                var result = sourceDownStreamPathMethods
                    .Where(jp => route.Metods.Contains(jp.Name.ToLower(), StringComparer.InvariantCultureIgnoreCase))
                    .ToList();

                paths.Add(route.DownStreamPath, new JObject(result));
            }

            return paths;
        }

        private async Task<JObject> BuildSchemas(
            IEnumerable<Route> routes)
        {
            var schemas = new JObject();

            foreach (var route in routes)
            {
                var specification = await specificationReader
                    .Read(route);

                var sourceComponents = JObject
                    .Parse(specification)["components"];

                if (sourceComponents == null)
                {
                    throw new InvalidDataException();
                }

                var sourceSchema = sourceComponents["schemas"];

                if (sourceSchema == null)
                {
                    throw new InvalidDataException();
                }

                var sourceSchemas = sourceSchema
                    .Children<JProperty>();

                foreach (var schema in sourceSchemas)
                {
                    if (!schemas.ContainsKey(schema.Name))
                    {
                        schemas.Add(schema.Name, schema.Value);
                    }
                }
            }

            return schemas;
        }

        private JObject BuildSpecification(
            JObject paths,
            JObject schemas)
        {
            var info = new JObject
            {
                { "title", "Gataway.Api" },
                { "version", "1.0" }
            };

            var specification = new JObject
            {
                { "openapi", "3.0.1" },
                { "info",  info },
                { "paths",  paths },
                { 
                    "components", 
                    new JObject 
                    {
                        { "schemas",  schemas }
                    }
                }
            };

            return specification;
        }
    }
}
