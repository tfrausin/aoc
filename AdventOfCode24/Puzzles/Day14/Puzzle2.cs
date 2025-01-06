namespace AdventOfCode.Puzzles.Day14;

using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public partial class Puzzle2
{
    [GeneratedRegex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
    private static partial Regex ExtractPositionAndVelocity();

    [Test]
    [Benchmark]
    public void Solve()
    {
        const int rows = 103;
        const int cols = 101;
        const int simulations = 10403;

        var directions = new[] { (-1, 0), (0, 1), (0, -1), (1, 0) };
        var robots = PuzzleInput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(
                (x, idx) =>
                {
                    var r = ExtractPositionAndVelocity().Match(x);

                    return (
                        id: idx,
                        x: int.Parse(r.Groups[1].Value),
                        y: int.Parse(r.Groups[2].Value),
                        vx: int.Parse(r.Groups[3].Value),
                        vy: int.Parse(r.Groups[4].Value)
                    );
                }
            )
            .ToArray();

        for (var s = 0; s < simulations; s++)
        {
            var grid = new char[rows][];
            for (var j = 0; j < rows; j++)
            {
                grid[j] = new char[cols];
                Array.Fill(grid[j], '.');
            }

            for (var i = 0; i < robots.Length; i++)
            {
                var (id, x, y, vx, vy) = robots[i];

                x = (x + vx + cols) % cols;
                y = (y + vy + rows) % rows;

                robots[i] = (id, x, y, vx, vy);

                grid[y][x] = '#';
            }

            var queue = new Queue<(int x, int y)>();
            var visited = new bool[rows, cols];
            var largestArea = 0;

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    if (!visited[i, j] && grid[i][j] == '#')
                    {
                        queue.Enqueue((i, j));
                        visited[i, j] = true;

                        var area = 0;

                        while (queue.Count > 0)
                        {
                            var (x, y) = queue.Dequeue();
                            area++;

                            foreach (var (dx, dy) in directions)
                            {
                                var nx = x + dx;
                                var ny = y + dy;

                                if (nx is >= 0 and < rows && ny is >= 0 and < cols)
                                {
                                    if (grid[nx][ny] is '#' && visited[nx, ny] is false)
                                    {
                                        visited[nx, ny] = true;
                                        queue.Enqueue((nx, ny));
                                    }
                                }
                            }
                        }

                        if (area > largestArea)
                        {
                            largestArea = area;
                        }
                    }
                }
            }

            if (largestArea >= 17)
            {
                Console.WriteLine($"Found a large continuous area of robots at simulation {s + 1}");
                Console.WriteLine(
                    string.Join(Environment.NewLine, grid.Select(x => new string(x)))
                );
                Console.WriteLine();
            }
        }
    }

    private const string PuzzleInputTest = """
        p=0,4 v=3,-3
        p=6,3 v=-1,-3
        p=10,3 v=-1,2
        p=2,0 v=2,-1
        p=0,0 v=1,3
        p=3,0 v=-2,-2
        p=7,6 v=-1,-3
        p=3,0 v=-1,-2
        p=9,3 v=2,3
        p=7,3 v=-1,2
        p=2,4 v=2,-3
        p=9,5 v=-3,-3
        """;

    private const string PuzzleInput = """
        p=98,102 v=52,-49
        p=36,69 v=19,-95
        p=32,43 v=23,1
        p=53,78 v=-68,-28
        p=57,68 v=58,99
        p=55,75 v=13,83
        p=71,101 v=33,93
        p=13,92 v=-74,78
        p=47,96 v=66,-70
        p=89,12 v=39,-13
        p=100,64 v=43,-78
        p=47,91 v=13,-88
        p=14,87 v=38,40
        p=66,17 v=65,23
        p=0,67 v=-27,-16
        p=71,17 v=-24,25
        p=97,30 v=-13,-77
        p=62,7 v=11,-26
        p=43,62 v=-57,-71
        p=78,66 v=79,-34
        p=62,54 v=-90,7
        p=98,98 v=64,21
        p=34,2 v=-23,-80
        p=16,94 v=-67,59
        p=66,70 v=-76,-65
        p=92,54 v=-48,-11
        p=48,30 v=-66,-66
        p=82,24 v=-40,67
        p=39,89 v=54,65
        p=37,102 v=84,52
        p=70,95 v=36,57
        p=9,80 v=-70,75
        p=1,1 v=-9,-70
        p=4,99 v=-73,79
        p=64,61 v=-21,-5
        p=88,67 v=42,-83
        p=44,1 v=91,-20
        p=8,85 v=38,9
        p=67,57 v=83,62
        p=20,74 v=38,81
        p=76,23 v=72,-19
        p=90,99 v=-91,-21
        p=99,12 v=-99,22
        p=49,65 v=19,13
        p=74,48 v=93,44
        p=73,97 v=21,27
        p=15,95 v=72,-15
        p=94,65 v=71,-90
        p=13,58 v=71,-96
        p=38,0 v=74,71
        p=81,89 v=75,5
        p=40,15 v=8,-87
        p=45,83 v=48,-40
        p=66,87 v=-7,33
        p=67,40 v=-83,-36
        p=73,9 v=-22,-56
        p=52,25 v=21,56
        p=50,78 v=98,99
        p=96,31 v=64,-12
        p=88,19 v=-37,-98
        p=15,90 v=99,88
        p=86,101 v=-80,-75
        p=27,90 v=-64,87
        p=33,83 v=-49,-70
        p=87,38 v=-81,23
        p=6,4 v=60,-50
        p=61,38 v=-37,-10
        p=3,36 v=-77,-91
        p=25,1 v=46,-11
        p=87,97 v=-87,41
        p=26,35 v=-31,67
        p=85,8 v=-48,60
        p=14,96 v=92,-99
        p=25,46 v=52,73
        p=29,75 v=-10,-21
        p=56,62 v=13,-50
        p=14,58 v=12,-98
        p=8,71 v=-85,32
        p=87,52 v=64,-78
        p=80,68 v=46,-94
        p=98,37 v=35,-79
        p=97,11 v=-23,-26
        p=59,71 v=76,38
        p=61,27 v=-28,-20
        p=83,46 v=50,98
        p=5,34 v=-73,79
        p=86,50 v=93,98
        p=94,101 v=50,3
        p=31,70 v=97,24
        p=14,21 v=40,38
        p=87,95 v=-6,-65
        p=33,11 v=-78,-92
        p=92,85 v=-35,82
        p=75,46 v=-72,-65
        p=98,55 v=64,7
        p=38,66 v=8,-53
        p=88,97 v=-62,-76
        p=90,75 v=-29,-21
        p=32,50 v=68,-25
        p=2,98 v=24,-14
        p=24,12 v=-34,-61
        p=32,82 v=-92,3
        p=72,96 v=94,-95
        p=55,72 v=15,-77
        p=84,77 v=3,99
        p=80,78 v=-47,-10
        p=79,36 v=-69,48
        p=42,6 v=33,-93
        p=72,2 v=-65,-8
        p=51,84 v=26,94
        p=15,17 v=31,-68
        p=23,7 v=45,-14
        p=52,14 v=-9,64
        p=8,29 v=16,-73
        p=100,59 v=-45,-41
        p=21,63 v=52,14
        p=9,48 v=-67,-60
        p=89,8 v=-31,79
        p=21,100 v=48,59
        p=81,49 v=-58,19
        p=1,77 v=24,57
        p=25,34 v=5,73
        p=20,101 v=-67,-2
        p=86,36 v=28,-95
        p=97,31 v=51,-7
        p=76,20 v=83,-12
        p=96,77 v=-8,-22
        p=84,94 v=39,-69
        p=11,10 v=-99,-26
        p=100,23 v=-1,-13
        p=20,50 v=16,-54
        p=15,81 v=-66,88
        p=27,62 v=-71,-72
        p=70,56 v=90,81
        p=10,45 v=-81,-66
        p=100,51 v=24,-90
        p=31,73 v=49,-59
        p=56,5 v=92,7
        p=45,28 v=-46,60
        p=84,81 v=-6,28
        p=89,60 v=-91,8
        p=66,42 v=54,-42
        p=71,101 v=-76,-93
        p=31,76 v=26,-52
        p=97,55 v=31,73
        p=31,26 v=-96,41
        p=79,4 v=-29,-87
        p=65,21 v=29,36
        p=3,102 v=-8,87
        p=20,65 v=-60,-90
        p=87,60 v=-98,86
        p=57,13 v=16,87
        p=66,95 v=-43,94
        p=33,32 v=68,47
        p=23,80 v=-51,-10
        p=67,56 v=-30,71
        p=36,58 v=-53,-29
        p=57,63 v=-54,-64
        p=6,30 v=46,-98
        p=0,40 v=86,59
        p=13,102 v=69,55
        p=93,14 v=-30,29
        p=34,7 v=45,-25
        p=36,7 v=-46,-8
        p=39,100 v=19,-69
        p=25,59 v=-64,-36
        p=20,14 v=52,59
        p=12,44 v=-29,-11
        p=45,71 v=12,7
        p=2,29 v=75,-66
        p=88,54 v=75,32
        p=27,51 v=70,-48
        p=88,85 v=-68,55
        p=31,64 v=-55,-89
        p=21,16 v=9,41
        p=40,58 v=26,55
        p=96,55 v=-39,-8
        p=30,29 v=-71,30
        p=74,80 v=65,-63
        p=98,78 v=2,38
        p=54,93 v=-62,-82
        p=35,70 v=-31,32
        p=73,17 v=-29,72
        p=0,97 v=-88,-62
        p=15,26 v=38,-43
        p=20,76 v=-71,-58
        p=0,63 v=24,68
        p=21,24 v=42,31
        p=78,68 v=97,63
        p=79,44 v=79,-84
        p=60,100 v=58,95
        p=46,72 v=-7,-83
        p=25,29 v=59,18
        p=88,16 v=-24,-34
        p=62,67 v=-54,14
        p=58,35 v=-43,-12
        p=40,34 v=80,7
        p=32,90 v=-55,4
        p=76,1 v=-40,22
        p=87,4 v=-57,52
        p=76,93 v=28,-75
        p=69,49 v=36,92
        p=66,79 v=-36,-28
        p=58,63 v=-7,-16
        p=97,30 v=82,18
        p=44,10 v=37,-20
        p=11,31 v=16,21
        p=78,20 v=-72,96
        p=72,56 v=-79,-4
        p=44,81 v=-75,-27
        p=76,51 v=43,8
        p=84,81 v=14,-69
        p=0,11 v=71,-50
        p=92,51 v=70,-69
        p=73,69 v=32,-77
        p=63,66 v=-36,93
        p=25,2 v=-49,-50
        p=90,73 v=10,9
        p=47,48 v=37,-48
        p=87,5 v=-58,38
        p=11,19 v=27,29
        p=3,82 v=92,90
        p=61,68 v=-84,-70
        p=25,40 v=52,7
        p=92,47 v=-86,-21
        p=95,21 v=17,60
        p=2,47 v=-30,49
        p=18,5 v=-56,77
        p=23,31 v=-85,-30
        p=19,7 v=-38,5
        p=37,25 v=-18,96
        p=37,2 v=99,95
        p=21,58 v=42,8
        p=59,92 v=94,82
        p=61,63 v=-18,-41
        p=4,65 v=78,-83
        p=40,71 v=-64,-76
        p=82,25 v=61,66
        p=50,15 v=-57,18
        p=54,97 v=-61,-39
        p=94,5 v=-30,4
        p=14,80 v=16,-28
        p=46,40 v=-10,49
        p=95,49 v=32,74
        p=77,45 v=14,-24
        p=37,16 v=77,-39
        p=62,35 v=22,-61
        p=36,100 v=77,4
        p=75,53 v=-26,54
        p=63,43 v=25,-75
        p=95,83 v=-48,-81
        p=10,34 v=-99,-30
        p=19,9 v=13,-49
        p=27,81 v=-81,99
        p=52,15 v=69,-1
        p=19,4 v=-92,-19
        p=72,87 v=-18,-88
        p=88,94 v=28,76
        p=71,57 v=65,-47
        p=85,48 v=-76,26
        p=39,16 v=-77,-57
        p=74,97 v=-8,-39
        p=57,74 v=-16,-87
        p=83,40 v=86,61
        p=14,26 v=39,88
        p=100,22 v=-71,-81
        p=35,25 v=-3,11
        p=98,79 v=28,32
        p=73,34 v=34,46
        p=71,17 v=-25,50
        p=69,78 v=-85,44
        p=39,33 v=89,65
        p=82,100 v=10,-51
        p=14,43 v=84,42
        p=14,41 v=38,-78
        p=70,91 v=89,-20
        p=80,61 v=31,53
        p=1,58 v=71,-29
        p=14,21 v=-63,-43
        p=40,4 v=25,8
        p=70,72 v=29,20
        p=39,50 v=13,-62
        p=9,70 v=-1,-47
        p=95,31 v=-32,-63
        p=15,64 v=-92,99
        p=26,33 v=-58,-92
        p=23,60 v=70,-47
        p=5,50 v=-22,-32
        p=5,12 v=-89,7
        p=21,63 v=48,68
        p=56,62 v=-29,-10
        p=59,77 v=-36,-10
        p=76,92 v=97,-81
        p=63,6 v=-65,-93
        p=5,58 v=5,56
        p=17,24 v=-24,18
        p=13,4 v=-92,-76
        p=55,18 v=-61,-92
        p=17,83 v=38,-70
        p=71,41 v=63,-86
        p=59,94 v=90,67
        p=47,46 v=44,38
        p=24,59 v=1,-65
        p=36,69 v=72,-48
        p=70,13 v=47,-50
        p=78,11 v=54,-75
        p=78,97 v=-51,82
        p=66,37 v=-68,92
        p=88,92 v=82,-15
        p=29,28 v=46,-44
        p=10,73 v=49,81
        p=55,43 v=-79,79
        p=45,57 v=-25,19
        p=56,79 v=15,-28
        p=35,81 v=98,14
        p=16,12 v=20,41
        p=86,60 v=-91,68
        p=62,32 v=-92,-57
        p=70,1 v=7,89
        p=78,76 v=-40,-10
        p=33,62 v=88,56
        p=35,70 v=-17,-77
        p=13,65 v=-26,35
        p=45,57 v=62,-23
        p=46,100 v=26,-12
        p=79,24 v=75,5
        p=12,78 v=53,-58
        p=88,88 v=57,-75
        p=8,70 v=74,8
        p=78,40 v=29,24
        p=16,2 v=59,52
        p=100,84 v=-13,97
        p=15,68 v=6,32
        p=18,55 v=92,-35
        p=26,11 v=63,-26
        p=4,12 v=76,77
        p=58,46 v=-72,-84
        p=47,63 v=-90,-4
        p=57,51 v=-46,79
        p=37,26 v=84,-43
        p=36,4 v=52,-33
        p=99,74 v=17,-4
        p=51,77 v=91,58
        p=44,72 v=69,-73
        p=77,14 v=54,-68
        p=39,45 v=34,-24
        p=8,29 v=-49,-60
        p=38,34 v=-85,-7
        p=96,68 v=-63,1
        p=19,74 v=85,-85
        p=29,61 v=7,-14
        p=16,45 v=-38,-42
        p=21,12 v=45,59
        p=84,21 v=39,11
        p=58,35 v=58,85
        p=69,30 v=54,90
        p=38,101 v=-28,-81
        p=78,11 v=-22,-2
        p=4,13 v=30,30
        p=11,93 v=85,-39
        p=10,53 v=25,88
        p=40,23 v=-29,95
        p=92,5 v=11,-12
        p=74,32 v=43,-24
        p=88,69 v=90,-85
        p=57,34 v=22,96
        p=37,19 v=62,67
        p=15,28 v=-38,18
        p=92,52 v=-8,-23
        p=17,97 v=-9,-69
        p=83,26 v=-87,48
        p=56,35 v=40,90
        p=10,90 v=64,-22
        p=97,48 v=-19,7
        p=57,100 v=33,28
        p=12,14 v=-13,-79
        p=17,1 v=-80,-32
        p=35,97 v=-85,91
        p=89,38 v=-41,13
        p=8,11 v=-85,66
        p=73,91 v=61,-3
        p=100,71 v=3,-37
        p=52,84 v=-14,88
        p=97,20 v=-37,-25
        p=6,95 v=85,-81
        p=82,56 v=66,-7
        p=71,101 v=-83,10
        p=27,56 v=5,-22
        p=48,43 v=-43,2
        p=72,86 v=55,-4
        p=68,62 v=86,-72
        p=82,53 v=-33,-85
        p=65,66 v=-27,67
        p=64,38 v=-65,86
        p=100,35 v=13,-13
        p=53,101 v=80,-8
        p=58,35 v=33,-61
        p=28,20 v=-31,84
        p=43,65 v=22,27
        p=86,100 v=-67,-42
        p=95,75 v=6,-22
        p=99,52 v=-30,-27
        p=70,62 v=25,-29
        p=92,75 v=-37,93
        p=40,55 v=91,-16
        p=57,46 v=1,50
        p=55,93 v=40,40
        p=77,63 v=43,87
        p=39,1 v=-21,-1
        p=38,27 v=55,35
        p=48,102 v=-39,89
        p=97,33 v=78,-79
        p=82,36 v=-73,-35
        p=56,84 v=74,-75
        p=83,85 v=-22,99
        p=91,64 v=34,-84
        p=47,65 v=67,67
        p=13,7 v=2,5
        p=17,6 v=-63,-31
        p=75,59 v=25,-35
        p=11,34 v=-9,-55
        p=39,43 v=82,-44
        p=72,12 v=-79,47
        p=38,4 v=-67,-33
        p=75,48 v=76,-90
        p=32,100 v=-82,73
        p=39,9 v=-93,-47
        p=83,27 v=57,-13
        p=39,99 v=13,-29
        p=97,88 v=-32,-60
        p=43,90 v=-28,14
        p=80,42 v=52,-67
        p=33,45 v=-46,84
        p=48,83 v=43,-60
        p=59,30 v=-61,19
        p=2,89 v=-77,27
        p=81,57 v=83,2
        p=78,29 v=-33,72
        p=78,28 v=-51,-6
        p=52,0 v=-32,-57
        p=11,25 v=-27,-91
        p=30,102 v=74,71
        p=21,9 v=-45,83
        p=58,32 v=-43,91
        p=70,40 v=-22,-91
        p=74,94 v=54,-93
        p=12,38 v=20,42
        p=9,5 v=-1,17
        p=91,4 v=10,70
        p=48,36 v=73,91
        p=28,86 v=34,-10
        p=64,74 v=80,-3
        p=22,87 v=63,88
        p=63,91 v=-39,94
        p=29,91 v=14,-93
        p=67,63 v=33,87
        p=59,31 v=-76,-13
        p=33,5 v=-16,19
        p=84,94 v=21,-99
        p=18,63 v=-91,60
        p=32,95 v=-6,-33
        p=94,13 v=-52,41
        p=77,32 v=61,-18
        p=20,65 v=-20,-90
        p=43,89 v=8,3
        p=35,42 v=56,-38
        p=16,33 v=24,-54
        p=43,96 v=30,88
        p=40,21 v=59,62
        p=59,37 v=40,-61
        p=10,81 v=-2,-92
        p=52,48 v=72,-24
        p=71,70 v=-40,-89
        p=42,99 v=-64,-93
        p=68,4 v=-97,-33
        p=52,42 v=79,-64
        p=52,97 v=66,4
        p=28,64 v=73,39
        p=57,94 v=-25,45
        p=47,5 v=40,-25
        p=59,25 v=43,77
        p=49,71 v=-54,62
        p=92,2 v=79,-27
        p=66,51 v=-7,-96
        p=14,18 v=23,42
        p=69,51 v=69,-11
        p=75,80 v=-25,-34
        p=90,5 v=-30,-39
        p=7,44 v=60,-54
        p=25,71 v=86,92
        p=25,99 v=-38,-93
        p=59,94 v=94,4
        p=41,14 v=-17,65
        p=48,86 v=-10,-64
        p=78,86 v=-40,45
        p=3,16 v=27,89
        p=36,56 v=25,-42
        p=38,65 v=30,38
        p=76,101 v=77,79
        p=56,82 v=-79,-40

        """;
}
