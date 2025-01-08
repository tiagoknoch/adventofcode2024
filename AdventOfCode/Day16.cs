using System.Text;
using FileParser;

namespace AdventOfCode;

public class Day16 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly char[][] _grid;
    private readonly int _rows;
    private readonly int _cols;
    private readonly (int dr, int dc)[] _directions = [(-1, 0), (0, 1), (1, 0), (0, -1)]; // Up, Right, Down, Left
    private const int ROTATION_COST = 1000;
    private const int MOVEMENT_COST = 1;

    public Day16()
    {
        _input = new ParsedFile(InputFilePath);
        var lineTemp = _input.ToList<string>();
        _grid = lineTemp.Select(i => i.ToCharArray()).ToArray();
        _rows = _grid.Length;
        _cols = _grid[0].Length;
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private object Solve_1_v1()
    {
        var optimalPaths = FindAllOptimalPaths();

        Console.WriteLine($"Found {optimalPaths.Count} optimal paths with score {optimalPaths[0].score}:");
        return optimalPaths.First().score;
    }

    private object Solve_2_v1()
    {
        //NOTE: for some reason the result is not valid but I dont know why
        var optimalPaths = FindAllOptimalPaths();

        Console.WriteLine($"Found {optimalPaths.Count} optimal paths with score {optimalPaths[0].score}:");

        var totalCount = optimalPaths.SelectMany(i => i.path).Distinct().Count();


        return totalCount;
    }

    public override ValueTask<string> Solve_2() => new($"{Solve_2_v1()}");

    private class Node
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Direction { get; set; } // 0=Up, 1=Right, 2=Down, 3=Left
        public int G { get; set; } // Cost from start
        public int H { get; set; } // Heuristic cost to end
        public int F => G + H; // Total cost
        public Node Parent { get; set; }

        public Node(int row, int col, int direction)
        {
            Row = row;
            Col = col;
            Direction = direction;
        }
    }

    public List<(List<(int row, int col)> path, int score)> FindAllOptimalPaths()
    {
        var optimalPaths = new List<(List<(int row, int col)> path, int score)>();
        int bestScore = int.MaxValue;

        (int startRow, int startCol) = FindPosition('S');
        (int endRow, int endCol) = FindPosition('E');

        var queue = new Queue<Node>();
        var costMap = new Dictionary<(int row, int col, int dir), int>();

        // Initialize with all possible directions
        for (int dir = 0; dir < 4; dir++)
        {
            var startNode = new Node(startRow, startCol, dir)
            {
                G = ROTATION_COST, // Initial rotation cost
                H = CalculateHeuristic(startRow, startCol, endRow, endCol)
            };
            queue.Enqueue(startNode);
            costMap[(startRow, startCol, dir)] = startNode.G;
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // If we've found some paths and this one can't be better, skip it
            if (bestScore != int.MaxValue && current.G > bestScore)
                continue;

            // If we reached the end
            if (current.Row == endRow && current.Col == endCol)
            {
                if (current.G <= bestScore)
                {
                    if (current.G < bestScore)
                    {
                        optimalPaths.Clear();
                        bestScore = current.G;
                    }
                    optimalPaths.Add((ReconstructPath(current), current.G));
                }
                continue;
            }

            // Try all possible moves
            for (int newDir = 0; newDir < 4; newDir++)
            {
                int newRow = current.Row + _directions[newDir].dr;
                int newCol = current.Col + _directions[newDir].dc;

                if (!IsValidPosition(newRow, newCol))
                    continue;

                int movementCost = MOVEMENT_COST;
                if (newDir != current.Direction)
                {
                    movementCost += ROTATION_COST;
                }

                int newCost = current.G + movementCost;

                // Key change: Allow equal-cost paths
                var key = (newRow, newCol, newDir);
                if (!costMap.ContainsKey(key) || newCost <= costMap[key])
                {
                    var neighbor = new Node(newRow, newCol, newDir)
                    {
                        G = newCost,
                        H = CalculateHeuristic(newRow, newCol, endRow, endCol),
                        Parent = current
                    };

                    costMap[key] = newCost;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return optimalPaths;
    }

    private (int row, int col) FindPosition(char target)
    {
        for (int i = 0; i < _rows; i++)
            for (int j = 0; j < _cols; j++)
                if (_grid[i][j] == target)
                    return (i, j);
        throw new Exception($"Character {target} not found in grid");
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _rows &&
               col >= 0 && col < _cols &&
               _grid[row][col] != '#';
    }

    private int CalculateHeuristic(int row1, int col1, int row2, int col2)
    {
        return Math.Abs(row1 - row2) + Math.Abs(col1 - col2);
    }

    private List<(int row, int col)> ReconstructPath(Node endNode)
    {
        var path = new List<(int row, int col)>();
        var current = endNode;

        while (current != null)
        {
            path.Add((current.Row, current.Col));
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }

    public string VisualizePathInGrid(List<(int row, int col)> path)
    {
        char[][] visualGrid = new char[_rows][];
        for (int i = 0; i < _rows; i++)
        {
            visualGrid[i] = new char[_cols];
            Array.Copy(_grid[i], visualGrid[i], _cols);
        }

        if (path.Count <= 1)
            return GridToString(visualGrid);

        for (int i = 0; i < path.Count - 1; i++)
        {
            var current = path[i];
            var next = path[i + 1];

            if (visualGrid[current.row][current.col] != 'S' &&
                visualGrid[current.row][current.col] != 'E')
            {
                if (current.row == next.row)
                {
                    visualGrid[current.row][current.col] = '-';
                }
                else
                {
                    visualGrid[current.row][current.col] = '|';
                }
            }
        }

        return GridToString(visualGrid);
    }

    private string GridToString(char[][] grid)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < grid.Length; i++)
        {
            sb.AppendLine(new string(grid[i]));
        }
        return sb.ToString();
    }
}
