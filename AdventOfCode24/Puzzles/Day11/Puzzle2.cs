namespace AdventOfCode.Puzzles.Day11;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public class Puzzle2
{
    private static readonly Dictionary<(long, int), long> Lookup = new();
    
    [Test]
    [Benchmark]
    public void Solve()
    {
        var stones = PuzzleInput.Split().Select(long.Parse).ToList();
        var result = stones.Sum(stone => Blink(stone, 75));
        
        Console.WriteLine(result);
    }

    private static long Blink(long stone, int depth)
    {
        if (depth is 0)
        {
            return 1;
        }

        if (Lookup.TryGetValue((stone, depth), out var value))
        {
            return value;
        }

        long result;

        if (stone is 0)
        {
            result = Blink(1, depth - 1);
        }
        else
        {
            var stoneAsString = stone.ToString();
            if (stoneAsString.Length % 2 is 0)
            {
                var mid = stoneAsString.Length / 2;
                var left = long.Parse(stoneAsString[..mid]);
                var right = long.Parse(stoneAsString[mid..]);

                result = Blink(left, depth - 1) + Blink(right, depth - 1);
            }
            else
            {
                result = Blink(stone * 2024, depth - 1);
            }
        }

        Lookup[(stone, depth)] = result;

        return result;
    }

    private const string PuzzleInput = """
        3279 998884 1832781 517 8 18864 28 0
        """;
}
