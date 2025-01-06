namespace AdventOfCode.Puzzles.Day17;

using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public partial class Puzzle1
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex ExtractDigits();

    [Test]
    [Benchmark]
    public void Solve()
    {
        var a = ExtractDigits().Matches(PuzzleInput).Select(x => int.Parse(x.Value)).ToArray()[0];

        Console.WriteLine(Run(a));
    }

    private static long Out(long a)
    {
        var b = a % 8 ^ 1;
        //var c = a >> (int)b;
        //C = A / (1 << B)
        var c = a / (1 << (int)b);
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
    [Benchmark]
    public void SolveOld()
    {
        var digits = ExtractDigits().Matches(PuzzleInput).Select(x => int.Parse(x.Value)).ToArray();
        var a = digits[0];
        var b = digits[1];
        var c = digits[2];
        var program = digits[3..];

        var outputs = new List<int>();

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
        for (var i = 0; i < program.Length; )
        {
            var opcode = program[i];
            var operand = program[i + 1];
            i += 2;

            if (opcode is 0) // adv
            {
                a /= (1 << GetComboOperandValue(operand, a, b, c));
            }
            else if (opcode is 1) // bxl
            {
                b ^= operand;
            }
            else if (opcode is 2) // bst
            {
                b = GetComboOperandValue(operand, a, b, c) % 8;
            }
            else if (opcode is 3) // jnz
            {
                if (a is not 0)
                {
                    i = operand;
                }
            }
            else if (opcode is 4) // bxc
            {
                b ^= c;
            }
            else if (opcode is 5) // out
            {
                outputs.Add(GetComboOperandValue(operand, a, b, c) % 8);
            }
            else if (opcode is 6) // bdv
            {
                b = a / (1 << GetComboOperandValue(operand, a, b, c));
            }
            else if (opcode is 7) // cdv
            {
                c = a / (1 << GetComboOperandValue(operand, a, b, c));
            }
            else
            {
                throw new Exception($"Invalid opcode {opcode} at position {i}");
            }
        }

        Console.WriteLine(string.Join(',', outputs));
    }

    private static int GetComboOperandValue(int operand, int a, int b, int c) =>
        operand switch
        {
            >= 0 and <= 3 => operand,
            4 => a,
            5 => b,
            6 => c,
            _ => throw new Exception()
        };

    private const string PuzzleInputTest1 = """
        TODO
        """;

    private const string PuzzleInputTest2 = """
        TODO
        """;

    private const string PuzzleInput = """
        Register A: 65804993
        Register B: 0
        Register C: 0

        Program: 2,4,1,1,7,5,1,4,0,3,4,5,5,5,3,0
        """;
}
