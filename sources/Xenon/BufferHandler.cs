using System.Threading.Tasks;

namespace Xenon
{
    public interface BufferHandler
    {
        Task<BufferEntry> Handle(BufferEntry entry);
    }
}
