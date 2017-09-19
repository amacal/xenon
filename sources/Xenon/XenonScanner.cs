namespace Xenon
{
    public static class XenonScanner
    {
        public static int NextOpening(BufferEntry entry, int position, string name)
        {
            int index;
            byte value;

            int size = entry.PrimarySize;
            byte[] primary = entry.PrimaryData;

            while (position < size)
            {
                index = position + 1;
                value = primary[position];

                if (value == '\0')
                    return -1;

                if (value == '<' && entry.IsEqual(ref index, name))
                {
                    value = entry.GetByteAt(index);

                    if (value == ' ' || value == '>' || value == '/' || value == '\r' || value == '\n' || value == '\t')
                        break;
                }

                position = index;
            }

            return position;
        }

        public static int NextElementName(BufferEntry entry, int position)
        {
            byte value;
            int size, skip;

            byte[] data = entry.GetData(0, out size, out skip);

            while (position < size)
            {
                value = data[position];

                if (value == '/' || value == '>' || value == ' ' || value == '\r' || value == '\n' || value == '\t')
                    return position;

                position++;
            }

            while (true)
            {
                data = entry.GetData(position, out size, out skip);
                int index = position - skip;

                while (index < size)
                {
                    value = data[index];

                    if (value == '/' || value == '>' || value == ' ' || value == '\r' || value == '\n' || value == '\t')
                        return position;

                    if (value == '\0')
                        return -1;

                    index++;
                    position++;
                }
            }
        }

        public static int NextAttributeName(BufferEntry entry, int position)
        {
            byte value;

            while (true)
            {
                value = entry.GetByteAt(position);

                if (value == '=' || value == ' ' || value == '\r' || value == '\n' || value == '\t')
                    break;

                if (value == '\0')
                    return -1;

                position++;
            }

            return value != '\0' ? position : -1;
        }

        public static int NextAttributeValue(BufferEntry entry, int position)
        {
            byte value;

            while (true)
            {
                value = entry.GetByteAt(position);

                if (value == '"' || value == '\0')
                    break;

                position++;
            }

            return value != '\0' ? position : -1;
        }

        public static int NextContent(BufferEntry entry, int position)
        {
            byte value;
            int size, skip;

            byte[] data = entry.GetData(0, out size, out skip);

            while (position < size)
            {
                value = data[position];

                if (value == '<')
                    return position;

                position++;
            }

            while (true)
            {
                data = entry.GetData(position, out size, out skip);
                int index = position - skip;

                while (index < size)
                {
                    value = data[index];

                    if (value == '<')
                        return position;

                    if (value == '\0')
                        return -1;

                    index++;
                    position++;
                }
            }
        }

        public static int SkipWhiteCharactersForward(BufferEntry entry, int position)
        {
            byte value;
            int size, skip;

            byte[] data = entry.GetData(0, out size, out skip);

            while (position < size)
            {
                value = data[position];

                if (value != ' ' && value != '\r' && value != '\n' && value != '\t')
                    return position;

                position++;
            }

            while (true)
            {
                data = entry.GetData(position, out size, out skip);
                int index = position - skip;

                while (index < size)
                {
                    value = data[index];

                    if (value != ' ' && value != '\r' && value != '\n' && value != '\t')
                        return position;

                    if (value == '\0')
                        return -1;

                    index++;
                    position++;
                }
            }
        }
        public static int SkipWhiteCharactersBackward(BufferEntry entry, int position)
        {
            byte value;
            int size, skip;

            while (true)
            {
                byte[] data = entry.GetData(position, out size, out skip);
                int index = position - skip;

                while (index >= 0)
                {
                    value = data[index];

                    if (value != ' ' && value != '\r' && value != '\n' && value != '\t')
                        return position;

                    index--;
                    position--;
                }
            }
        }

    }
}
