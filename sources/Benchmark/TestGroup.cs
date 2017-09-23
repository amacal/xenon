namespace Benchmark
{
    public interface TestGroup
    {
        TestSupport Support { get; }

        void InvokeXenonStatic(TestInvoke context);

        void InvokeXenonDynamic(TestInvoke context);

        void InvokeXmlReader(TestInvoke context);

        void InvokeXmlLinq(TestInvoke context);
    }
}
