using FileParser;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<Puzzle> _list = [];

    record Puzzle(long A_X, long A_Y, long B_X, long B_Y, long Prize_X, long Prize_Y);

    public Day13()
    {
        _input = new ParsedFile(InputFilePath);
        while (!_input.Empty)
        {
            var lineA = _input.NextLine();
            var lineB = _input.NextLine();
            var lineResult = _input.NextLine();
            Puzzle puzzle = new(
                long.Parse(lineA.ElementAt<string>(2).Split('+')[1].TrimEnd(',')),
                long.Parse(lineA.ElementAt<string>(3).Split('+')[1].TrimEnd(',')),
                long.Parse(lineB.ElementAt<string>(2).Split('+')[1].TrimEnd(',')),
                long.Parse(lineB.ElementAt<string>(3).Split('+')[1].TrimEnd(',')),
                long.Parse(lineResult.ElementAt<string>(1).Split('=')[1].TrimEnd(',')),
                long.Parse(lineResult.ElementAt<string>(2).Split('=')[1].TrimEnd(','))
            );

            _list.Add(puzzle);
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private object Solve_1_v1()
    {
        return _list.Sum(puzzle =>
        {
            var solutions = FindCommonSolutions(puzzle);
            if (!solutions.Any()) return 0;

            var (gold, silver) = solutions.First();
            return gold * 3 + silver * 1;
        });
    }

    private static IEnumerable<(long, long)> FindCommonSolutions(Puzzle puzzle)
    {
        var xSolutions = FindAllMultipliers(puzzle.Prize_X, puzzle.A_X, puzzle.B_X);
        var ySolutions = FindAllMultipliers(puzzle.Prize_Y, puzzle.A_Y, puzzle.B_Y);

        return xSolutions.Intersect(ySolutions);
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();


    public static List<(long, long)> FindAllMultipliers(long target, long coeffA, long coeffB)
    {
        var solutions = new List<(long, long)>();
        var maxX = target / coeffA;

        for (long x = 0; x <= maxX; x++)
        {
            var remainder = target - coeffA * x;
            if (remainder % coeffB != 0) continue;

            var y = remainder / coeffB;
            if (y >= 0)
            {
                solutions.Add((x, y));
            }
        }

        return solutions;
    }
}
