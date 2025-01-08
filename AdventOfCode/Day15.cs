using System.Data;
using FileParser;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;

    private readonly char[] _movements;

    public Day15()
    {
        _input = new ParsedFile(InputFilePath);
        var lineTemp = _input.ToList<string>();
        var lines = lineTemp.Select(i => i.ToCharArray()).ToArray();
        _movements = lines.Where(i => i.First() != '#').SelectMany(arr => arr).ToArray();
        _matrix = lines.Where(i => i.First() == '#').ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

    private int Solve_1_v1()
    {
        //PrintMatrix();
        var robotCoordinates = FindFirstChar('@');
        foreach (var movement in _movements)
        {
            robotCoordinates = ProcessMovement(robotCoordinates, movement);
            // Console.WriteLine($"----------------------------");
            // Console.WriteLine($"Movement: {movement}");
            // PrintMatrix();
        }
        var boxesCoords = FindCharPositions('O');
        var result = boxesCoords.Sum(coord => 100 * coord.Row + coord.Col);

        return result;
    }

    private (int Row, int Col) ProcessMovement((int Row, int Col) robotCoordinates, char movement)
    {
        var directionOffsets = movement switch
        {
            '^' => (-1, 0),
            '>' => (0, 1),
            '<' => (0, -1),
            'v' => (1, 0),
            _ => throw new InvalidOperationException("Invalid Movement"),
        };

        return ProcessDirection(robotCoordinates, directionOffsets);
    }

    private (int Row, int Col) ProcessDirection((int Row, int Col) robotCoordinates, (int dRow, int dCol) direction)
    {
        var newCoordinates = (Row: robotCoordinates.Row + direction.dRow, Col: robotCoordinates.Col + direction.dCol);
        var target = _matrix[newCoordinates.Row][newCoordinates.Col];

        if (target == '#')
        {
            return robotCoordinates; // Blocked, stay in place
        }
        else if (target == '.')
        {
            MoveChar(robotCoordinates, newCoordinates);
            return newCoordinates;
        }
        else if (target == 'O')
        {
            var tempCoordinates = ProcessDirection(newCoordinates, direction);
            if (tempCoordinates != newCoordinates)
            {
                // The object moved, so we can move
                MoveChar(robotCoordinates, newCoordinates);
                return newCoordinates;
            }
            else
            {
                // The object didn't move, so we also don't move
                return robotCoordinates;
            }
        }

        throw new InvalidOperationException("Invalid Processing Target");
    }


    private (int Row, int Col) FindFirstChar(char target)
    {
        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                if (_matrix[row][col] == target)
                {
                    return (row, col);
                }
            }
        }
        throw new InvalidOperationException("Target not found");
    }

    private void MoveChar((int Row, int Col) oldCoordinate, (int Row, int Col) newCoordinate)
    {
        var tempChar = _matrix[oldCoordinate.Row][oldCoordinate.Col];
        _matrix[oldCoordinate.Row][oldCoordinate.Col] = '.';
        _matrix[newCoordinate.Row][newCoordinate.Col] = tempChar;
    }

    private void PrintMatrix()
    {
        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                Console.Write(_matrix[row][col]);
            }
            Console.WriteLine();
        }
    }

    private List<(int Row, int Col)> FindCharPositions(char charToLookFor)
    {
        var positions = new List<(int Row, int Col)>();

        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                if (_matrix[row][col] == charToLookFor)
                {
                    positions.Add((row, col));
                }
            }
        }

        return positions;
    }
}
