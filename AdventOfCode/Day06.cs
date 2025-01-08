using FileParser;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;

    private enum DirectionEnum
    {
        North,
        East,
        South,
        West
    }

    public Day06()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(i => i.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");
    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");


    private int Solve_1_v1()
    {
        var path = NavigateWithGuard();
        return path.Count;
    }
    private int Solve_2_v1()
    {
        var path = NavigateWithGuard();
        var loops = CountLoopsFromObstacles(path.Skip(1).ToList());

        return loops;
    }

    private int CountLoopsFromObstacles(List<(int Row, int Col)> distinctPositions)
    {
        int loopCount = 0;

        foreach (var position in distinctPositions)
        {
            // Backup the original character at the position
            var originalChar = _matrix[position.Row][position.Col];

            // Place a temporary obstacle
            _matrix[position.Row][position.Col] = '#';

            // Execute the navigation function with loop detection
            if (NavigateAndDetectLoop())
            {
                loopCount++; // Increment the loop count if a loop is detected
            }

            // Restore the original character
            _matrix[position.Row][position.Col] = originalChar;
        }

        return loopCount;
    }

    private bool NavigateAndDetectLoop()
    {
        var guardCoords = FindFirstChar('^');
        var direction = DirectionEnum.North;
        var currentCoords = guardCoords;

        // Use a HashSet to track visited states (coordinate + direction)
        var visitedStates = new HashSet<((int Row, int Col) Coords, DirectionEnum Direction)>();

        while (currentCoords.Row >= 0 && currentCoords.Row < _matrix.Length &&
               currentCoords.Col >= 0 && currentCoords.Col < _matrix[currentCoords.Row].Length)
        {
            // Create the current state (coordinate + direction)
            var currentState = (currentCoords, direction);

            // Check if this state has already been visited
            if (!visitedStates.Add(currentState))
            {
                // A loop is detected if the same state is revisited
                return true;
            }

            var nextCoords = GetNewCoords(currentCoords, direction);

            // Check if the nextCoords are out of bounds
            if (nextCoords.Row < 0 || nextCoords.Row >= _matrix.Length ||
                nextCoords.Col < 0 || nextCoords.Col >= _matrix[nextCoords.Row].Length)
            {
                // Exit the loop if nextCoords are outside the matrix
                break;
            }

            var nextChar = _matrix[nextCoords.Row][nextCoords.Col];

            if (nextChar == '#')
            {
                direction = GetNewDirection(direction); // Change direction if there's an obstacle
            }
            else
            {
                currentCoords = nextCoords; // Move to the next position
            }
        }

        return false; // No loop was detected
    }

    private List<(int Row, int Col)> NavigateWithGuard()
    {
        var guardCoords = FindFirstChar('^');
        var direction = DirectionEnum.North;
        var currentCoords = guardCoords;

        var path = new List<(int Row, int Col)>(); // Store the path of visited positions

        while (currentCoords.Row >= 0 && currentCoords.Row < _matrix.Length &&
               currentCoords.Col >= 0 && currentCoords.Col < _matrix[currentCoords.Row].Length)
        {
            path.Add(currentCoords); // Add the current position to the path

            var nextCoords = GetNewCoords(currentCoords, direction);

            // Check if the nextCoords are out of bounds
            if (nextCoords.Row < 0 || nextCoords.Row >= _matrix.Length ||
                nextCoords.Col < 0 || nextCoords.Col >= _matrix[nextCoords.Row].Length)
            {
                // Exit the loop if nextCoords are outside the matrix
                break;
            }

            var nextChar = _matrix[nextCoords.Row][nextCoords.Col];

            if (nextChar == '#')
            {
                direction = GetNewDirection(direction);
            }
            else
            {
                currentCoords = nextCoords; // Move to the next position
            }
        }


        var distinctPositions = path.Distinct().ToList();
        return distinctPositions;
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

    private (int Row, int Col) GetNewCoords((int Row, int Col) currentCoords, DirectionEnum direction)
    {
        return direction switch
        {
            DirectionEnum.North => (currentCoords.Row - 1, currentCoords.Col),
            DirectionEnum.East => (currentCoords.Row, currentCoords.Col + 1),
            DirectionEnum.South => (currentCoords.Row + 1, currentCoords.Col),
            DirectionEnum.West => (currentCoords.Row, currentCoords.Col - 1),
            _ => throw new InvalidOperationException("Invalid direction")
        };
    }

    private DirectionEnum GetNewDirection(DirectionEnum currentDirection)
    {
        return currentDirection switch
        {
            DirectionEnum.North => DirectionEnum.East,
            DirectionEnum.South => DirectionEnum.West,
            DirectionEnum.West => DirectionEnum.North,
            DirectionEnum.East => DirectionEnum.South,
            _ => throw new InvalidOperationException("Invalid direction"),
        };
    }
}
