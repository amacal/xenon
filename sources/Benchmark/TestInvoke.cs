using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

        public void BenchmarkXenon(string scenario, string mode, string node, Func<XenonReader, Task> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            {
                XenonReader reader = new XenonReader(stream, "revision", 4 * 1024 * 1024);

                reporter.Benchmarking(scenario, mode);
                callback.Invoke(reader).Wait();
                reporter.Completed(scenario, mode, watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
            }
        }

        public void BenchmarkReader(string scenario, Action<XmlTextReader> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan started = Process.GetCurrentProcess().TotalProcessorTime;

            using (FileStream stream = File.OpenRead(path))
            using (XmlTextReader text = new XmlTextReader(stream))
            {
                reporter.Benchmarking(scenario, "xml-reader");
                callback.Invoke(text);
            }

            reporter.Completed(scenario, "xml-reader", watch.Elapsed, Process.GetCurrentProcess().TotalProcessorTime - started);
        }
    }
}
