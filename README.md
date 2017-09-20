# xenon
The projects aims to be xml deserializer working somewhere between SAX and DOM model. It benefits from multiple CPUs and fast SDD. It is multiple times faster then built-in .net XmlReader.

# installation
Just include latest NuGet package.

# usage
`````csharp
using (FileStream stream = File.OpenRead(path))
{
    HashSet<string> found = new HashSet<string>();
    XenonReader reader = new XenonReader(source, "book");

    await reader.Process(document =>
    {
        found.Add(document.Get("id").ToString());
    });

    Console.WriteLine($"Found {found.Count} unique books.");
}
`````

# benchmark
- Intel(R) Core(TM)2 Quad CPUQ9300 @ 2.50GHz, 6GB RAM, TOSHIBA MD04ACA50D (max 200MB/s)
- Source: [plwiki-20170801-pages-meta-current4.xml](http://ftp.acc.umu.se/mirror/wikimedia.org/dumps/plwiki/20170801/plwiki-20170801-pages-meta-current4.xml.bz2) (about 5GB)

    Test Case | Xenon Static | Xenon Dynamic | Xml Reader
    --- | --- | --- | --- |
    just-seek | 31.6s, 22.1s CPU | 31.6s, 21.6s CPU | 60.3s, 60.2s CPU
    text-length | 31.6s, 41.4s CPU | 31.6s, 42.1s CPU | 61.8s, 61.1s CPU

- Intel(R) Core(TM) i7-4770HQ CPU @ 2.20GHz, 16GB RAM, APPLE SSD SM0512G (max 1500MB/s)
- Source: [plwiki-20170901-pages-meta-history2.xml](http://ftp.acc.umu.se/mirror/wikimedia.org/dumps/plwiki/20170901/plwiki-20170901-pages-meta-history2.xml.bz2) (about 100GB)

    Test Case | Xenon Static | Xenon Dynamic | Xml Reader
    --- | --- | --- | ---
    just-seek | 76.7s, 183.5s CPU | 75.1s, 181.5s CPU | 570.0s, 574.8s CPU
    text-length | 94.2s, 641.9s CPU | 94.1s, 638.0s CPU | 595.3s, 594.5s CPU
