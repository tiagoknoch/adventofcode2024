using FileParser;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<Robot> _robots = [];
    private readonly int X_Length = 101;
    private readonly int Y_Length = 103;

    record Robot((int X, int Y) Position, (int X, int Y) Velicity);

    public Day14()
    {
        _input = new ParsedFile(InputFilePath);
        while (!_input.Empty)
        {
            var line = _input.NextLine();
            var position = line.NextElement<string>().Replace("p=", "").Split(',');
            var velocity = line.NextElement<string>().Replace("v=", "").Split(',');

            _robots.Add(new Robot((int.Parse(position[0]), int.Parse(position[1])), (int.Parse(velocity[0]), int.Parse(velocity[1]))));
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private int Solve_1_v1()
    {
        var iterations = 100;

        var newPositions = _robots.Select(robot => IterateRobot(robot.Position, robot.Velicity, iterations)).ToList();

        //PrintGrid(newPositions);

        decimal quadrantX = (decimal)X_Length / 2;
        decimal quadrantY = (decimal)Y_Length / 2;
        var total1 = GetRobosInQuadrant(newPositions, 0, 0, (int)Math.Floor(quadrantX), (int)Math.Floor(quadrantY));
        var total2 = GetRobosInQuadrant(newPositions, (int)Math.Ceiling(quadrantX), 0, X_Length, (int)Math.Floor(quadrantY));
        var total3 = GetRobosInQuadrant(newPositions, 0, (int)Math.Ceiling(quadrantY), (int)Math.Floor(quadrantX), Y_Length);
        var total4 = GetRobosInQuadrant(newPositions, (int)Math.Ceiling(quadrantX), (int)Math.Ceiling(quadrantY), X_Length, Y_Length);

        return total1 * total2 * total3 * total4;
    }

    void PrintGrid(List<(int X, int Y)> newPositions)
    {
        for (int i = 0; i < Y_Length; i++)
        {
            for (int j = 0; j < X_Length; j++)
            {
                if (newPositions.Any(position => position.X == j && position.Y == i))
                {
                    Console.Write(newPositions.Count(position => position.X == j && position.Y == i));
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    private int GetRobosInQuadrant(List<(int X, int Y)> newPositions, int v1, int v2, int x_Length, int y_Length)
    {
        return newPositions.Count(position => position.X >= v1 && position.X < x_Length && position.Y >= v2 && position.Y < y_Length);
    }

    private (int X, int Y) IterateRobot((int X, int Y) position, (int X, int Y) velicity, int iterations)
    {
        var newX = position.X + (velicity.X * iterations);
        var newY = position.Y + (velicity.Y * iterations);
        newX = newX % X_Length;
        newY = newY % Y_Length;

        if (newX < 0)
        {
            newX = X_Length + newX;
        }
        if (newY < 0)
        {
            newY = Y_Length + newY;
        }


        return (newX, newY);
    }

    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private int Solve_2_v1()
    {
        throw new NotImplementedException();
    }
}
