using System.Windows.Markup;
using Microsoft.VisualBasic;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v2()}");

    public override ValueTask<string> Solve_2() => new($"{Solve_2_v2()}");

    public int Solve_2_v1()
    {
        // Split input into lines and parse into integer lists
        var lines = _input.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var values = lines
            .Select(line => line.Split([' '], StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray())
            .ToList();

        // Extract and sort the first and second elements
        var firstItems = values.Select(v => v[0]).OrderBy(x => x).ToList();
        var secondItems = values.Select(v => v[1]).OrderBy(x => x).ToList();

        var scores = new List<int>();

        foreach (var item in firstItems)
        {
            var count = secondItems.Count(i => i == item);
            var score = item * count;
            scores.Add(score);
        }

        return scores.Sum();
    }

    public int Solve_2_v2()
    {
        // Parse input into a list of integer pairs
        var values = _input
            .Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split([' '], StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray())
            .ToList();

        // Extract and sort the first and second elements
        var firstItems = values.Select(v => v[0]).OrderBy(x => x);
        var secondItems = values.Select(v => v[1]);

        // Create a dictionary to count occurrences of each number in secondItems
        var secondItemCounts = secondItems.GroupBy(x => x)
                                          .ToDictionary(g => g.Key, g => g.Count());

        // Compute the total score
        var totalScore = firstItems
            .Where(item => secondItemCounts.ContainsKey(item)) // Only consider items present in secondItems
            .Sum(item => item * secondItemCounts[item]);

        return totalScore;
    }

    public int Solve_1_v1()
    {
        var lines = _input.Split([Environment.NewLine],
          StringSplitOptions.RemoveEmptyEntries);
        var values = lines.Select(i =>
        i.Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
    .ToList();

        var firstItems = values.Select(item => item[0]).ToList();
        var secondItems = values.Select(item => item[1]).ToList();

        firstItems.Sort();
        secondItems.Sort();

        var differences = new List<int>();
        for (int i = 0; i < firstItems.Count; i++)
        {
            var item1 = firstItems[i];
            var item2 = secondItems[i];

            var difference = Math.Abs(item1 - item2);
            differences.Add(difference);
        }

        var sum = differences.Sum();
        return sum;
    }

    public int Solve_1_v2()
    {
        // Split input into lines and parse into integer lists
        var lines = _input.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
        var values = lines
            .Select(line => line.Split([' '], StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray())
            .ToList();

        // Extract and sort the first and second elements
        var firstItems = values.Select(v => v[0]).OrderBy(x => x).ToList();
        var secondItems = values.Select(v => v[1]).OrderBy(x => x).ToList();

        // Calculate the sum of absolute differences
        var sum = firstItems
            .Zip(secondItems, (first, second) => Math.Abs(first - second))
            .Sum();

        return sum;
    }
}
