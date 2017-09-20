using Xenon;

namespace Benchmark.Groups
{
    public class JustSeekGroup : TestGroup
    {
        public void InvokeXenonStatic(TestInvoke context)
        {
            context.BenchmarkXenon("just-seek", "xenon-static", "revision", reader =>
            {
                return reader.Process(document =>
                {
                });
            });
        }

        public void InvokeXenonDynamic(TestInvoke context)
        {
            context.BenchmarkXenon("just-seek", "xenon-dynamic", "revision", reader =>
            {
                return reader.Process(document =>
                {
                });
            });
        }

        public void InvokeXmlReader(TestInvoke context)
        {
            context.BenchmarkReader("just-seek", reader =>
            {
                while (reader.Read())
                {
                }
            });
        }

        public void InvokeXmlDocument(TestInvoke context)
        {
            throw new System.NotImplementedException();
        }
    }
}
