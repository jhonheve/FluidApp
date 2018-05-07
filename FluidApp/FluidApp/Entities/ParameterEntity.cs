

using System.Net.Http;

namespace FluidApp.Entities
{
    public class ParameterEntity
    {
        public string Name { get; set; }
        public HttpContent httpContent { get; set; }
    }
}
