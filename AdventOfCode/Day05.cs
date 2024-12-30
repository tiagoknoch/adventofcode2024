using FileParser;
using System.Linq;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<(int, int)> _pageOrders = [];
    private readonly List<List<int>> _updates = [];


    public Day05()
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


    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");
    private object Solve_1_v1()
    {
        var result = 0;
        foreach (var update in _updates.Where(IsPageCorrect))
        {
            result += GetMiddleItem(update);
        }

        return result;
    }

    private object Solve_2_v1()
    {
        // var result = 0;
        // foreach (var update in _updates.Where(i => !IsPageCorrect(i)))
        // {
        //     for (int i = 0; i < update.Count; i++)
        //     {
        //         var currentItem = update[i];

        //         // Check all items before and after the current index
        //         int? previousIncorrect = update.Take(i).FirstOrDefault(prev => !_pageOrders.Contains((prev, currentItem)));
        //         int? followingIncorrect = update.Skip(i + 1).FirstOrDefault(next => !_pageOrders.Contains((currentItem, next)));

        //         if (previousIncorrect.HasValue)
        //         {
        //             Swap(update, i, update.IndexOf(followingIncorrect.Value));
        //         }

        //         if (followingIncorrect.HasValue)
        //         {
        //             Swap(update, i, update.IndexOf(followingIncorrect.Value));
        //         }

        //     }

        // }

        // return result;
        throw new NotImplementedException();
    }

    private bool IsPageCorrect(List<int> update)
    {
        for (int i = 0; i < update.Count; i++)
        {
            var currentItem = update[i];

            // Check all items before and after the current index
            var isPreviousCorrect = update.Take(i).All(prev => _pageOrders.Contains((prev, currentItem)));
            var isFollowingCorrect = update.Skip(i + 1).All(next => _pageOrders.Contains((currentItem, next)));

            if (!isPreviousCorrect || !isFollowingCorrect)
            {
                return false;
            }
        }

        return true;
    }

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

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
    }
}
