namespace AdventOfCode.Puzzles.Day11;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public class Puzzle1
{
    [Test]
    [Benchmark]
    public void Solve()
    {
        var stones = PuzzleInput.Split().Select(long.Parse).ToList();
        var memo = new Dictionary<(long, int), long>();

        var result = stones.Aggregate(0L, (result, stone) => result + Traverse(stone, 25, memo));

        Console.WriteLine(result);
    }

    private static long Traverse(long stone, int depth, Dictionary<(long, int), long> memo)
    {
        if (depth is 0)
        {
            return 1;
        }

        if (memo.TryGetValue((stone, depth), out var value))
        {
            return value;
        }

        long result;

        if (stone is 0)
        {
            result = Traverse(1, depth - 1, memo);
        }
        else
        {
            var stoneAsString = stone.ToString();
            if (stoneAsString.Length % 2 is 0)
            {
                var mid = stoneAsString.Length / 2;
                var left = long.Parse(stoneAsString[..mid]);
                var right = long.Parse(stoneAsString[mid..]);

                result = Traverse(left, depth - 1, memo) + Traverse(right, depth - 1, memo);
            }
            else
            {
                result = Traverse(stone * 2024, depth - 1, memo);
            }
        }

        memo[(stone, depth)] = result;

        return result;
    }

    public void SolveOld()
    {
        var stones = PuzzleInput.Split().Select(long.Parse).ToList();

        for (var i = 0; i < 25; i++)
        {
            var newStones = new List<long>();

            foreach (var stone in stones)
            {
                if (stone is 0)
                {
                    newStones.Add(1);
                    continue;
                }

                var stoneAsString = stone.ToString();
                if (stoneAsString.Length % 2 is 0)
                {
                    var mid = stoneAsString.Length / 2;
                    newStones.Add(long.Parse(stoneAsString[..mid]));
                    newStones.Add(long.Parse(stoneAsString[mid..]));

                    continue;
                }

                newStones.Add(stone * 2024);
            }

            stones = newStones;
        }

        Console.WriteLine(stones.Count);
    }

    private const string PuzzleInput = """
        3279 998884 1832781 517 8 18864 28 0
        """;
}
