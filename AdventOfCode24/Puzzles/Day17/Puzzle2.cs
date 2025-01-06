namespace AdventOfCode.Puzzles.Day17;

using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public partial class Puzzle2
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex ExtractDigits();

    [Test]
    [Benchmark]
    public void Solve()
    {
        var digits = ExtractDigits().Matches(PuzzleInput).Select(x => int.Parse(x.Value)).ToArray();
        var candidates = new HashSet<long> { 0 };

        foreach (var digit in digits[3..].Reverse())
        {
            var nextCandidates = new HashSet<long>();

            foreach (var candidate in candidates)
            {
                for (var offset = 0; offset < 8; offset++)
                {
                    var next = (candidate << 3) + offset;

                    if (Out(next) == digit)
                    {
                        nextCandidates.Add(next);
                    }
                }
            }

            candidates = nextCandidates;
        }

        Console.WriteLine(candidates.Min());
    }

    /*
     * Program: 2,4,1,1,7,5,1,4,0,3,4,5,5,5,3,0
     *
     * 2,4 => B = A % 8
     * 1,1 => B = B ^ 1
     * 7,5 => C = A / (1 << B)
     * 1,4 => B = B ^ 4
     * 0,3 => A = A >> 3
     * 4,5 => B = B ^ C
     * 5,5 => out(B)
     * 3,0 => jnz(0)
     */
    // var c = a / (1 << (int)b);
    private static long Out(long a)
    {
        var b = a % 8 ^ 1;
        var c = a >> (int)b;
        return (b ^ c ^ 4) % 8;
    }

    private static string Run(long a)
    {
        var result = new List<int>();

        while (a > 0)
        {
            result.Add((int)Out(a));
            a >>= 3;
        }

        return string.Join(',', result);
    }

    [Test]
    public void PrintNunitTests()
    {
        TestContext.Progress.WriteLine(
            "this will print immediately and not wait for the test to complete"
        );
    }

    private const string PuzzleInput = """
        Register A: 65804993
        Register B: 0
        Register C: 0

        Program: 2,4,1,1,7,5,1,4,0,3,4,5,5,5,3,0
        """;
}
