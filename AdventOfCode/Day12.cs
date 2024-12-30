using FileParser;

namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _matrix;

    public Day12()
    {
        _input = new ParsedFile(InputFilePath);
        _matrix = _input.ToList<string>().Select(i => i.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");
    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

    private int Solve_1_v1()
    {
        var result = 0;
        var regions = FindRegions(_matrix);

        foreach (var region in regions)
        {
            int freeSides = CountFreeSides(_matrix, region);
            Console.WriteLine($"Region {_matrix[region[0].Row][region[0].Col]}, Area {region.Count}, Perimeter {freeSides}");
            result += region.Count * freeSides;
        }

        return result;
    }


    public static List<List<(int Row, int Col)>> FindRegions(char[][] matrix)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;
        bool[,] visited = new bool[rows, cols];
        List<List<(int Row, int Col)>> regions = [];

        // Process each cell
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (!visited[row, col])
                {
                    // Found an unvisited cell, start a new region search
                    List<(int Row, int Col)> region = [];
                    char currentChar = matrix[row][col];
                    DFS(matrix, visited, row, col, currentChar, region);
                    regions.Add(region);
                }
            }
        }

        return regions;
    }

    public static int CountFreeSides(char[][] matrix, List<(int Row, int Col)> region)
    {
        int freeSides = 0;

        foreach (var (row, col) in region)
        {
            // For each cell, check its four neighbors
            foreach (var (dr, dc) in Directions)
            {
                int newRow = row + dr;
                int newCol = col + dc;

                // Check if the neighbor is out of bounds (edge of the matrix)
                if (newRow < 0 || newRow >= matrix.Length || newCol < 0 || newCol >= matrix[0].Length)
                {
                    freeSides++;
                }
                else if (matrix[row][col] != matrix[newRow][newCol])
                {
                    // The neighbor is a different character, so this side is free
                    freeSides++;
                }
            }
        }

        return freeSides;
    }

    private static void DFS(char[][] matrix, bool[,] visited, int row, int col, char currentChar, List<(int Row, int Col)> region)
    {
        int rows = matrix.Length;
        int cols = matrix[0].Length;

        // If out of bounds or already visited or not the same character, return
        if (row < 0 || row >= rows || col < 0 || col >= cols || visited[row, col] || matrix[row][col] != currentChar)
        {
            return;
        }

        // Mark this cell as visited
        visited[row, col] = true;

        // Add the current coordinate to the region
        region.Add((row, col));

        // Explore all four possible directions (up, down, left, right)
        foreach (var (dr, dc) in Directions)
        {
            DFS(matrix, visited, row + dr, col + dc, currentChar, region);
        }
    }


    private static readonly (int Row, int Col)[] Directions =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1)  // up, down, left, right
    ];
}
