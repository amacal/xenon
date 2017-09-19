using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class XenonReaderTests
    {
        [Test]
        public async Task ShouldFindOneCatalog()
        {
            using (Stream source = ResourceFactory.Open("Books"))
            {
                int counter = 0;
                XenonReader reader = new XenonReader(source, "catalog");

                await reader.Process(document =>
                {
                    counter++;
                });

                Assert.That(counter, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task ShouldFindTwelveBooks()
        {
            using (Stream source = ResourceFactory.Open("Books"))
            {
                int counter = 0;
                XenonReader reader = new XenonReader(source, "book");

                await reader.Process(document =>
                {
                    counter++;
                });

                Assert.That(counter, Is.EqualTo(12));
            }
        }
    }
}
