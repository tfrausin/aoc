namespace AdventOfCode.Puzzles.Day10;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public class Puzzle1
{
    private static readonly (int, int)[] Directions = [(-1, 0), (1, 0), (0, -1), (0, 1)];

    [Test]
    [Benchmark]
    public void Solve()
    {
        var map = PuzzleInput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Select(c => c - '0').ToArray())
            .ToArray();
        var rows = map.Length;
        var cols = map[0].Length;
        var visited = new bool[rows, cols];
        var sum = 0;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (map[i][j] is 0)
                {
                    Array.Clear(visited);

                    sum += CountTrails(map, i, j, visited);
                }
            }
        }

        Console.WriteLine($"Number of trails: {sum}");
    }

    private static int CountTrails(int[][] map, int x, int y, bool[,] visited)
    {
        visited[x, y] = true;

        if (map[x][y] is 9)
        {
            return 1;
        }

        var count = 0;

        foreach (var (dx, dy) in Directions)
        {
            var nextX = x + dx;
            var nextY = y + dy;

            if (
                nextX >= 0
                && nextX < map.Length
                && nextY >= 0
                && nextY < map[nextX].Length
                && visited[nextX, nextY] == false
                && map[nextX][nextY] == map[x][y] + 1
            )
            {
                count += CountTrails(map, nextX, nextY, visited);
            }
        }

        visited[x, y] = false;

        return count;
    }

    [Test]
    [Benchmark]
    public void Iterative()
    {
        var map = PuzzleInput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Select(c => c - '0').ToArray())
            .ToArray();
        var rows = map.Length;
        var cols = map[0].Length;

        bool ValidPosition(int x, int y) => x >= 0 && x < rows && y >= 0 && y < cols;

        var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

        var stack = new Stack<(int, int)>();
        var visited = new bool[rows, cols];

        var score = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (map[i][j] == 0)
                {
                    Array.Clear(visited);

                    stack.Push((i, j));

                    while (stack.Count > 0)
                    {
                        var (x, y) = stack.Pop();

                        if (!visited[x, y])
                        {
                            visited[x, y] = true;

                            if (map[x][y] == 9)
                            {
                                score += 1;
                                continue;
                            }

                            foreach (var (dx, dy) in directions)
                            {
                                var nextX = x + dx;
                                var nextY = y + dy;

                                if (
                                    ValidPosition(nextX, nextY)
                                    && !visited[nextX, nextY]
                                    && map[nextX][nextY] == map[x][y] + 1
                                )
                                {
                                    stack.Push((nextX, nextY));
                                }
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Total Score: {score}");
    }

    private const string PuzzleInput = """
        87013234598721098890112345654321567654321430678
        96523105678652567781103454765010108501230521669
        85434987019563401632902169898173219101045678754
        12325673423444514547811070186784109892101289603
        04510012537654321036723489235695698763230310512
        23456767648945890125432108744312787654785401412
        10012898105456787436987610653201098945696982303
        94326743204321896547876325672102367038987801454
        85780659812310986540105234986789456127432196565
        76691234787404567831234156705434549786512087676
        34500125696589458921940049812321678690103458985
        23210034545672307810851234767870676561054760876
        10345895632361016901764123456901689432367891965
        01256786761054325410953010289812544343456982334
        65987789834583610323874320176543443214367887403
        74325692123498725678865343212652345305236996512
        85014301034567634589987054307891656456105678109
        92189212376556546543216125696780760147814309238
        43078301985431037650105434785410871412923210147
        14565467692323128965410231294321982303887426756
        07890198501015432378321140345321073494396545890
        16763223401456701489210056576438996585654034321
        23454015432389810543256787682347687676783128932
        34980106501247817650149896591056014561292107876
        45673297621056908961230123401267123450354510945
        39654388930543213870234985432678984565463211234
        08701276845470142310145676561067897878972100125
        12102345456987231456501654376458956987989934324
        03347876567896540987632781281302345101879875016
        14256960656934567896545690190211054256760766545
        45108921245023100945989018981000763345601057893
        96907830332110231235678727852167892034512347812
        87816543214398345894165436543450901124323456903
        70727631201267654583065435454321303265411092104
        61238980310345675672178920346765214578702383415
        56745675476568986543069011207894363699813476576
        43873456787487654322124302216787654780124549985
        02912169892398901210765214345568903441035678870
        11004076781870145567856702106693412234549898901
        29865985010969236450909813456782561127656787632
        36774554323458787321812301101101870018723452543
        45983463212387693210903432212232986789610961056
        35612378301496543210876520129801235498101872345
        34703569410103452323987015638734389328901903216
        23805677654312601034989234745655676210872214707
        10914988723276541025678109823456789432563345898
        23423889010189432010123211014349876501454356767
        """;
}
