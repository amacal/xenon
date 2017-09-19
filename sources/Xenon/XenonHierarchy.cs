namespace Xenon
{
    public class XenonHierarchy
    {
        private XenonRow[] rows;
        private int used;

        public XenonHierarchy(XenonRow[] rows)
        {
            this.rows = rows;
            this.used = 0;
        }

        public int Add()
        {
            rows[used].Position = 0;
            rows[used].Length = 0;

            rows[used].First = 0;
            rows[used].Next = 0;

            return used++;
        }

        public int Add(int position, int length)
        {
            rows[used].Position = position;
            rows[used].Length = length;

            rows[used].First = 0;
            rows[used].Next = 0;

            return used++;
        }

        public void SetPosition(int owner, int position)
        {
            rows[owner].Position = position;
        }

        public int GetPosition(int owner)
        {
            return rows[owner].Position;
        }

        public void SetLength(int owner, int length)
        {
            rows[owner].Length = length;
        }

        public int GetLength(int owner)
        {
            return rows[owner].Length;
        }

        public int GetNext(int owner)
        {
            return rows[owner].Next;
        }

        public void SetNext(int owner, int next)
        {
            rows[owner].Next = next;
        }

        public int GetFirst(int owner)
        {
            return rows[owner].First;
        }

        public void SetFirst(int owner, int first)
        {
            rows[owner].First = first;
        }
    }
}
