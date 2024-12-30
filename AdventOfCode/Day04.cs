using FileParser;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;

    public Day04()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(i => i.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");
    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private int Solve_1_v1()
    {
        var result = 0;
        for (int row = 0; row < _matrix.Length; row++) // Iterate rows
        {
            for (int col = 0; col < _matrix[row].Length; col++) // Iterate columns in each row
            {
                if (_matrix[row][col] == 'X')
                {
                    // Retrieve all possible strings starting at this coordinate
                    var possibleStrings = GetStringsFromCoordinate(row, col);

                    // Count valid matches
                    result += possibleStrings.Count(str => str is "XMAS" or "SAMX");
                }
            }
        }

        return result;
    }

    private int Solve_2_v1()
    {
        var result = 0;
        for (int row = 0; row < _matrix.Length; row++) // Iterate rows
        {
            for (int col = 0; col < _matrix[row].Length; col++) // Iterate columns in each row
            {
                if (_matrix[row][col] == 'A')
                {
                    // Retrieve all possible strings starting at this coordinate
                    var possibleStrings = GetXStringsFromCoordinate(row, col);
                    if (possibleStrings.Length == 2 && possibleStrings.All(str => str == "MAS" || str == "SAM"))
                    {
                        result += 1;
                    }
                }
            }
        }

        return result;
    }


    public IEnumerable<string> GetStringsFromCoordinate(int row, int col, int length = 4)
    {
        var directions = new List<string>();

        // Up (vertical)
        if (row >= length - 1)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row - i][col]).ToArray()));

        // Down (vertical)
        if (row + length <= _matrix.Length)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row + i][col]).ToArray()));

        // Left (horizontal)
        if (col >= length - 1)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row][col - i]).ToArray()));

        // Right (horizontal)
        if (col + length <= _matrix[row].Length)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row][col + i]).ToArray()));

        // Diagonal Up-Left
        if (row >= length - 1 && col >= length - 1)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row - i][col - i]).ToArray()));

        // Diagonal Up-Right
        if (row >= length - 1 && col + length <= _matrix[row].Length)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row - i][col + i]).ToArray()));

        // Diagonal Down-Left
        if (row + length <= _matrix.Length && col >= length - 1)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row + i][col - i]).ToArray()));

        // Diagonal Down-Right
        if (row + length <= _matrix.Length && col + length <= _matrix[row].Length)
            directions.Add(new string(Enumerable.Range(0, length).Select(i => _matrix[row + i][col + i]).ToArray()));

        return directions;
    }

    private string[] GetXStringsFromCoordinate(int row, int col)
    {
        // Validate bounds for the matrix
        if (row < 1 || row >= _matrix.Length - 1 || col < 1 || col >= _matrix[row].Length - 1)
            return []; // Return empty if not enough space for an X.

        // Upper-left to bottom-right diagonal (e.g., top-left, center, bottom-right)
        var diagonal1 = new string(
        [
        _matrix[row - 1][col - 1], // Top-left
        _matrix[row][col],         // Center
        _matrix[row + 1][col + 1]  // Bottom-right
    ]);

        // Upper-right to bottom-left diagonal (e.g., top-right, center, bottom-left)
        var diagonal2 = new string(
        [
        _matrix[row - 1][col + 1], // Top-right
        _matrix[row][col],         // Center
        _matrix[row + 1][col - 1]  // Bottom-left
    ]);

        return [diagonal1, diagonal2];
    }
}
