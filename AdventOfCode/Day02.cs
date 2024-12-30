namespace AdventOfCode;

using FileParser;

public class Day02 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<List<int>> _lines;

    public Day02()
    {
        _input = new ParsedFile(InputFilePath);
        _lines = [];
        while (!_input.Empty)
        {
            _lines.Add(_input.NextLine().ToList<int>());
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");
    public override ValueTask<string> Solve_2() => new($"{Solve_2_v2()}");


    public int Solve_1_v1()
    {
        return _lines.Count(IsLineSafe);
    }

    public int Solve_2_v1()
    {
        var linesSafe = 0;
        foreach (var line in _lines)
        {
            if (IsLineSafe(line))
            {
                linesSafe += 1;
            }
            else
            {
                //remove one item from list
                for (int i = 0; i < line.Count; i++)
                {
                    var copyList = line.ToList();
                    copyList.RemoveAt(i);
                    if (IsLineSafe(copyList))
                    {
                        linesSafe += 1;
                        break;
                    }
                }
            }
        }

        return linesSafe;
    }

    public int Solve_2_v2()
    {
        var linesSafe = 0;

        foreach (var line in _lines)
        {
            if (IsLineSafe(line))
            {
                linesSafe++;
                continue;
            }

            // Check if removing a single element makes the line safe
            for (int i = 0; i < line.Count; i++)
            {
                var modifiedLine = line.Where((_, index) => index != i).ToList();
                if (IsLineSafe(modifiedLine))
                {
                    linesSafe++;
                    break;
                }
            }
        }

        return linesSafe;
    }

    public static bool AreDifferencesMonotonic(List<int> numbers)
    {
        if (numbers == null || numbers.Count < 2)
            return true; // Not enough elements to determine monotonicity

        // Compute the differences between consecutive elements
        var differences = numbers.Zip(numbers.Skip(1), (a, b) => b - a).ToList();

        // Check if all differences are non-negative (increasing) or non-positive (decreasing)
        bool isIncreasing = differences.All(diff => diff > 0);
        bool isDecreasing = differences.All(diff => diff < 0);

        return isIncreasing || isDecreasing;
    }

    private static bool IsLineSafe(List<int> values)
    {
        //first check if the list is monotonic
        if (!AreDifferencesMonotonic(values))
        {
            return false;
        }

        // Check if all consecutive differences are within the allowed range
        return values.Zip(values.Skip(1), (prev, current) => Math.Abs(current - prev))
                     .All(difference => difference >= 1 && difference <= 3);
    }
}
