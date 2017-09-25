using System.Threading;
using System.Xml;

namespace Benchmark.Groups
{
    public class TextLengthGroup : TestGroup
    {
        public TestSupport Support
        {
            get { return TestSupport.Static | TestSupport.Dynamic | TestSupport.Reader; }
        }

        public void InvokeXenonStatic(TestInvoke context)
        {
            long length = 0;

            context.BenchmarkStatic("text-length", "revision", document =>
            {
                Interlocked.Add(ref length, document.Get("text").ToText().GetLength());
            });

            context.Complete(length);
        }

        public void InvokeXenonDynamic(TestInvoke context)
        {
            long length = 0;

            context.BenchmarkDynamic("text-length", "revision", document =>
            {
                Interlocked.Add(ref length, document.GetDynamic().text.ToText().GetLength());
            });

            context.Complete(length);
        }

        public void InvokeXmlReader(TestInvoke context)
        {
            long length = 0;
            char[] buffer = new char[4 * 1024 * 1024];

            context.BenchmarkReader("text-length", "revision", reader =>
            {
                int depth = reader.Depth;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "text")
                    {
                        int read = 0;

                        do
                        {
                            read = reader.ReadChars(buffer, 0, buffer.Length);
                            length = length + read;
                        }
                        while (read > 0);
                    }

                    if (depth == reader.Depth)
                        return;
                }
            });

            context.Complete(length);
        }

        public void InvokeXmlLinq(TestInvoke context)
        {
            throw new System.NotImplementedException();
        }
    }
}
