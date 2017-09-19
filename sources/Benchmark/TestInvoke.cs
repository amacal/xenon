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

        public TestInvoke(string path)
        {
            this.path = path;
        }

        public void BenchmarkXenon(string scenario, string mode, Func<XenonReader, Task> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            double started = Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;

            using (FileStream stream = File.OpenRead(path))
            {
                Console.WriteLine($"Benchmarking {scenario} using {mode}...");
                XenonReader reader = new XenonReader(stream, "revision", 4 * 1024 * 1024);

                callback.Invoke(reader).Wait();
            }

            Console.WriteLine($"Completed in {watch.Elapsed.TotalSeconds:F1} seconds.");
            Console.WriteLine($"Completed using {(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds - started):F1} CPU seconds.");
            Console.WriteLine();
        }

        public void BenchmarkReader(string scenario, Action<XmlTextReader> callback)
        {
            Stopwatch watch = Stopwatch.StartNew();
            double started = Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;

            using (FileStream stream = File.OpenRead(path))
            using (XmlTextReader text = new XmlTextReader(stream))
            {
                Console.WriteLine($"Benchmarking {scenario} using reader...");
                callback.Invoke(text);
            }

            Console.WriteLine($"Completed in {watch.Elapsed.TotalSeconds:F1} seconds.");
            Console.WriteLine($"Completed using {(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds - started):F1} CPU seconds.");
            Console.WriteLine();
        }
    }
}
