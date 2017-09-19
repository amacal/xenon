using System.IO;
using System.Text;

namespace Xenon.Tests
{
    public static class ResourceFactory
    {
        public static Stream Open(string name)
        {
            return typeof(ResourceFactory).Assembly.GetManifestResourceStream($"Xenon.Tests.Resources.{name}.xml");
        }

        public static Stream Inline(string data)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(data));
        }
    }
}
