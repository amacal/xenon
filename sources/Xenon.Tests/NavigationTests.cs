using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class NavigationTests
    {
        [Test]
        public async Task ShouldDetectExistingProperty()
        {
            const string data = @"<data><first></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.Has("first"), Is.True);
                });
            }
        }

        [Test]
        public async Task ShouldDetectMissingProperty()
        {
            const string data = @"<data><first></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.Has("second"), Is.False);
                });
            }
        }

        [Test]
        public async Task ShouldHandleMissingProperty()
        {
            const string data = @"<data><first></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.Get("second"), Is.Null);
                });
            }
        }

        [Test]
        public async Task ShouldAccessNestedElement()
        {
            const string data = @"<data><first><second>abc</second></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.Get("first"), Is.Not.Null);
                    Assert.That(document.Get("first").Get("second"), Is.Not.Null);
                    Assert.That(document.Get("first").Get("second").ToString(), Is.EqualTo("abc"));
                });
            }
        }

        [Test]
        public async Task ShouldEnumerateExistingPropertyOnce()
        {
            const string data = @"<data><first></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                int counter = 0;
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("first"))
                    {
                        counter++;
                    }
                });

                Assert.That(counter, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task ShouldEnumerateMissingProperty()
        {
            const string data = @"<data><first></first></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("second"))
                    {
                        Assert.Fail();
                    }
                });
            }
        }

        [Test]
        public async Task ShouldEnumerateExistingPropertyTwice()
        {
            const string data = @"<data><node/><node/></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                int counter = 0;
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("node"))
                    {
                        counter++;
                    }
                });

                Assert.That(counter, Is.EqualTo(2));
            }
        }

        [Test]
        public async Task ShouldEnumerateMixedProperty()
        {
            const string data = "<data node=\"abc\"><node/><node/></data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                int counter = 0;
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("node"))
                    {
                        counter++;
                    }
                });

                Assert.That(counter, Is.EqualTo(3));
            }
        }
    }
}
