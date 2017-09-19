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

            runner.Execute(new TestInvoke(args[0]));
        }
    }
}
