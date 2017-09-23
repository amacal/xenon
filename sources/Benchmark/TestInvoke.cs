using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Xenon;

namespace Benchmark
{
    public class TestInvoke
    {
        private readonly string path;
        private readonly TestReporter reporter;

        public TestInvoke(string path, TestReporter reporter)
        {
            this.path = path;
            this.reporter = reporter;
        }

        public void Complete(object value)
        {
            reporter.Complete(value);
        }

        public void BenchmarkStatic(string scenario, string node, Action<XenonDocument> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            {
                XenonReader reader = new XenonReader(stream, node, 4 * 1024 * 1024);

                reporter.Benchmarking(scenario, "xenon-static");
                reader.Process(callback).Wait();
                reporter.Complete(scenario, "xenon-static", watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
            }
        }

        public void BenchmarkDynamic(string scenario, string node, Action<dynamic> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            {
                XenonReader reader = new XenonReader(stream, node, 4 * 1024 * 1024);

                reporter.Benchmarking(scenario, "xenon-dynamic");
                reader.Process(callback).Wait();
                reporter.Complete(scenario, "xenon-dynamic", watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
            }
        }

        public void BenchmarkReader(string scenario, string node, Action<XmlTextReader> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reporter.Benchmarking(scenario, "xml-reader");

                while (reader.ReadToFollowing(node))
                {
                    callback.Invoke(reader);
                }
            }

            reporter.Complete(scenario, "xml-reader", watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
        }

        public void BenchmarkLinq(string scenario, string node, Action<XmlNode> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            using (XmlTextReader reader = new XmlTextReader(stream))
            {
                reporter.Benchmarking(scenario, "xml-linq");

                while (reader.Read())
                {
                    while (reader.ReadToFollowing(node))
                    {
                        using (XmlReader tree = reader.ReadSubtree())
                        {
                            callback.Invoke(new XmlDocument().ReadNode(tree));
                        }
                    }
                }
            }

            reporter.Complete(scenario, "xml-linq", watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
        }

    }
}
