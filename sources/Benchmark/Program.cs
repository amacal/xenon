using Benchmark.Groups;

namespace Benchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            TestRunner runner = new TestRunner
            {
                new JustSeekGroup(),
                new TextLengthGroup()
            };

            TestReporter reporter = new TestReporter();
            TestInvoke invoke = new TestInvoke(args[0], reporter);

            runner.Execute(invoke);
            reporter.Summarize();
        }
    }
}
