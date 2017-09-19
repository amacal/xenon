using System.Collections;
using System.Collections.Generic;

namespace Benchmark
{
    public class TestRunner : IEnumerable<TestGroup>
    {
        private readonly List<TestGroup> groups;

        public TestRunner()
        {
            this.groups = new List<TestGroup>();
        }

        public void Add(TestGroup group)
        {
            this.groups.Add(group);
        }

        public void Execute(TestInvoke context)
        {
            foreach (TestGroup group in groups)
            {
                group.InvokeXenonStatic(context);
                group.InvokeXenonDynamic(context);
                group.InvokeXmlReader(context);
            }
        }

        public IEnumerator<TestGroup> GetEnumerator()
        {
            return groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
