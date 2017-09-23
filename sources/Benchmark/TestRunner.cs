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
                if (group.Support.HasFlag(TestSupport.Static))
                    group.InvokeXenonStatic(context);

                if (group.Support.HasFlag(TestSupport.Dynamic))
                    group.InvokeXenonDynamic(context);

                if (group.Support.HasFlag(TestSupport.Reader))
                    group.InvokeXmlReader(context);

                if (group.Support.HasFlag(TestSupport.Linq))
                    group.InvokeXmlLinq(context);
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
