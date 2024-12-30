using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v2()}");
    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

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

    private int Solve_2_v1()
    {
        string pattern = @"(mul\((-?\d{1,3}),(-?\d{1,3})\))|(do\(\))|(don't\(\))";
        var matches = Regex.Matches(_input, pattern);
        var skip = false;
        var result = 0;

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success && !skip)
            {
                // Handle mul(x, y)
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                result += x * y;
            }
            else if (match.Groups[4].Success)
            {
                // Handle do()
                skip = false;
            }
            else if (match.Groups[5].Success)
            {
                // Handle don't()
                skip = true;
            }
        }

        return result;
    }

}
