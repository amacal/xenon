using System;

namespace Benchmark
{
    [Flags]
    public enum TestSupport
    {
        None,
        Static,
        Dynamic,
        Reader,
        Linq
    }
}
