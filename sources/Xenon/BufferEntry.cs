using System;
using System.Collections.Generic;

namespace Xenon
{
    public sealed class BufferEntry
    {
        private readonly int id;
        private readonly IReadOnlyList<BufferBlock> blocks;

        private readonly int available;
        private readonly int size;
        private readonly byte[] zero;

        private List<byte[]> data;
        private List<int> sizes;

        public BufferEntry(int id, IReadOnlyList<BufferBlock> blocks)
        {
            this.id = id;
            this.blocks = blocks;

            this.available = blocks.Count - id;
            this.zero = blocks[id].Data;
            this.size = blocks[id].Size;

            this.data = new List<byte[]> { blocks[id].Data };
            this.sizes = new List<int> { blocks[id].Size };
        }

        public int Id
        {
            get { return id; }
        }

        public byte[] PrimaryData
        {
            get { return zero; }
        }

        public int PrimarySize
        {
            get { return size; }
        }

        public byte[] GetData(int index, out int count, out int skip)
        {
            if (index < size)
            {
                skip = 0;
                count = size;
                return zero;
            }

            skip = size;
            index = index - size;

            for (int i = 1; i < available; i++)
            {
                if (data.Count == i)
                {
                    data.Add(null);
                    sizes.Add(0);
                }

                if (data[i] == null)
                {
                    if (blocks.Count <= id + i)
                        break;

                    if (blocks[id + i] == null)
                        break;

                    blocks[id + i].Request.Release();
                    blocks[id + i].Ready.Wait();

                    data[i] = blocks[id + i].Data;
                    sizes[i] = blocks[id + i].Size;
                }

                if (sizes[i] > index)
                {
                    count = sizes[i];
                    return data[i];
                }

                skip = skip + sizes[i];
                index = index - sizes[i];
            }

            count = 1;
            return new byte[] {0};
        }

        public byte GetByteAt(int index)
        {
            if (index < size)
                return zero[index];

            index = index - size;

            for (int i = 1; i < available; i++)
            {
                if (data.Count == i)
                {
                    data.Add(null);
                    sizes.Add(0);
                }

                if (data[i] == null)
                {
                    if (blocks.Count <= id + i)
                        break;

                    if (blocks[id + i] == null)
                        break;

                    blocks[id + i].Request.Release();
                    blocks[id + i].Ready.Wait();

                    data[i] = blocks[id + i].Data;
                    sizes[i] = blocks[id + i].Size;
                }

                if (sizes[i] > index)
                    return data[i][index];

                index = index - sizes[i];
            }

            return 0;
        }

        public bool IsEqual(ref int index, string value)
        {
            char character;

            for (int i = 0; i < value.Length; i++)
            {
                character = GetCharacterAt(index, out int length);
                index = index + length;

                if (character != value[i])
                    return false;
            }

            return true;
        }

        public int GetLengthAt(int index)
        {
            byte value = GetByteAt(index);

            if (value < 128)
                return 1;

            if (value < 224)
                return 2;

            if (value < 240)
                return 3;

            if (value < 248)
                return 4;

            throw new InvalidOperationException();
        }

        public static int GetLength(byte value)
        {
            if (value < 128)
                return 1;

            if (value < 224)
                return 2;

            if (value < 240)
                return 3;

            if (value < 248)
                return 4;

            throw new InvalidOperationException();
        }

        public char GetCharacterAt(int index, out int length)
        {
            int result = 0;
            byte first = GetByteAt(index);

            if (first < 128)
            {
                length = 1;
                result = first;
            }

            else if (first < 224)
            {
                byte second = GetByteAt(index + 1);

                length = 2;
                result = ((first & 0b00011111) << 6) + (second & 0b00111111);
            }

            else if (first < 240)
            {
                byte second = GetByteAt(index + 1);
                byte third = GetByteAt(index + 2);

                length = 3;
                result = ((first & 0b00001111) << 12) + ((second & 0b00111111) << 6) + (third & 0b00111111);
            }

            else if (first < 248)
            {
                byte second = GetByteAt(index + 1);
                byte third = GetByteAt(index + 2);
                byte forth = GetByteAt(index + 3);

                length = 4;
                result = ((first & 0b00000111) << 18) + ((second & 0b00111111) << 12) + ((third & 0b00111111) << 6) + (forth & 0b00111111);
            }
            else
            {
                throw new InvalidOperationException();
            }


            return (char)result;
        }
    }
}
