using System;
using System.Threading.Tasks;

namespace Xenon
{
    public static class XenonExtensions
    {
        public static Task Process(this XenonReader reader, Action<XenonDocument> callback)
        {
            return reader.Process(new CallbackHandler(callback));
        }

        private class CallbackHandler : XenonHandler
        {
            private readonly Action<XenonDocument> callback;

            public CallbackHandler(Action<XenonDocument> callback)
            {
                this.callback = callback;
            }

            public void Handle(XenonDocument document)
            {
                callback.Invoke(document);
            }
        }
    }
}
