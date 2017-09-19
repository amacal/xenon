using System;
using System.Collections.Generic;
using System.Text;

namespace Xenon
{
    public class XenonNode
    {
        private readonly int id;
        private readonly BufferEntry entry;
        private readonly XenonHierarchy hierarchy;

        protected XenonNode(int id, BufferEntry entry, XenonHierarchy hierarchy)
        {
            this.id = id;
            this.entry = entry;
            this.hierarchy = hierarchy;
        }

        public bool Has(string key)
        {
            int position = 0;
            int index = id;

            while (index > 0)
            {
                int length = hierarchy.GetLength(index);
                int current = position = hierarchy.GetPosition(index);

                if (entry.IsEqual(ref current, key) && current == position + length)
                    break;

                index = hierarchy.GetNext(index);
            }

            if (index > 0)
                index = hierarchy.GetFirst(index);

            return index > 0;
        }

        public XenonNode Get(string key)
        {
            int position = 0;
            int index = id;

            while (index > 0)
            {
                int length = hierarchy.GetLength(index);
                int current = position = hierarchy.GetPosition(index);

                if (entry.IsEqual(ref current, key) && current == position + length)
                    break;

                index = hierarchy.GetNext(index);
            }

            if (index > 0)
                index = hierarchy.GetFirst(index);

            if (index == 0)
                return null;

            return new XenonNode(index, entry, hierarchy);
        }

        public IEnumerable<XenonNode> GetEnumerable(string key)
        {
            int index = id;
            int position = 0;

            while (index > 0)
            {
                int length = hierarchy.GetLength(index);
                int current = position = hierarchy.GetPosition(index);

                if (entry.IsEqual(ref current, key) && current == position + length)
                {
                    yield return new XenonNode(hierarchy.GetFirst(index), entry, hierarchy);
                }

                index = hierarchy.GetNext(index);
            }
        }

        public override string ToString()
        {
            int position = hierarchy.GetPosition(id);

            if (id > 0)
            {
                int length = hierarchy.GetLength(id);
                StringBuilder builder = new StringBuilder(length);

                if (length < 0)
                    length = -length;

                while (length > 0)
                {
                    builder.Append(entry.GetCharacterAt(position, out int read));
                    position = position + read;
                    length = length - read;
                }

                return builder.ToString();
            }

            return null;
        }

        public string ToString(XenonWhiteCharacter mode)
        {
            if (id > 0)
            {
                int length = hierarchy.GetLength(id);
                int position = hierarchy.GetPosition(id);
                StringBuilder builder = new StringBuilder(length);

                if (length < 0)
                    length = -length;

                if (mode == XenonWhiteCharacter.Omit)
                {
                    int next = XenonScanner.SkipWhiteCharactersForward(entry, position);
                    length = length - (next - position);
                    position = next;
                }

                if (length > 0)
                {
                    int prev = XenonScanner.SkipWhiteCharactersBackward(entry, position + length - 1);
                    length = length - (position + length - 1 - prev);
                }

                while (length > 0)
                {
                    builder.Append(entry.GetCharacterAt(position, out int read));
                    position = position + read;
                    length = length - read;
                }

                return builder.ToString();
            }

            return null;
        }

        public XenonText ToText()
        {
            return new XenonText(id, entry, hierarchy);
        }

        public long? ToInt64()
        {
            return Int64.Parse(ToString());
        }

        public double? ToDouble()
        {
            return Double.Parse(ToString());
        }

        public DateTime? ToDateTime()
        {
            return DateTime.Parse(ToString());
        }

        private int FindKey(string key)
        {
            int position = 0;
            int index = id;

            while (index > 0)
            {
                int length = hierarchy.GetLength(index);
                int current = position = hierarchy.GetPosition(index);

                if (entry.IsEqual(ref current, key) && current == position + length)
                    break;

                index = hierarchy.GetNext(index);
            }

            if (index > 0)
                index = hierarchy.GetFirst(index);

            return index;
        }
    }
}
