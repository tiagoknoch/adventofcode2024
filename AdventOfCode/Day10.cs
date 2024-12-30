using FileParser;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly int[][] _matrix;

    public Day10()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(str => str.Select(c => c - '0')
                     .ToArray())
                    .ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private long Solve_1_v1()
    {
        var zeroPositions = FindItemPositions(0);
        long trailCount = 0;

        foreach (var i in zeroPositions)
        {
            var trails = FindTrailheadScore(i);
            trailCount += trails;
        }
        return trailCount;
    }


    private int FindTrailheadScore((int Row, int Col) startPosition)
    {
        var allTrails = new List<List<(int Row, int Col)>>(); // List to store all valid trails
        FollowTrail(startPosition, [startPosition], allTrails);
        var lastItems = allTrails.Select(trail => trail.Last()).ToList();
        return lastItems.Distinct().Count();
    }

    private void FollowTrail((int Row, int Col) currentPos, List<(int Row, int Col)> positions, List<List<(int Row, int Col)>> allTrails)
    {
        var item = _matrix[currentPos.Row][currentPos.Col];

        // Base case: if we reach the item 9, add the current trail to allTrails
        if (item == 9)
        {
            allTrails.Add([.. positions]);  // Add a copy of the current trail
            return;  // Stop recursion
        }

        // Find the next positions with the expected value (item + 1)
        var nextPositions = FindItemPositions(currentPos, item + 1);

        // If no valid next positions are found, return (no trails)
        if (nextPositions.Count == 0)
        {
            return;
        }

        // Recursively process each next position
        foreach (var nextPos in nextPositions)
        {
            // Create a new trail list by copying the current trail and adding the next position
            var newTrail = new List<(int Row, int Col)>(positions) { nextPos };  // Copy the list and add the new position

            // Recurse to find further trails from this next position
            FollowTrail(nextPos, newTrail, allTrails);
        }
    }

    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private long Solve_2_v1()
    {
        var zeroPositions = FindItemPositions(0);
        long trailCount = 0;

        foreach (var i in zeroPositions)
        {
            var trails = FindTrailheads(i);
            trailCount += trails;
        }
        return trailCount;
    }

    private int FindTrailheads((int Row, int Col) startPosition)
    {
        var allTrails = new List<List<(int Row, int Col)>>(); // List to store all valid trails
        FollowTrail(startPosition, [startPosition], allTrails);
        return allTrails.Count();
    }

    public List<(int Row, int Col)> FindItemPositions(int item)
    {
        var positions = new List<(int Row, int Col)>();

        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                if (_matrix[row][col] == item)
                {
                    positions.Add((row, col));
                }
            }
        }

        return positions;
    }

    private List<(int Row, int Col)> FindItemPositions((int Row, int Col) coordinate, int item)
    {
        var positions = new List<(int Row, int Col)>();
        var directions = new (int dRow, int dCol)[]
        {
        (1, 0), // down
        (-1, 0), // up
        (0, 1), // right
        (0, -1)  // left
        };

        // Loop through all directions
        foreach (var (dRow, dCol) in directions)
        {
            var newRow = coordinate.Row + dRow;
            var newCol = coordinate.Col + dCol;

            // Check if the new position is within bounds and matches the item
            if (IsValidPosition(newRow, newCol) && _matrix[newRow][newCol] == item)
            {
                positions.Add((newRow, newCol));
            }
        }

        return positions;
    }

    // Helper function to check if the position is within bounds
    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _matrix.Length && col >= 0 && col < _matrix[0].Length;
    }
}
