using FileParser;

namespace AdventOfCode;

public class Day5 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<(int, int)> _pageOrders = [];
    private readonly List<List<int>> _updates = [];


    public Day5()
    {
        _input = new ParsedFile(InputFilePath);
        var lines = _input.ToList<string>();
        foreach (var line in lines)
        {
            if (line.Contains('|'))
            {
                // Parse lines containing '|' into (int, int) pairs
                var items = line.Split('|').Select(int.Parse).ToArray();
                _pageOrders.Add((items[0], items[1]));
            }
            else
            {
                // Parse other lines into a list of integers
                var items = line.Split(',').Select(int.Parse).ToList();
                _updates.Add(items);
            }
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private object Solve_1_v1()
    {
        var result = 0;
        var pageIncorrect = false;
        foreach (var update in _updates)
        {
            for (int i = 0; i < update.Count; i++)
            {
                var item = update[i];
                // Create the list of previous items (all items before the current index)
                var previousItems = update.Take(i).ToList();

                // Create the list of following items (all items after the current index)
                var followingItems = update.Skip(i + 1).ToList();

                //now check if there is a pageOrder for the previous items
                var previousCorrecct = previousItems.All(i => _pageOrders.Contains((i, item)));
                var followingCorrecct = followingItems.All(i => _pageOrders.Contains((item, i)));

                if (!previousCorrecct || !followingCorrecct)
                {
                    pageIncorrect = true;
                    break;
                }
            }

            if (!pageIncorrect)
            {
                result += GetMiddleItem(update);
            }
            pageIncorrect = false;
        }

        return result;
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

    public int GetMiddleItem(List<int> inputList)
    {
        // Check if the list is empty
        if (inputList.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }

        // Calculate the middle index
        int middleIndex = inputList.Count / 2;

        // If the list has an even number of elements, the middle item can be either of the two middle items
        // This will return the item at the middle index (for odd lengths) or the first of the two middle items (for even lengths)
        return inputList[middleIndex];
    }
}
