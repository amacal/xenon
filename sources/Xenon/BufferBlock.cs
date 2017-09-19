using System.Threading;

namespace Xenon
{
    public class BufferBlock
    {
        public int Size;
        public byte[] Data;

        public SemaphoreSlim Request;
        public ManualResetEventSlim Ready;
    }
}
