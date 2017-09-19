using System;
using System.Threading.Tasks;

namespace Xenon
{
    public static class BufferExtensions
    {
        public static Task Process(this BufferWindow window, Action<BufferEntry> callback)
        {
            return window.Process(new CallbackHandler(callback));
        }

        private class CallbackHandler : BufferHandler
        {
            private readonly Action<BufferEntry> callback;

            public CallbackHandler(Action<BufferEntry> callback)
            {
                this.callback = callback;
            }

            public Task<BufferEntry> Handle(BufferEntry entry)
            {
                return Task.Run(() =>
                {
                    callback(entry);
                    return entry;
                });
            }
        }
    }
}
