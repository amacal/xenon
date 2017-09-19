namespace Benchmark
{
    public interface TestGroup
    {
        void InvokeXenonStatic(TestInvoke context);

        void InvokeXenonDynamic(TestInvoke context);

        void InvokeXmlReader(TestInvoke context);

        void InvokeXmlDocument(TestInvoke context);
    }
}
