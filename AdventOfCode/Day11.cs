using FileParser;

namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly List<long> _list;

    public Day11()
    {
        _input = new ParsedFile(InputFilePath);
        _list = _input.ToList<long>();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private object Solve_1_v1()
    {
        List<long> processList = [.. _list];
        var blinks = 25;
        while (blinks > 0)
        {
            --blinks;
            processList = ProcessList(processList);
        }

        //Console.WriteLine(string.Join(' ', processList));
        return processList.Count;
    }

    private List<long> ProcessList(List<long> list)
    {
        List<long> newList = [];
        foreach (var number in list)
        {
            if (number == 0)
            {
                newList.Add(1);
            }
            //if number has even numbers
            else if (number.ToString().Length % 2 == 0)
            {
                string numberStr = number.ToString();

                // Split the string in half
                var middleIndex = numberStr.Length / 2;
                var firstPart = long.Parse(numberStr[..middleIndex]);
                var secondPart = long.Parse(numberStr[middleIndex..]);
                newList.AddRange([firstPart, secondPart]);
            }
            else
            {
                newList.Add(number * 2024);
            }
        }

        return newList;
    }

    public override ValueTask<string> Solve_2() => new($"{Solve_2_v5()}");

    // Function to process a number according to the rules
    public static Dictionary<long, long> ProcessMap(Dictionary<long, long> map)
    {
        var newMap = new Dictionary<long, long>();

        foreach (var kvp in map)
        {
            long number = kvp.Key;
            long count = kvp.Value;

            if (number == 0)
            {
                // Special case: if the number is 0, treat it as 1
                newMap[1] = newMap.ContainsKey(1) ? newMap[1] + count : count;
            }
            else
            {
                int numDigits = Digits_Log10(number);
                if (numDigits % 2 == 0)  // Even number of digits, split
                {
                    int middleIndex = numDigits / 2;
                    long firstPart = number / (long)Math.Pow(10, middleIndex);
                    long secondPart = number % (long)Math.Pow(10, middleIndex);

                    // Add the first part and second part to the new map
                    newMap[firstPart] = newMap.ContainsKey(firstPart) ? newMap[firstPart] + count : count;
                    newMap[secondPart] = newMap.ContainsKey(secondPart) ? newMap[secondPart] + count : count;
                }
                else  // Odd number of digits, multiply by 2024
                {
                    long newNumber = number * 2024;
                    newMap[newNumber] = newMap.ContainsKey(newNumber) ? newMap[newNumber] + count : count;
                }
            }
        }

        return newMap;
    }

    public static long Solve(List<long> list, int iterations)
    {
        // Step 1: Initialize the frequency map
        var map = new Dictionary<long, long>();

        foreach (var number in list)
        {
            map[number] = map.ContainsKey(number) ? map[number] + 1 : 1;
        }

        // Step 2: Apply the transformation for the given number of iterations
        for (int i = 0; i < iterations; i++)
        {
            map = ProcessMap(map);
        }

        // Step 3: Return the total count of numbers after all iterations
        long totalCount = 0;
        foreach (var kvp in map)
        {
            totalCount += kvp.Value;
        }

        return totalCount;
    }

    public long Solve_2_v5()
    {
        int iterations = 75;

        long result = Solve(_list, iterations);

        Console.WriteLine($"Total count after {iterations} iterations: {result}");
        return result;
    }

    private static int Digits_Log10(long n)
    {
        // Special case for 0, which has 1 digit
        if (n == 0L) return 1;

        // If n is positive or negative, calculate the number of digits using Log10
        return (n > 0L ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n));
    }

}
