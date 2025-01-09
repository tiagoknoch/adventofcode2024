using FileParser;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly string[] _patterns;
    private readonly string[] _groups;

    public Day19()
    {
        _input = new ParsedFile(InputFilePath);
        _patterns = _input.NextLine().ToSingleString().Split(',', StringSplitOptions.TrimEntries);
        _groups = _input.ToList<string>().ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private int Solve_1_v1()
    {
        var result = 0;
        PatternMatcher matcher = new PatternMatcher(_patterns);
        var results = matcher.CheckGroups(_groups);

        foreach (var kvp in results)
        {
            Console.Write($"Group: {kvp.Key} => ");
            if (kvp.Value == null)
            {
                Console.WriteLine("Cannot be formed.");
            }
            else
            {
                ++result;
                Console.WriteLine($"Can be formed with patterns: {string.Join(", ", kvp.Value)}");
            }
        }
        return result;
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException(); class PatternMatcher
    {
        private HashSet<string> patternSet;  // Use HashSet for O(1) lookups

        public PatternMatcher(string[] patterns)
        {
            patternSet = [.. patterns];
        }

        public Dictionary<string, List<string>> CheckGroups(string[] groups)
        {
            Dictionary<string, List<string>> results = [];

            foreach (var group in groups)
            {
                results[group] = CanFormGroup(group);
            }

            return results;
        }

        private List<string> CanFormGroup(string group)
        {
            // DP dictionary to store the best way to form substrings
            Dictionary<int, List<string>> dp = new()
            {
            { 0, new List<string>() } // Base case: Empty string can be formed with an empty list
        };

            for (int i = 1; i <= group.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    string sub = group.Substring(j, i - j);
                    if (patternSet.Contains(sub) && dp.ContainsKey(j)) // If sub is a valid pattern
                    {
                        dp[i] = new List<string>(dp[j]) { sub }; // Store the sequence used
                        break; // Stop early if we find a valid way
                    }
                }
            }

            return dp.ContainsKey(group.Length) ? dp[group.Length] : null;
        }
    }
}
