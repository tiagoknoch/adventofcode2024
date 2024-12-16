using FileParser;

namespace AdventOfCode;

public class Day4 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;

    public Day4()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(i => i.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v2()}");

    private int Solve_1_v2()
    {
        var result = 0;
        for (int row = 0; row < _matrix.Length; row++) // Iterate rows
        {
            for (int col = 0; col < _matrix[row].Length; col++) // Iterate columns in each row
            {
                char item = _matrix[row][col];
                if (item == 'X')
                {
                    var possibleStrings = GetStringsFromCoordinate(row, col);
                    result += possibleStrings.Count(i => i == "XMAS" || i == "SAMX");
                }
            }
        }

        return result;
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

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
}
