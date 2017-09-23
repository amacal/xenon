using System;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark
{
    public class TestReporter
    {
        private readonly Dictionary<Item, Result> data;

        public TestReporter()
        {
            data = new Dictionary<Item, Result>();
        }

        public void Benchmarking(string scenario, string approach)
        {
            Console.WriteLine($"Benchmarking {scenario} using {approach}...");
        }

        public void Complete(object value)
        {
            Console.WriteLine($"Completed with result {value}");
            Console.WriteLine();
        }

        public void Complete(string scenario, string approach, TimeSpan elapsed, TimeSpan consumed)
        {
            data.Add(
                new Item { Scenario = scenario, Approach = approach },
                new Result { Elapsed = elapsed, Consumed = consumed });

            Console.WriteLine($"Completed in {elapsed.TotalSeconds:F1} seconds.");
            Console.WriteLine($"Completed using {consumed.TotalSeconds:F1} CPU seconds.");
        }

        public void Summarize()
        {
            Console.WriteLine("    Test Case | Xenon Static | Xenon Dynamic | Xml Reader");
            Console.WriteLine("    --- | --- | --- | --- |");

            foreach (var item in data.Keys.GroupBy(x => x.Scenario).ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Approach)))
            {
                Console.Write($"    {item.Key} ");
                Console.Write($"| {data[item.Value["xenon-static"]]} ");
                Console.Write($"| {data[item.Value["xenon-dynamic"]]} ");
                Console.Write($"| {data[item.Value["xml-reader"]]} ");
                Console.WriteLine();
            }
        }

        private struct Item
        {
            public string Scenario;
            public string Approach;
        }

        private class Result
        {
            public TimeSpan Elapsed;
            public TimeSpan Consumed;

            public override string ToString()
            {
                return $"{Elapsed.TotalSeconds:F1}s, {Consumed.TotalSeconds:F1}s CPU";
            }
        }
    }
}
