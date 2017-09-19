using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xenon
{
    public class BufferWindow
    {
        private readonly int blockCount;
        private readonly int blockSize;
        private readonly Stream source;

        public BufferWindow(int blockCount, int blockSize, Stream source)
        {
            this.blockCount = blockCount;
            this.blockSize = blockSize;
            this.source = source;
        }

        public async Task Process(BufferHandler handler)
        {
            int started = 0;
            int registered = 0;

            Queue<byte[]> memory = new Queue<byte[]>(blockCount + 1);
            Queue<Task<BufferEntry>> tasks = new Queue<Task<BufferEntry>>(blockCount);
            BufferBlock[] blocks = new BufferBlock[(source.Length - 1) / blockSize + 1];

            void newBlockIfNotExists(int id)
            {
                if (blocks.Length > id && blocks[id] == null)
                {
                    blocks[id] = new BufferBlock
                    {
                        Request = new SemaphoreSlim(0, blocks.Length - id),
                        Ready = new ManualResetEventSlim(false)
                    };
                }
            }

            void enableBlock(int id, byte[] data, int size)
            {
                blocks[id].Data = data;
                blocks[id].Size = size;
                blocks[id].Ready.Set();
            }

            async Task<bool> readNextBlock(int id)
            {
                byte[] data;

                if (memory.Count == 0)
                    data = new byte[blockSize];
                else
                    data = memory.Dequeue();

                int read = await ReadNext(data).ConfigureAwait(false);
                if (read == 0)
                    return false;

                for (int i = 0; i <= 1; i++)
                {
                    newBlockIfNotExists(id + i);
                }

                enableBlock(id, data, read);
                return true;
            }

            while (true)
            {
                if (await readNextBlock(registered++).ConfigureAwait(false) == false)
                    break;

                while (tasks.Count == blockCount)
                {
                    List<Task> wait = new List<Task> { tasks.Peek() };

                    if (blocks.Length > registered)
                    {
                        wait.Add(blocks[registered].Request.WaitAsync());
                    }

                    int index = Task.WaitAny(wait.ToArray());

                    if (index == 0)
                    {
                        BufferEntry entry = tasks.Dequeue().Result;
                        BufferBlock block = blocks[entry.Id];

                        //Console.WriteLine($"awaited block {entry.Id}");

                        blocks[entry.Id] = null;
                        memory.Enqueue(block.Data);

                        block.Request.Dispose();
                        block.Ready.Dispose();
                        block.Data = null;
                    }
                    else
                    {
                        //Console.WriteLine($"awaited request");

                        if (await readNextBlock(registered++).ConfigureAwait(false) == false)
                            break;
                    }
                }

                //Console.WriteLine($"started block {started}");
                tasks.Enqueue(handler.Handle(new BufferEntry(started++, blocks)));
            }

            while (started < blocks.Length)
            {
                tasks.Enqueue(handler.Handle(new BufferEntry(started++, blocks)));
            }

            while (tasks.Count > 0)
            {
                await tasks.Dequeue();
            }
        }

        private async Task<int> ReadNext(byte[] data)
        {
            int total = 0;

            while (total < data.Length)
            {
                int read = await source.ReadAsync(data, total, data.Length - total).ConfigureAwait(false);

                if (read == 0)
                    break;

                total += read;
            }

            return total;
        }
    }
}
