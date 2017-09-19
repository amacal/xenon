using System;
using System.IO;
using System.Threading.Tasks;

namespace Xenon
{
    public class XenonReader
    {
        private readonly string name;
        private readonly BufferWindow inner;
        private readonly int size;

        public XenonReader(Stream source, string name, int bufferSize = 1024 * 1024)
        {
            this.name = name;
            this.size = bufferSize;

            this.inner = new BufferWindow(Environment.ProcessorCount, size, source);
        }

        public Task Process(XenonHandler handler)
        {
            return inner.Process(entry =>
            {
                XenonRow[] rows = new XenonRow[1024];
                int position = XenonScanner.NextOpening(entry, 0, name);

                while (position >= 0 && position < size)
                {
                    XenonHierarchy hierarchy = new XenonHierarchy(rows);

                    if (Handle(entry, hierarchy, ref position) == -1)
                        return;

                    handler.Handle(new XenonDocument(entry, hierarchy));
                    position = XenonScanner.NextOpening(entry, position, name);
                }
            });
        }

        private int Handle(BufferEntry entry, XenonHierarchy hierarchy, ref int position)
        {
            //Console.WriteLine($"el {position}");

            byte value;
            bool self = false;

            if (entry.GetByteAt(position++) != '<')
                return -1;

            int starting = position;
            if ((position = XenonScanner.NextElementName(entry, starting)) == -1)
                return -1;

            // handle element start
            int previousId = 0;
            int owner = hierarchy.Add(starting, position - starting);

            if ((position = XenonScanner.SkipWhiteCharactersForward(entry, position)) == -1)
                return -1;

            if ((value = entry.GetByteAt(position)) == 0)
                return -1;

            int append(int id)
            {
                if (previousId == 0)
                {
                    hierarchy.SetFirst(owner, id);
                    previousId = id;
                }
                else
                {
                    hierarchy.SetNext(previousId, id);
                    previousId = id;
                }

                return id;
            }

            int addToHierarchy(int index, int length)
            {
                return append(hierarchy.Add(index, length));
            }

            int contentId = append(hierarchy.Add());
            void setContent(int index, int length)
            {
                if (contentId != 0)
                {
                    hierarchy.SetPosition(contentId, index);
                    hierarchy.SetLength(contentId, length);
                    contentId = 0;
                }
                else
                {
                    addToHierarchy(index, length);
                }
            }

            while (value != '/' && value != '>')
            {
                starting = position;
                if ((position = XenonScanner.NextAttributeName(entry, starting)) == -1)
                    return -1;

                // handle attribute name
                int nameId = addToHierarchy(starting, position - starting);
                
                if ((position = XenonScanner.SkipWhiteCharactersForward(entry, position)) == -1)
                    return -1;

                if (entry.GetByteAt(position++) != '=')
                    return -1;

                if ((position = XenonScanner.SkipWhiteCharactersForward(entry, position)) == -1)
                    return -1;

                if (entry.GetByteAt(position++) != '"')
                    return -1;

                starting = position;
                if ((position = XenonScanner.NextAttributeValue(entry, starting)) == -1)
                    return -1;

                // handle attribute value
                int valueId = hierarchy.Add(starting, position - starting);
                hierarchy.SetFirst(nameId, valueId);

                if (entry.GetByteAt(position++) != '"')
                    return -1;

                if ((position = XenonScanner.SkipWhiteCharactersForward(entry, position)) == -1)
                    return -1;

                if ((value = entry.GetByteAt(position)) == 0)
                    return -1;
            }

            if (value == '/')
            {
                if ((value = entry.GetByteAt(++position)) == 0)
                    return -1;

                self = true;
            }

            if (value != '>')
                return -1;

            if (self == false)
            {
                position = position + 1;

                while (true)
                {
                    starting = position;

                    if ((position = XenonScanner.SkipWhiteCharactersForward(entry, position)) == -1)
                        return -1;

                    if (starting == position)
                    {
                        if ((value = entry.GetByteAt(position)) == 0)
                            return -1;

                        if (value == '<')
                        {
                            if ((value = entry.GetByteAt(position + 1)) == 0)
                                return -1;

                            if (value == '/')
                            {
                                starting = position + 2;
                                if ((position = XenonScanner.NextElementName(entry, starting + 2)) == -1)
                                    return -1;

                                // handle element end

                                if ((value = entry.GetByteAt(position)) != '>')
                                    return -1;

                                //Console.WriteLine($"cl {position}");
                                return owner;
                            }

                            if ((starting = Handle(entry, hierarchy, ref position)) == -1)
                                return -1;

                            append(starting);
                            position++;

                            continue;
                        }
                    }

                    if ((position = XenonScanner.NextContent(entry, position)) == -1)
                        return -1;

                    setContent(starting, position - starting);
                }
            }

            return owner;
        }
    }
}
