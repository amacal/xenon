using System.Dynamic;

namespace Xenon
{
    public class XenonDocument : XenonNode
    {
        public XenonDocument(BufferEntry entry, XenonHierarchy hierarchy)
            : base(hierarchy.GetFirst(0), entry, hierarchy)
        {
        }

        public dynamic GetDynamic()
        {
            return new Dynamic(this);
        }

        private class Dynamic : DynamicObject
        {
            private readonly XenonDocument document;

            public Dynamic(XenonDocument document)
            {
                this.document = document;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = document.Get(binder.Name);
                return true;
            }
        }
    }
}
