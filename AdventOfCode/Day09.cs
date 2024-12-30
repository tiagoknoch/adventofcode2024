

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;
    private List<int?> _disk;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    private void PrepareDisk()
    {
        _disk = new List<int?>();
        var numbers = _input.Select(c => int.Parse(c.ToString())).ToList();

        int id = 0;
        bool isFile = true;
        foreach (var number in numbers)
        {
            if (isFile)
            {
                _disk.AddRange(Enumerable.Repeat((int?)id, number));
                id++;
            }
            else
            {
                _disk.AddRange(Enumerable.Repeat((int?)null, number));
            }

            //flip isFile
            isFile = !isFile;
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private long Solve_1_v1()
    {

        PrepareDisk();
        //PrintDisk();
        SortDisk();
        //PrintDisk();
        var result = CalculateCheckSum();
        return result;
    }

    private void SortDisk()
    {
        for (int i = _disk.Count - 1; i >= 0; i--)
        {
            if (AreNumbersAtFront(_disk))
            {
                break;
            }

            var item = _disk[i];
            if (item == null)
            {
                continue;
            }

            //find the first null item
            var emptyIndex = _disk.FindIndex(x => x == null);
            if (emptyIndex == -1)
            {
                break;
            }

            _disk[emptyIndex] = item;
            _disk[i] = null;
        }
    }


    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private long Solve_2_v1()
    {
        PrepareDisk();
        //PrintDisk();
        SortDisk2();
        var result = CalculateCheckSum();
        return result;
    }

    private void SortDisk2()
    {
        var processedItems = new HashSet<int?>();
        for (int i = _disk.Count - 1; i >= 0; i--)
        {
            var item = _disk[i];

            if (item == null || processedItems.Contains(item))
            {
                continue;
            }

            var lastIndex = i;
            var firstIndex = _disk.FindIndex(x => x == item);
            var sequenceSize = lastIndex - firstIndex + 1;

            var nullIndex = FindSequenceOfNulls(_disk, sequenceSize);

            if (nullIndex == -1 || nullIndex > firstIndex)
            {
                processedItems.Add(item);
                continue;
            }

            for (int j = 0; j < sequenceSize; j++)
            {
                _disk[nullIndex + j] = item;
                _disk[firstIndex + j] = null;
            }

        }
    }

    private void PrintDisk()
    {
        foreach (var item in _disk)
        {
            Console.Write(item == null ? "." : item.ToString());
        }
        Console.WriteLine();
    }
    private long CalculateCheckSum()
    {
        long checksum = 0;
        for (int i = 0; i < _disk.Count; i++)
        {
            if (_disk[i] == null)
            {
                continue;
            }


            checksum += _disk[i].Value * i;
        }

        return checksum;
    }

    bool AreNumbersAtFront(List<int?> list)
    {
        bool nullEncountered = false;

        foreach (var item in list)
        {
            if (item == null)
            {
                nullEncountered = true; // Start of nulls
            }
            else if (nullEncountered)
            {
                // If a number appears after a null, return false
                return false;
            }
        }

        return true;
    }

    private int FindSequenceOfNulls(List<int?> list, int N)
    {
        int consecutiveNulls = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                consecutiveNulls++;
                // If we found a sequence of N nulls, return the starting index
                if (consecutiveNulls == N)
                {
                    return i - N + 1; // Starting index of the sequence
                }
            }
            else
            {
                consecutiveNulls = 0; // Reset count if a non-null value is found
            }
        }

        return -1; // Return -1 if no sequence of N nulls is found
    }
}
