using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day3 : BaseDay
{
    private readonly string _input;

    public Day3()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v2()}");

    private int Solve_1_v1()
    {
        string pattern = @"mul\((-?\d{1,3}),(-?\d{1,3})\)";
        var matches = Regex.Matches(_input, pattern);

        var result = matches
            .Select(match =>
            {
                var parsedString = match.Value;
                parsedString = parsedString.Replace("mul(", "");
                return parsedString.TrimEnd(')').Split(',').Select(int.Parse);
            }
                )
            .Select(values => values.ElementAt(0) * values.ElementAt(1))
            .Sum();

        return result;
    }

    private int Solve_1_v2()
    {
        string pattern = @"mul\((-?\d{1,3}),(-?\d{1,3})\)";
        var matches = Regex.Matches(_input, pattern);

        // Extract and process matches to compute the sum of products
        return matches
            .Select(match =>
            {
                var groups = match.Groups;
                int x = int.Parse(groups[1].Value); // Extract first number
                int y = int.Parse(groups[2].Value); // Extract second number
                return x * y; // Compute product
            })
            .Sum(); // Sum all products
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();
}
