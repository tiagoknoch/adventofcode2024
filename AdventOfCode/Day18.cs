using System.Text;
using FileParser;

namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _grid;

    private readonly List<(int x, int y)> _bytes;
    private readonly int _rows = 70 + 1;
    private readonly int _cols = 70 + 1;
    private static readonly (int Row, int Col)[] Directions =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1) // Up, Down, Left, Right
    ];

    public Day18()
    {
        _input = new ParsedFile(InputFilePath);
        var lineTemp = _input.ToList<string>();
        var items = lineTemp.Select(i => i.Split(',')
                        .Select(int.Parse)
                        .ToList());

        _bytes = items.Select(i => (i[0], i[1])).ToList();

        //fill grid with .
        _grid = new char[_rows][];
        for (int i = 0; i < _rows; i++)
        {
            _grid[i] = new char[_cols];
            for (int j = 0; j < _cols; j++)
            {
                _grid[i][j] = '.';
            }
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private int Solve_1_v1()
    {
        for (int i = 0; i < 1024; i++)
        {
            _grid[_bytes[i].y][_bytes[i].x] = '#';
        }
        PrintGrid();

        var start = (0, 0); // Top-left
        var goal = (70, 70); // Bottom-right

        int cost = FindShortestPath(_grid, start, goal);
        return cost;
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

    public static int FindShortestPath(char[][] grid, (int Row, int Col) start, (int Row, int Col) goal)
    {
        int rows = grid.Length, cols = grid[0].Length;

        var openSet = new PriorityQueue<(int Row, int Col), int>();
        var gCost = new Dictionary<(int Row, int Col), int>(); // Cost from start to this node
        var fCost = new Dictionary<(int Row, int Col), int>(); // Estimated total cost

        openSet.Enqueue(start, 0);
        gCost[start] = 0;
        fCost[start] = ManhattanDistance(start, goal);

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();
            if (current == goal)
                return gCost[current]; // Return shortest path cost

            foreach (var (dRow, dCol) in Directions)
            {
                var neighbor = (current.Row + dRow, current.Col + dCol);
                if (!IsValidCell(neighbor, grid, rows, cols)) continue;

                int tentativeG = gCost[current] + 1; // Cost to move to neighbor

                if (!gCost.ContainsKey(neighbor) || tentativeG < gCost[neighbor])
                {
                    gCost[neighbor] = tentativeG;
                    fCost[neighbor] = tentativeG + ManhattanDistance(neighbor, goal);
                    openSet.Enqueue(neighbor, fCost[neighbor]);
                }
            }
        }

        return -1; // No path found
    }

    private static int ManhattanDistance((int Row, int Col) a, (int Row, int Col) b)
    {
        return Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);
    }

    private static bool IsValidCell((int Row, int Col) cell, char[][] grid, int rows, int cols)
    {
        return cell.Row >= 0 && cell.Row < rows &&
               cell.Col >= 0 && cell.Col < cols &&
               grid[cell.Row][cell.Col] != '#';
    }

    private void PrintGrid()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                Console.Write(_grid[row][col]);
            }
            Console.WriteLine();
        }
    }
}
