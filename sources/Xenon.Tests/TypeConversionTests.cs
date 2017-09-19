using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class TypeConversionTests
    {
        [Test, SetCulture("en-us")]
        public async Task ShouldParseInt64()
        {
            const string data = "<data>123</data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToInt64(), Is.InstanceOf<Int64>());
                    Assert.That(document.ToInt64(), Is.EqualTo(123));
                });
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldParseInt64WithAdditionalWhiteCharacters()
        {
            const string data = "<data>\n 123\n </data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToInt64(), Is.InstanceOf<Int64>());
                    Assert.That(document.ToInt64(), Is.EqualTo(123));
                });
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldParseDouble()
        {
            const string data = "<data>123.45</data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToDouble(), Is.InstanceOf<Double>());
                    Assert.That(document.ToDouble(), Is.EqualTo(123.45d));
                });
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldParseDoubleWithAdditionalWhiteCharacters()
        {
            const string data = "<data>\n 123.45\n </data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToDouble(), Is.InstanceOf<Double>());
                    Assert.That(document.ToDouble(), Is.EqualTo(123.45d));
                });
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldParseDateTime()
        {
            const string data = "<data>2007-12-31</data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToDateTime(), Is.InstanceOf<DateTime>());
                    Assert.That(document.ToDateTime(), Is.EqualTo(new DateTime(2007, 12, 31)));
                });
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldParseDateTimeWithAdditionalWhiteCharacters()
        {
            const string data = "<data>\n 2007-12-31\n </data>";

            using (Stream source = ResourceFactory.Inline(data))
            {
                XenonReader reader = new XenonReader(source, "data");

                await reader.Process(document =>
                {
                    Assert.That(document.ToDateTime(), Is.InstanceOf<DateTime>());
                    Assert.That(document.ToDateTime(), Is.EqualTo(new DateTime(2007, 12, 31)));
                });
            }
        }
    }
}
