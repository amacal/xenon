using System.Threading;
using System.Xml;
using Xenon;

namespace Benchmark.Groups
{
    public class TextLengthGroup : TestGroup
    {
        public void InvokeXenonStatic(TestInvoke context)
        {
            long length = 0;

            context.BenchmarkXenon("text-length", "xenon-static", "revision", reader =>
            {
                return reader.Process(document =>
                {
                    Interlocked.Add(ref length, document.Get("text").ToText().GetLength());
                });
            });
        }

        public void InvokeXenonDynamic(TestInvoke context)
        {
            long length = 0;

            context.BenchmarkXenon("text-length", "xenon-dynamic", "revision", reader =>
            {
                return reader.Process(document =>
                {
                    Interlocked.Add(ref length, document.GetDynamic().text.ToText().GetLength());
                });
            });
        }

        public void InvokeXmlReader(TestInvoke context)
        {
            int length = 0;
            char[] buffer = new char[1024];

            context.BenchmarkReader("text-length", reader =>
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "revision")
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "sha1")
                            {
                                break;
                            }

                            if (reader.NodeType == XmlNodeType.Element && reader.Name == "text")
                            {
                                reader.MoveToContent();
                                int read = 0;

                                do
                                {
                                    read = reader.ReadChars(buffer, 0, buffer.Length);
                                    length = length + read;
                                } while (read > 0);
                            }
                        }
                    }
                }
            });
        }

        public void InvokeXmlDocument(TestInvoke context)
        {
            throw new System.NotImplementedException();
        }
    }
}
