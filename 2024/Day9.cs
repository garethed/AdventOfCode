using System.Text;

namespace AdventOfCode.AoC2024;

class Day9
{
    [Solution]
    [Test(1928, "2333133121414131402")]
    public long Part1(string input)
    {
        var ints = input.Select(c => long.Parse(c.ToString())).ToArray();
        var data = new List<long>();

        bool isFile = true;
        long fileId = 0L;
        foreach (var ii in ints)
        {
            for (int jj = 0; jj < ii; jj++)
            {
                data.Add(isFile ? fileId : -1);
            }
            isFile = !isFile;
            if (isFile) 
            {
                fileId++;
            }
        }

        int i = 0, j = data.Count - 1;
        while (i < j)
        {
            if (data[i] < 0)
            {
                data[i] = data[j];
                data[j] = -1;
                
                do
                {
                    j--;
                }
                while (data[j] < 0);
            }
            i++;
        }

        var chk = 0L;

        for (int ii = 0; ii < data.Count; ii++)
        {
            if (data[ii] < 0) break;
            chk += ii * data[ii];
        }

        return chk;        
    }

    [Solution]
    [Test(2858, "2333133121414131402")]
    public long Part2(string input)
    {
        var ints = input.Select(c => long.Parse(c.ToString())).ToArray();
        var data = new List<Segment>();

        void mergeSegments(int i)
        {
            if (i + 1 < data.Count && data[i].FileId < 0 && data[i + 1].FileId < 0)
            {
                data[i].Length = data[i].Length + data[i + 1].Length;
                data.RemoveAt(i + 1);
            }
        }

        for (int i = 0; i < ints.Length; i++)
        {
            var fileId = (i % 2 == 0) ? i / 2 : -1;
            var len = ints[i];        
            data.Add(new Segment(fileId, len));
        }

        for (int j = data.Count - 1; j >= 0; j--)
        {
            var toMove = data[j];

            if (toMove.FileId >= 0)
            {
                Segment? swapWith = null;
                int k = 0;
                for (k = 0; k < j; k++)
                {
                    var s = data[k];
                    if (s.FileId < 0 && s.Length >= toMove.Length)
                    {
                        swapWith = data[k];
                        break;
                    }
                }
                 
                if (swapWith != null)
                {
                    if (toMove.Length < swapWith.Length)
                    {
                        var newSegment = new Segment(-1, swapWith.Length - toMove.Length);
                        data.Insert(k + 1, newSegment);
                        j++;
                    }

                    swapWith.FileId = toMove.FileId;
                    swapWith.Length = toMove.Length;
                    toMove.FileId = -1;
                    mergeSegments(j);
                    mergeSegments(j - 1);
                }
            }

        }
        
        var chk = 0L;
        var ii = 0L;

        foreach (var s in data)
        {
            if (s.FileId >= 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    chk += ((ii + i) * s.FileId);
                }
            }
            ii += s.Length;
        }

        return chk;        
    }    

    class Segment(long fileId, long length) {

        public long FileId = fileId;
        public long Length = length;
    }
}