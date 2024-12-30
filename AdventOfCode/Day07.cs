using FileParser;

namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<(long result, List<long> numbers)> _lines = [];


    public Day07()
    {
        _input = new ParsedFile(InputFilePath);
        while (!_input.Empty)
        {

            var test = _input.NextLine();
            var result = long.Parse(test.NextElement<string>().Trim(':'));
            var numbers = test.ToList<long>();
            _lines.Add((result, numbers));
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");
    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private object Solve_1_v1()
    {
        long result = 0;
        foreach (var line in _lines)
        {
            var results = GenerateExpressions(line.numbers, ['+', '*']);
            if (results.Any(x => x == line.result))
            {
                result += line.result;
            }
        }
        return result;
    }

    private object Solve_2_v1()
    {
        long result = 0;
        foreach (var line in _lines)
        {
            var results = GenerateExpressions(line.numbers, ['+', '*', '|']);
            if (results.Any(x => x == line.result))
            {
                result += line.result;
            }
        }
        return result;
    }

    private List<long> GenerateExpressions(List<long> numbers, char[] operations)
    {
        var results = new List<long>();

        // Helper function to recursively build expressions
        void Generate(int index, long result)
        {
            if (index == numbers.Count - 1)
            {
                results.Add(result);
                return;
            }

            foreach (var operation in operations)
            {
                if (operation == '+')
                {
                    Generate(index + 1, result + numbers[index + 1]);
                }
                if (operation == '*')
                {
                    Generate(index + 1, result * numbers[index + 1]);
                }
                if (operation == '|')
                {
                    Generate(index + 1, long.Parse($"{result}{numbers[index + 1]}"));
                }
            }
        }

        // Start the recursion with the first number
        Generate(0, numbers[0]);

        return results;
    }


}
