using System.Threading;

namespace Benchmark.Groups
{
    public class JustSeekGroup : TestGroup
    {
        public TestSupport Support
        {
            get { return TestSupport.Static | TestSupport.Dynamic | TestSupport.Reader | TestSupport.Linq; }
        }

        public void InvokeXenonStatic(TestInvoke context)
        {
            long count = 0;

            context.BenchmarkStatic("just-seek", "revision", document =>
            {
                Interlocked.Add(ref count, 1);
            });

            context.Complete(count);
        }

        public void InvokeXenonDynamic(TestInvoke context)
        {
            long count = 0;

            context.BenchmarkDynamic("just-seek", "revision", reader =>
            {
                Interlocked.Add(ref count, 1);
            });

            context.Complete(count);
        }

        public void InvokeXmlReader(TestInvoke context)
        {
            long count = 0;

            context.BenchmarkReader("just-seek", "revision", reader =>
            {
                Interlocked.Add(ref count, 1);
            });

            context.Complete(count);
        }

        public void InvokeXmlLinq(TestInvoke context)
        {
            long count = 0;

            context.BenchmarkLinq("just-seek", "revision", node =>
            {
                Interlocked.Add(ref count, 1);
            });

            context.Complete(count);
        }
    }
}
