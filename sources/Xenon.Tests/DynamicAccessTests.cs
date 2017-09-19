using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class DynamicAccessTests
    {
        [Test]
        public async Task ShouldFindAllIdentifiers()
        {
            HashSet<string> expected = new HashSet<string>
            {
                "bk101", "bk102", "bk103", "bk104", "bk105", "bk106",
                "bk107", "bk108", "bk109", "bk110", "bk111", "bk112",
            };

            using (Stream source = ResourceFactory.Open("Books"))
            {
                HashSet<string> found = new HashSet<string>();
                XenonReader reader = new XenonReader(source, "book");

                await reader.Process(document =>
                {
                    found.Add(document.GetDynamic().id.ToString());
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test]
        public async Task ShouldFindAllTitles()
        {
            Dictionary<string, string> expected = new Dictionary<string, string>
            {
                ["bk101"] = "XML Developer's Guide",
                ["bk102"] = "Midnight Rain",
                ["bk103"] = "Maeve Ascendant",
                ["bk104"] = "Oberon's Legacy",
                ["bk105"] = "The Sundered Grail",
                ["bk106"] = "Lover Birds",
                ["bk107"] = "Splish Splash",
                ["bk108"] = "Creepy Crawlies",
                ["bk109"] = "Paradox Lost",
                ["bk110"] = "Microsoft .NET: The Programming Bible",
                ["bk111"] = "MSXML3: A Comprehensive Guide",
                ["bk112"] = "Visual Studio 7: A Comprehensive Guide"
            };

            using (Stream source = ResourceFactory.Open("Books"))
            {
                XenonReader reader = new XenonReader(source, "book");
                Dictionary<string, string> found = new Dictionary<string, string>();

                await reader.Process(document =>
                {
                    dynamic data = document.GetDynamic();

                    found.Add(data.id.ToString(), data.title.ToString());
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

    }
}
