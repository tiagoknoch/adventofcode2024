
using FileParser;

namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;


    public Day08()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(i => i.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private int Solve_1_v1()
    {
        // Step 1: Find all distinct characters in the matrix, excluding dots
        var distinctChars = FindAllDistinctChars();
        List<(int Row, int Col)> result = []; // List to store the hash positions

        // Step 2: Iterate through each distinct character
        foreach (var c in distinctChars)
        {
            // Step 3: Find all occurrences (positions) of this character in the matrix
            var occurrences = FindCharPositions(c);

            // Step 4: Get all pair permutations for the current character's positions
            var pairs = GetAllPairPermutations(occurrences);

            // Step 5: Draw hashes for each pair of coordinates
            foreach (var pair in pairs)
            {
                result.AddRange(GetHashPositionsForCharPair(pair.item1, pair.item2));
            }
        }
        var sortedPositions = result.Distinct().OrderBy(p => p.Row).ThenBy(p => p.Col).ToList();
        return sortedPositions.Count;
    }

    public List<((int Row, int Col) item1, (int Row, int Col) item2)> GetAllPairPermutations(List<(int Row, int Col)> positions)
    {
        var pairs = new List<((int Row, int Col) item1, (int Row, int Col) item2)>();

        // Generate all unique pairs of coordinates
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                // Create a pair with the two items
                pairs.Add((positions[i], positions[j]));
            }
        }

        return pairs;
    }

    public IEnumerable<(int Row, int Col)> GetHashPositionsForCharPair((int Row, int Col) item1, (int Row, int Col) item2)
    {
        var hashPositions = new List<(int Row, int Col)>(); // List to store the hash positions

        // Calculate the number of steps in row and column between the two items
        var (rowSteps, colSteps) = CalculateStepsBetweenPoints(item1, item2);

        // Determine direction from item1 to item2
        int rowDirection = Math.Sign(item2.Row - item1.Row);
        int colDirection = Math.Sign(item2.Col - item1.Col);

        // Calculate the position for the hash above item1 and below item2
        var hashPosition1 = (Row: item1.Row - rowDirection * rowSteps, Col: item1.Col - colDirection * colSteps);
        var hashPosition2 = (Row: item2.Row + rowDirection * rowSteps, Col: item2.Col + colDirection * colSteps);

        // Add the valid hash positions to the list if they are within bounds and on a dot ('.')
        if (IsPositionValid(hashPosition1))
        {
            hashPositions.Add(hashPosition1);
        }

        if (IsPositionValid(hashPosition2))
        {
            hashPositions.Add(hashPosition2);
        }

        return hashPositions;
    }

    // Helper method to check if a position is valid inside the matrix bounds
    private bool IsPositionValid((int Row, int Col) position)
    {
        return position.Row >= 0 && position.Row < _matrix.Length &&
               position.Col >= 0 && position.Col < _matrix[position.Row].Length;
    }


    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private object Solve_2_v1()
    {
        // Step 1: Find all distinct characters in the matrix, excluding dots
        var distinctChars = FindAllDistinctChars();
        List<(int Row, int Col)> result = []; // List to store the hash positions

        // Step 2: Iterate through each distinct character
        foreach (var c in distinctChars)
        {
            // Step 3: Find all occurrences (positions) of this character in the matrix
            var occurrences = FindCharPositions(c);

            // Step 4: Get all pair permutations for the current character's positions
            var pairs = GetAllPairPermutations(occurrences);

            // Step 5: Draw hashes for each pair of coordinates
            foreach (var pair in pairs)
            {
                result.AddRange(GetHashPositionsAlongLine(pair.item1, pair.item2));
            }
        }
        var sortedPositions = result.Distinct().OrderBy(p => p.Row).ThenBy(p => p.Col).ToList();
        return sortedPositions.Count;
    }

    public IEnumerable<(int Row, int Col)> GetHashPositionsAlongLine((int Row, int Col) item1, (int Row, int Col) item2)
    {
        var hashPositions = new List<(int Row, int Col)> { item1, item2 }; // List to store the hash positions

        // Calculate the number of steps in row and column between the two items
        var (rowSteps, colSteps) = CalculateStepsBetweenPoints(item1, item2);

        // Determine direction from item1 to item2
        int rowDirection = Math.Sign(item2.Row - item1.Row);
        int colDirection = Math.Sign(item2.Col - item1.Col);

        // Iterate backwards from item1, adding hash positions
        var currentPosition = item1;
        while (true)
        {
            var nextPosition = (
                Row: currentPosition.Row - rowDirection * rowSteps,
                Col: currentPosition.Col - colDirection * colSteps
            );

            if (!IsPositionValid(nextPosition))
            {
                break;
            }

            hashPositions.Add(nextPosition);
            currentPosition = nextPosition;
        }

        // Iterate forwards from item2, adding hash positions
        currentPosition = item2;
        while (true)
        {
            var nextPosition = (
                Row: currentPosition.Row + rowDirection * rowSteps,
                Col: currentPosition.Col + colDirection * colSteps
            );

            if (!IsPositionValid(nextPosition))
            {
                break;
            }

            hashPositions.Add(nextPosition);
            currentPosition = nextPosition;
        }

        return hashPositions.Distinct();
    }

    public List<(int Row, int Col)> FindCharPositions(char charToLookFor)
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

    public IEnumerable<char> FindAllDistinctChars()
    {
        var distinctChars = new HashSet<char>(); // Using HashSet to automatically avoid duplicates

        for (int row = 0; row < _matrix.Length; row++)
        {
            for (int col = 0; col < _matrix[row].Length; col++)
            {
                var charAtPos = _matrix[row][col];
                if (charAtPos != '.')  // Skip dots
                {
                    distinctChars.Add(charAtPos);
                }
            }
        }

        return distinctChars; // Convert the HashSet to a List if needed
    }


    public (int rowSteps, int colSteps) CalculateStepsBetweenPoints((int Row, int Col) item1, (int Row, int Col) item2)
    {
        // Calculate the number of steps in the row direction (absolute difference)
        int rowSteps = Math.Abs(item2.Row - item1.Row);

        // Calculate the number of steps in the column direction (absolute difference)
        int colSteps = Math.Abs(item2.Col - item1.Col);

        // Return the result as a tuple (rowSteps, colSteps)
        return (rowSteps, colSteps);
    }
}
