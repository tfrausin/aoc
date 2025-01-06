namespace AdventOfCode.Puzzles.Day8;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
public class Puzzle1
{
    [Test]
    [Benchmark]
    public void Solve()
    {
        var lines = PuzzleInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var antennas = lines
            .SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
            .Where(x => char.IsLetterOrDigit(x.c))
            .ToList();
        var antiNodes = new HashSet<(int x, int y)>();
        var distances = new[] { -1, 2 };
        
        for (var i = 0; i < antennas.Count; i++)
        {
            for (var j = i + 1; j < antennas.Count; j++)
            {
                if (antennas[i].c == antennas[j].c)
                {
                    var dx = antennas[j].x - antennas[i].x;
                    var dy = antennas[j].y - antennas[i].y;
                    var idx = dx > 0 ? i : j;
                    var gradient = dx == 0 ? 0 : (double)dy / dx;
                    var angle = Math.Atan(gradient);
        
                    foreach (var d in distances)
                    {
                        var distance = d * Math.Sqrt(dx * dx + dy * dy);
        
                        var x = (int)Math.Round(antennas[idx].x + distance * Math.Cos(angle));
                        var y = (int)Math.Round(antennas[idx].y + distance * Math.Sin(angle));
        
                        if (x >= 0 && x < lines.Length && y >= 0 && y < lines[0].Length)
                        {
                            antiNodes.Add((x, y));
                        }
                    }
                }
            }
        }

        var output = lines.Select(x => x.ToCharArray()).ToArray();
        foreach (var (x, y) in antiNodes)
        {
            if (output[y][x] == '.')
            {
                output[y][x] = '#';
            }
        }

        Console.WriteLine($"Unique locations with an antinode: {antiNodes.Count}");
        Console.WriteLine();
        Console.WriteLine(string.Join('\n', output.Select(x => new string(x))));
    }

    private const string PuzzleInputTest = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """;

    private const string PuzzleInput = """
        .....................................O..V.........
        ..................................................
        ................................O.........Z.......
        ....W....................................V....v...
        ........................m................8........
        .....................................n........Z..v
        .............F.....3...n....5m....................
        ................................................V.
        ................3............iv....Z.............V
        ...........................O..n..i........p......H
        ......W..6..............................i.........
        ......................................b...........
        ..................................n........p......
        ........M.......c...........m..5......1...........
        ...M............................L..5..A...........
        ...w...........9.............F5..................q
        .W.....................................q....p.....
        .......W........r.......H.....LA......q...........
        ................4.F....................A..........
        ........3.......a.....F...................A..L....
        ....ME...............................Q..........q.
        .E..................ih...................Z........
        ................E...H...........h.................
        .........m.........X..............................
        ..................0......C.................h......
        .M......l.................Q.h.....................
        ..........C..............0........................
        .............lX............3.c....................
        ......8.X.........c....r..a......H.....9..........
        .................QE.....C.........................
        ..R................a........Q...................7.
        ...........................a......................
        l..........X.R............1..I..........9.........
        .................0R..............b.....z......x...
        .......l.....w....r..........................b....
        .8..........0...................P1z...............
        .............c.........................L..........
        .................C..N............o............9...
        ...........e..f..N................................
        8.............................B...................
        ...........4...............................x......
        ....w....RY..........4.......................P....
        .........yw.....Y.............o2...............7..
        ..6y........4..............fo..............7......
        .........Y..6............o......................x.
        .....Y....e.....y..I.r...........2................
        ....e.............................P.......z.bB....
        .............6.................B........7......x..
        ..y.N........f...........1....I....z....B.........
        .....e....f.............I.................2.......
        """;
}
