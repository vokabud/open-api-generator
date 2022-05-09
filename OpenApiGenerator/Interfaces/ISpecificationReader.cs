using OpenApiGenerator.Models;

namespace OpenApiGenerator.Interfaces
{
    internal interface ISpecificationReader
    {
        public Task<string> Read(Route route);
    }
}
