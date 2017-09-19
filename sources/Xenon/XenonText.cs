namespace Xenon
{
    public class XenonText
    {
        private readonly int id;
        private readonly BufferEntry entry;
        private readonly XenonHierarchy hierarchy;

        public XenonText(int id, BufferEntry entry, XenonHierarchy hierarchy)
        {
            this.id = id;
            this.entry = entry;
            this.hierarchy = hierarchy;
        }

        public int GetLength()
        {
            int read = 0;
            int characters = 0;

            int length = hierarchy.GetLength(id);
            int position = hierarchy.GetPosition(id);

            if (length > 0)
            {
                byte value;
                byte[] data = entry.GetData(0, out int size, out int skip);

                while (position < size)
                {
                    value = data[position];
                    read = value < 128 ? 1 : BufferEntry.GetLength(value);

                    length = length - read;
                    position = position + read;
                    characters = characters + (read <= 3 ? 1 : 2);

                    if (length <= 0)
                        return characters;
                }

                while (true)
                {
                    data = entry.GetData(position, out size, out skip);
                    int offset = position - skip;

                    while (offset < size)
                    {
                        value = data[offset];
                        read = value < 128 ? 1 : BufferEntry.GetLength(value);

                        length = length - read;
                        position = position + read;
                        characters = characters + (read <= 3 ? 1 : 2);
                        offset = offset + read;

                        if (length <= 0)
                            return characters;
                    }
                }
            }

            return characters;
        }

        public int GetSize(string key)
        {
            int length = hierarchy.GetLength(id);
            return length > 0 ? length : -length;
        }
    }
}
