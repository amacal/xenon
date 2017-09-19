using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
    public class StaticAccessTests
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
                    found.Add(document.Get("id").ToString());
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
                    found.Add(document.Get("id").ToString(), document.Get("title").ToString());
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldFindAllPrices()
        {
            Dictionary<string, double> expected = new Dictionary<string, double>
            {
                ["bk101"] = 44.95,
                ["bk102"] = 5.95,
                ["bk103"] = 5.95,
                ["bk104"] = 5.95,
                ["bk105"] = 5.95,
                ["bk106"] = 4.95,
                ["bk107"] = 4.95,
                ["bk108"] = 4.95,
                ["bk109"] = 6.95,
                ["bk110"] = 36.95,
                ["bk111"] = 36.95,
                ["bk112"] = 49.95
            };

            using (Stream source = ResourceFactory.Open("Books"))
            {
                XenonReader reader = new XenonReader(source, "book");
                Dictionary<string, double?> found = new Dictionary<string, double?>();

                await reader.Process(document =>
                {
                    found.Add(document.Get("id").ToString(), document.Get("price").ToDouble());
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldFindAllDates()
        {
            Dictionary<string, DateTime> expected = new Dictionary<string, DateTime>
            {
                ["bk101"] = DateTime.Parse("2000-10-01"),
                ["bk102"] = DateTime.Parse("2000-12-16"),
                ["bk103"] = DateTime.Parse("2000-11-17"),
                ["bk104"] = DateTime.Parse("2001-03-10"),
                ["bk105"] = DateTime.Parse("2001-09-10"),
                ["bk106"] = DateTime.Parse("2000-09-02"),
                ["bk107"] = DateTime.Parse("2000-11-02"),
                ["bk108"] = DateTime.Parse("2000-12-06"),
                ["bk109"] = DateTime.Parse("2000-11-02"),
                ["bk110"] = DateTime.Parse("2000-12-09"),
                ["bk111"] = DateTime.Parse("2000-12-01"),
                ["bk112"] = DateTime.Parse("2001-04-16")
            };

            using (Stream source = ResourceFactory.Open("Books"))
            {
                XenonReader reader = new XenonReader(source, "book");
                Dictionary<string, DateTime?> found = new Dictionary<string, DateTime?>();

                await reader.Process(document =>
                {
                    found.Add(document.Get("id").ToString(), document.Get("publish_date").ToDateTime());
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test]
        public async Task ShouldEnumerateAllBooks()
        {
            HashSet<string> expected = new HashSet<string>
            {
                "bk101", "bk102", "bk103", "bk104", "bk105", "bk106",
                "bk107", "bk108", "bk109", "bk110", "bk111", "bk112",
            };

            using (Stream source = ResourceFactory.Open("Books"))
            {
                HashSet<string> found = new HashSet<string>();
                XenonReader reader = new XenonReader(source, "catalog");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("book"))
                    {
                        found.Add(node.Get("id").ToString());
                    }
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test, SetCulture("en-us")]
        public async Task ShouldEnumerateAllRevisionsInAllPages()
        {
            HashSet<long> expected = new HashSet<long>
            {
                607051494, 607051522, 607082459, 609806417, 610782955, 615708849,
                607051549, 607053970, 607057294, 607057676, 607064502, 607064697,
                607065037, 616919844, 645443958, 645444002, 653021729, 653021968,
                653022435, 653783525, 674703256, 674719566, 675614952, 679157456,
                682724788, 682952438
            };

            using (Stream source = ResourceFactory.Open("History"))
            {
                HashSet<long?> found = new HashSet<long?>();
                XenonReader reader = new XenonReader(source, "page");

                await reader.Process(document =>
                {
                    foreach (XenonNode node in document.GetEnumerable("revision"))
                    {
                        found.Add(node.Get("id").ToInt64());
                    }
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }

        [Test]
        public async Task ShouldFindAllContributors()
        {
            HashSet<string> expected = new HashSet<string>
            {
                "Darkreason", "Invertzoo", "Auric", "Mx. Granger",
                "OccultZone", "Yserbius", "AnomieBOT"
            };

            using (Stream source = ResourceFactory.Open("History"))
            {
                HashSet<string> found = new HashSet<string>();
                XenonReader reader = new XenonReader(source, "contributor");

                await reader.Process(document =>
                {
                    if (document.Has("username"))
                    {
                        found.Add(document.Get("username").ToString());
                    }
                });

                Assert.That(found, Is.EquivalentTo(expected));
            }
        }
    }
}
