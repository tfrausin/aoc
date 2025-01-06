namespace AdventOfCode.Puzzles.Day13;

using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true, iterationCount: 20)]
public partial class Puzzle1
{
    [GeneratedRegex(@"X\+(\d+), Y\+(\d+)")]
    private static partial Regex ExtractButtonMovement();

    [GeneratedRegex(@"X=(\d+), Y=(\d+)")]
    private static partial Regex ExtractPrizePosition();

    [Test]
    public void Solve()
    {
        var machines = PuzzleInput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Chunk(3)
            .Select(x =>
            {
                var a = ExtractButtonMovement().Match(x[0]);
                var b = ExtractButtonMovement().Match(x[1]);
                var p = ExtractPrizePosition().Match(x[2]);

                return (
                    ax: int.Parse(a.Groups[1].Value),
                    ay: int.Parse(a.Groups[2].Value),
                    bx: int.Parse(b.Groups[1].Value),
                    by: int.Parse(b.Groups[2].Value),
                    px: int.Parse(p.Groups[1].Value),
                    py: int.Parse(p.Groups[2].Value)
                );
            })
            .ToArray();

        var result = 0;

        foreach (var machine in machines)
        {
            var minCost = int.MaxValue;

            for (var a = 0; a <= 100; a++)
            {
                for (var b = 0; b <= 100; b++)
                {
                    if (
                        machine.ax * a + machine.bx * b == machine.px
                        && machine.ay * a + machine.by * b == machine.py
                    )
                    {
                        var cost = 3 * a + b;

                        if (cost < minCost)
                        {
                            minCost = cost;
                        }
                    }
                }
            }

            if (minCost is not int.MaxValue)
            {
                result += minCost;
            }
        }

        Console.WriteLine(result);
    }

    [Test]
    [Benchmark]
    public void SolveCramer()
    {
        var machines = PuzzleInput
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Chunk(3)
            .Select(x =>
            {
                var a = ExtractButtonMovement().Match(x[0]);
                var b = ExtractButtonMovement().Match(x[1]);
                var p = ExtractPrizePosition().Match(x[2]);

                return (
                    ax: int.Parse(a.Groups[1].Value),
                    ay: int.Parse(a.Groups[2].Value),
                    bx: int.Parse(b.Groups[1].Value),
                    by: int.Parse(b.Groups[2].Value),
                    px: int.Parse(p.Groups[1].Value) + 10000000000000,
                    py: int.Parse(p.Groups[2].Value) + 10000000000000
                );
            })
            .ToArray();

        var result = 0L;

        foreach (var machine in machines)
        {
            var d = machine.ax * machine.by - machine.ay * machine.bx;
            
            if (d is not 0)
            {
                var dx = machine.px * machine.by - machine.py * machine.bx;
                var dy = machine.ax * machine.py - machine.ay * machine.px;

                if (dx % d is 0 && dy % d is 0)
                {
                    var a = dx / d;
                    var b = dy / d;

                    if (a >= 0 && b >= 0)
                    {
                        result += 3 * a + b;
                    }
                }
            }
        }

        Console.WriteLine(result);
    }

    private const string PuzzleInputTest = """
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=8400, Y=5400

        Button A: X+26, Y+66
        Button B: X+67, Y+21
        Prize: X=12748, Y=12176

        Button A: X+17, Y+86
        Button B: X+84, Y+37
        Prize: X=7870, Y=6450

        Button A: X+69, Y+23
        Button B: X+27, Y+71
        Prize: X=18641, Y=10279
        """;

    private const string PuzzleInput = """
        Button A: X+51, Y+11
        Button B: X+38, Y+78
        Prize: X=16146, Y=3706

        Button A: X+49, Y+16
        Button B: X+27, Y+68
        Prize: X=273, Y=5932

        Button A: X+62, Y+26
        Button B: X+14, Y+72
        Prize: X=5778, Y=5994

        Button A: X+59, Y+16
        Button B: X+13, Y+33
        Prize: X=2588, Y=4419

        Button A: X+12, Y+54
        Button B: X+84, Y+30
        Prize: X=6264, Y=3132

        Button A: X+17, Y+36
        Button B: X+24, Y+11
        Prize: X=9386, Y=10508

        Button A: X+86, Y+16
        Button B: X+53, Y+99
        Prize: X=9224, Y=4212

        Button A: X+49, Y+19
        Button B: X+17, Y+29
        Prize: X=8562, Y=2808

        Button A: X+94, Y+33
        Button B: X+76, Y+99
        Prize: X=13170, Y=10626

        Button A: X+31, Y+83
        Button B: X+30, Y+13
        Prize: X=2342, Y=5328

        Button A: X+19, Y+70
        Button B: X+91, Y+57
        Prize: X=9831, Y=10341

        Button A: X+17, Y+53
        Button B: X+42, Y+19
        Prize: X=18363, Y=6523

        Button A: X+23, Y+53
        Button B: X+54, Y+25
        Prize: X=19213, Y=10463

        Button A: X+82, Y+59
        Button B: X+12, Y+30
        Prize: X=962, Y=19691

        Button A: X+87, Y+39
        Button B: X+41, Y+85
        Prize: X=5806, Y=5534

        Button A: X+50, Y+22
        Button B: X+12, Y+40
        Prize: X=9012, Y=12484

        Button A: X+12, Y+77
        Button B: X+54, Y+40
        Prize: X=5808, Y=9070

        Button A: X+23, Y+55
        Button B: X+22, Y+12
        Prize: X=7262, Y=9212

        Button A: X+37, Y+72
        Button B: X+48, Y+18
        Prize: X=13728, Y=19538

        Button A: X+47, Y+13
        Button B: X+19, Y+68
        Prize: X=10217, Y=353

        Button A: X+19, Y+78
        Button B: X+64, Y+11
        Prize: X=6429, Y=18831

        Button A: X+68, Y+39
        Button B: X+18, Y+39
        Prize: X=14100, Y=13250

        Button A: X+36, Y+16
        Button B: X+30, Y+43
        Prize: X=1212, Y=1488

        Button A: X+65, Y+66
        Button B: X+52, Y+13
        Prize: X=8502, Y=5210

        Button A: X+11, Y+52
        Button B: X+46, Y+15
        Prize: X=17383, Y=16691

        Button A: X+18, Y+62
        Button B: X+40, Y+14
        Prize: X=11606, Y=11752

        Button A: X+64, Y+13
        Button B: X+22, Y+84
        Prize: X=6838, Y=14971

        Button A: X+48, Y+74
        Button B: X+36, Y+14
        Prize: X=15608, Y=19390

        Button A: X+21, Y+38
        Button B: X+46, Y+13
        Prize: X=11724, Y=4297

        Button A: X+83, Y+59
        Button B: X+32, Y+78
        Prize: X=3043, Y=4981

        Button A: X+84, Y+44
        Button B: X+19, Y+36
        Prize: X=8075, Y=6548

        Button A: X+93, Y+82
        Button B: X+14, Y+82
        Prize: X=8616, Y=13448

        Button A: X+50, Y+16
        Button B: X+13, Y+66
        Prize: X=8478, Y=8144

        Button A: X+65, Y+11
        Button B: X+21, Y+71
        Prize: X=15378, Y=13478

        Button A: X+86, Y+32
        Button B: X+32, Y+94
        Prize: X=10188, Y=8306

        Button A: X+56, Y+89
        Button B: X+81, Y+29
        Prize: X=7731, Y=9594

        Button A: X+16, Y+67
        Button B: X+56, Y+15
        Prize: X=14112, Y=7986

        Button A: X+25, Y+92
        Button B: X+46, Y+43
        Prize: X=3915, Y=6199

        Button A: X+64, Y+22
        Button B: X+31, Y+72
        Prize: X=19601, Y=11484

        Button A: X+75, Y+32
        Button B: X+12, Y+59
        Prize: X=7142, Y=17241

        Button A: X+15, Y+37
        Button B: X+22, Y+15
        Prize: X=16767, Y=14005

        Button A: X+69, Y+45
        Button B: X+13, Y+34
        Prize: X=13399, Y=13150

        Button A: X+12, Y+95
        Button B: X+34, Y+43
        Prize: X=2226, Y=7445

        Button A: X+30, Y+72
        Button B: X+63, Y+15
        Prize: X=19817, Y=3335

        Button A: X+38, Y+65
        Button B: X+52, Y+21
        Prize: X=14800, Y=8406

        Button A: X+44, Y+70
        Button B: X+43, Y+13
        Prize: X=3023, Y=2767

        Button A: X+29, Y+72
        Button B: X+47, Y+12
        Prize: X=18585, Y=5444

        Button A: X+46, Y+93
        Button B: X+88, Y+26
        Prize: X=4466, Y=5687

        Button A: X+12, Y+78
        Button B: X+89, Y+67
        Prize: X=8782, Y=6956

        Button A: X+14, Y+47
        Button B: X+79, Y+37
        Prize: X=19474, Y=14377

        Button A: X+84, Y+62
        Button B: X+35, Y+87
        Prize: X=9870, Y=12056

        Button A: X+52, Y+67
        Button B: X+47, Y+12
        Prize: X=8019, Y=6399

        Button A: X+58, Y+15
        Button B: X+15, Y+46
        Prize: X=5308, Y=14885

        Button A: X+15, Y+87
        Button B: X+49, Y+36
        Prize: X=5263, Y=10173

        Button A: X+42, Y+91
        Button B: X+96, Y+20
        Prize: X=7362, Y=2603

        Button A: X+23, Y+32
        Button B: X+68, Y+17
        Prize: X=6489, Y=3906

        Button A: X+14, Y+35
        Button B: X+44, Y+26
        Prize: X=18884, Y=13730

        Button A: X+96, Y+23
        Button B: X+14, Y+45
        Prize: X=2052, Y=1741

        Button A: X+45, Y+12
        Button B: X+73, Y+87
        Prize: X=9082, Y=6744

        Button A: X+76, Y+95
        Button B: X+69, Y+23
        Prize: X=10338, Y=9507

        Button A: X+19, Y+45
        Button B: X+49, Y+33
        Prize: X=3493, Y=4889

        Button A: X+19, Y+68
        Button B: X+36, Y+27
        Prize: X=2483, Y=4711

        Button A: X+14, Y+44
        Button B: X+76, Y+40
        Prize: X=1176, Y=18144

        Button A: X+71, Y+30
        Button B: X+11, Y+34
        Prize: X=1722, Y=1788

        Button A: X+70, Y+14
        Button B: X+13, Y+48
        Prize: X=15826, Y=3506

        Button A: X+16, Y+86
        Button B: X+24, Y+11
        Prize: X=1736, Y=1897

        Button A: X+19, Y+46
        Button B: X+40, Y+20
        Prize: X=3434, Y=5956

        Button A: X+29, Y+76
        Button B: X+60, Y+13
        Prize: X=652, Y=10334

        Button A: X+51, Y+16
        Button B: X+11, Y+88
        Prize: X=1148, Y=5264

        Button A: X+56, Y+74
        Button B: X+69, Y+19
        Prize: X=5731, Y=5913

        Button A: X+97, Y+13
        Button B: X+35, Y+87
        Prize: X=8825, Y=7109

        Button A: X+13, Y+97
        Button B: X+82, Y+36
        Prize: X=8277, Y=9933

        Button A: X+20, Y+78
        Button B: X+98, Y+50
        Prize: X=10512, Y=9770

        Button A: X+16, Y+59
        Button B: X+74, Y+15
        Prize: X=12152, Y=3677

        Button A: X+59, Y+28
        Button B: X+12, Y+97
        Prize: X=1281, Y=3621

        Button A: X+57, Y+16
        Button B: X+26, Y+68
        Prize: X=9838, Y=4304

        Button A: X+62, Y+30
        Button B: X+17, Y+40
        Prize: X=4604, Y=15530

        Button A: X+23, Y+43
        Button B: X+61, Y+35
        Prize: X=6302, Y=6328

        Button A: X+24, Y+58
        Button B: X+76, Y+50
        Prize: X=7264, Y=4990

        Button A: X+60, Y+14
        Button B: X+15, Y+47
        Prize: X=15905, Y=11499

        Button A: X+60, Y+21
        Button B: X+28, Y+71
        Prize: X=10440, Y=3544

        Button A: X+58, Y+79
        Button B: X+79, Y+23
        Prize: X=11945, Y=8571

        Button A: X+84, Y+39
        Button B: X+59, Y+89
        Prize: X=10972, Y=10762

        Button A: X+66, Y+11
        Button B: X+30, Y+29
        Prize: X=8274, Y=3035

        Button A: X+12, Y+46
        Button B: X+38, Y+14
        Prize: X=1074, Y=2142

        Button A: X+39, Y+75
        Button B: X+63, Y+31
        Prize: X=4113, Y=2861

        Button A: X+11, Y+48
        Button B: X+60, Y+23
        Prize: X=6374, Y=6559

        Button A: X+82, Y+32
        Button B: X+32, Y+73
        Prize: X=8438, Y=3898

        Button A: X+61, Y+69
        Button B: X+13, Y+92
        Prize: X=5963, Y=13547

        Button A: X+53, Y+13
        Button B: X+24, Y+56
        Prize: X=3878, Y=14454

        Button A: X+69, Y+19
        Button B: X+60, Y+88
        Prize: X=11541, Y=9611

        Button A: X+99, Y+17
        Button B: X+69, Y+74
        Prize: X=7701, Y=1571

        Button A: X+12, Y+16
        Button B: X+53, Y+12
        Prize: X=4613, Y=1868

        Button A: X+29, Y+46
        Button B: X+76, Y+30
        Prize: X=7025, Y=3084

        Button A: X+41, Y+96
        Button B: X+89, Y+47
        Prize: X=3940, Y=5352

        Button A: X+16, Y+70
        Button B: X+58, Y+13
        Prize: X=14844, Y=1830

        Button A: X+37, Y+76
        Button B: X+48, Y+23
        Prize: X=3037, Y=2912

        Button A: X+19, Y+56
        Button B: X+53, Y+23
        Prize: X=8795, Y=1122

        Button A: X+16, Y+57
        Button B: X+73, Y+22
        Prize: X=7196, Y=9582

        Button A: X+28, Y+81
        Button B: X+81, Y+31
        Prize: X=9565, Y=7948

        Button A: X+28, Y+83
        Button B: X+79, Y+53
        Prize: X=1757, Y=3940

        Button A: X+47, Y+15
        Button B: X+20, Y+43
        Prize: X=9073, Y=16137

        Button A: X+12, Y+59
        Button B: X+34, Y+15
        Prize: X=1408, Y=15815

        Button A: X+67, Y+82
        Button B: X+54, Y+12
        Prize: X=7393, Y=7750

        Button A: X+16, Y+42
        Button B: X+60, Y+24
        Prize: X=8492, Y=6932

        Button A: X+63, Y+22
        Button B: X+26, Y+39
        Prize: X=6160, Y=2570

        Button A: X+29, Y+50
        Button B: X+39, Y+14
        Prize: X=7277, Y=6706

        Button A: X+75, Y+30
        Button B: X+15, Y+85
        Prize: X=1275, Y=4855

        Button A: X+56, Y+80
        Button B: X+99, Y+29
        Prize: X=10204, Y=6932

        Button A: X+39, Y+76
        Button B: X+59, Y+23
        Prize: X=2603, Y=18618

        Button A: X+55, Y+22
        Button B: X+11, Y+21
        Prize: X=8790, Y=16090

        Button A: X+59, Y+27
        Button B: X+20, Y+49
        Prize: X=8642, Y=989

        Button A: X+46, Y+18
        Button B: X+11, Y+56
        Prize: X=701, Y=6206

        Button A: X+89, Y+36
        Button B: X+21, Y+50
        Prize: X=1986, Y=2090

        Button A: X+67, Y+39
        Button B: X+23, Y+45
        Prize: X=3283, Y=7841

        Button A: X+14, Y+38
        Button B: X+53, Y+21
        Prize: X=15486, Y=5782

        Button A: X+55, Y+82
        Button B: X+34, Y+13
        Prize: X=3881, Y=5654

        Button A: X+70, Y+93
        Button B: X+81, Y+22
        Prize: X=7456, Y=8536

        Button A: X+57, Y+20
        Button B: X+23, Y+70
        Prize: X=2748, Y=1150

        Button A: X+13, Y+52
        Button B: X+65, Y+24
        Prize: X=2252, Y=13208

        Button A: X+43, Y+26
        Button B: X+33, Y+94
        Prize: X=3682, Y=4892

        Button A: X+12, Y+66
        Button B: X+59, Y+37
        Prize: X=2237, Y=3391

        Button A: X+29, Y+44
        Button B: X+41, Y+15
        Prize: X=2883, Y=14474

        Button A: X+58, Y+23
        Button B: X+26, Y+69
        Prize: X=18548, Y=790

        Button A: X+15, Y+48
        Button B: X+34, Y+14
        Prize: X=6766, Y=3120

        Button A: X+99, Y+62
        Button B: X+20, Y+71
        Prize: X=10131, Y=10204

        Button A: X+82, Y+53
        Button B: X+49, Y+98
        Prize: X=4986, Y=8529

        Button A: X+58, Y+19
        Button B: X+17, Y+64
        Prize: X=18319, Y=16836

        Button A: X+24, Y+61
        Button B: X+51, Y+15
        Prize: X=16619, Y=5249

        Button A: X+40, Y+15
        Button B: X+15, Y+28
        Prize: X=14600, Y=11862

        Button A: X+69, Y+94
        Button B: X+96, Y+38
        Prize: X=9843, Y=5430

        Button A: X+64, Y+18
        Button B: X+37, Y+57
        Prize: X=1690, Y=1314

        Button A: X+32, Y+88
        Button B: X+49, Y+17
        Prize: X=3211, Y=1883

        Button A: X+21, Y+81
        Button B: X+56, Y+13
        Prize: X=4211, Y=9104

        Button A: X+24, Y+57
        Button B: X+62, Y+29
        Prize: X=8984, Y=9446

        Button A: X+16, Y+38
        Button B: X+36, Y+17
        Prize: X=5120, Y=13756

        Button A: X+16, Y+58
        Button B: X+33, Y+16
        Prize: X=2125, Y=1382

        Button A: X+32, Y+18
        Button B: X+34, Y+81
        Prize: X=4858, Y=8487

        Button A: X+26, Y+63
        Button B: X+59, Y+11
        Prize: X=7428, Y=1346

        Button A: X+12, Y+18
        Button B: X+39, Y+17
        Prize: X=3768, Y=2166

        Button A: X+51, Y+21
        Button B: X+21, Y+68
        Prize: X=2031, Y=1964

        Button A: X+18, Y+44
        Button B: X+32, Y+16
        Prize: X=1642, Y=2396

        Button A: X+76, Y+21
        Button B: X+15, Y+58
        Prize: X=14609, Y=10087

        Button A: X+80, Y+14
        Button B: X+11, Y+66
        Prize: X=17960, Y=10392

        Button A: X+11, Y+27
        Button B: X+70, Y+29
        Prize: X=7280, Y=16804

        Button A: X+13, Y+69
        Button B: X+41, Y+14
        Prize: X=3560, Y=15352

        Button A: X+21, Y+82
        Button B: X+79, Y+58
        Prize: X=3112, Y=8144

        Button A: X+65, Y+17
        Button B: X+32, Y+80
        Prize: X=11536, Y=19696

        Button A: X+15, Y+77
        Button B: X+72, Y+16
        Prize: X=4748, Y=14316

        Button A: X+44, Y+18
        Button B: X+48, Y+77
        Prize: X=944, Y=8166

        Button A: X+59, Y+81
        Button B: X+84, Y+31
        Prize: X=11871, Y=9889

        Button A: X+84, Y+25
        Button B: X+34, Y+40
        Prize: X=10454, Y=5950

        Button A: X+42, Y+87
        Button B: X+26, Y+15
        Prize: X=2418, Y=3027

        Button A: X+74, Y+15
        Button B: X+11, Y+75
        Prize: X=7287, Y=7385

        Button A: X+61, Y+25
        Button B: X+61, Y+94
        Prize: X=3782, Y=2378

        Button A: X+21, Y+47
        Button B: X+61, Y+42
        Prize: X=6148, Y=4591

        Button A: X+15, Y+31
        Button B: X+54, Y+26
        Prize: X=8996, Y=300

        Button A: X+44, Y+15
        Button B: X+26, Y+65
        Prize: X=2162, Y=4835

        Button A: X+15, Y+65
        Button B: X+77, Y+27
        Prize: X=3721, Y=14871

        Button A: X+18, Y+54
        Button B: X+96, Y+85
        Prize: X=2700, Y=5055

        Button A: X+75, Y+24
        Button B: X+14, Y+65
        Prize: X=13006, Y=9742

        Button A: X+84, Y+22
        Button B: X+80, Y+81
        Prize: X=4768, Y=1549

        Button A: X+67, Y+13
        Button B: X+26, Y+90
        Prize: X=7217, Y=7687

        Button A: X+17, Y+38
        Button B: X+64, Y+26
        Prize: X=5849, Y=13186

        Button A: X+24, Y+84
        Button B: X+67, Y+13
        Prize: X=4194, Y=15954

        Button A: X+81, Y+16
        Button B: X+89, Y+98
        Prize: X=5807, Y=1710

        Button A: X+58, Y+42
        Button B: X+12, Y+31
        Prize: X=11756, Y=17980

        Button A: X+71, Y+31
        Button B: X+24, Y+60
        Prize: X=487, Y=8439

        Button A: X+14, Y+30
        Button B: X+54, Y+36
        Prize: X=2868, Y=824

        Button A: X+20, Y+50
        Button B: X+57, Y+35
        Prize: X=14224, Y=2120

        Button A: X+50, Y+90
        Button B: X+52, Y+27
        Prize: X=8558, Y=10143

        Button A: X+18, Y+56
        Button B: X+59, Y+29
        Prize: X=9187, Y=6331

        Button A: X+42, Y+12
        Button B: X+17, Y+43
        Prize: X=2076, Y=7726

        Button A: X+16, Y+71
        Button B: X+57, Y+12
        Prize: X=16784, Y=8489

        Button A: X+17, Y+34
        Button B: X+97, Y+44
        Prize: X=7236, Y=5622

        Button A: X+12, Y+34
        Button B: X+70, Y+45
        Prize: X=8912, Y=6744

        Button A: X+51, Y+15
        Button B: X+27, Y+61
        Prize: X=18809, Y=5569

        Button A: X+35, Y+19
        Button B: X+14, Y+50
        Prize: X=19450, Y=18814

        Button A: X+32, Y+60
        Button B: X+89, Y+38
        Prize: X=5907, Y=5534

        Button A: X+26, Y+66
        Button B: X+82, Y+64
        Prize: X=5058, Y=5776

        Button A: X+11, Y+58
        Button B: X+33, Y+12
        Prize: X=3421, Y=5888

        Button A: X+67, Y+22
        Button B: X+39, Y+54
        Prize: X=3615, Y=2670

        Button A: X+54, Y+11
        Button B: X+25, Y+60
        Prize: X=4322, Y=3098

        Button A: X+23, Y+65
        Button B: X+67, Y+28
        Prize: X=13907, Y=8288

        Button A: X+81, Y+40
        Button B: X+12, Y+48
        Prize: X=6386, Y=17280

        Button A: X+28, Y+76
        Button B: X+66, Y+16
        Prize: X=17828, Y=7740

        Button A: X+75, Y+42
        Button B: X+15, Y+41
        Prize: X=2630, Y=5829

        Button A: X+41, Y+16
        Button B: X+27, Y+69
        Prize: X=11413, Y=17601

        Button A: X+21, Y+50
        Button B: X+47, Y+17
        Prize: X=3152, Y=7762

        Button A: X+23, Y+49
        Button B: X+59, Y+23
        Prize: X=11332, Y=18758

        Button A: X+17, Y+48
        Button B: X+35, Y+16
        Prize: X=6295, Y=18640

        Button A: X+21, Y+36
        Button B: X+46, Y+22
        Prize: X=3066, Y=3864

        Button A: X+14, Y+49
        Button B: X+32, Y+17
        Prize: X=6780, Y=2055

        Button A: X+51, Y+11
        Button B: X+74, Y+96
        Prize: X=2837, Y=2853

        Button A: X+57, Y+93
        Button B: X+95, Y+42
        Prize: X=7296, Y=7158

        Button A: X+40, Y+19
        Button B: X+26, Y+42
        Prize: X=2502, Y=4213

        Button A: X+53, Y+26
        Button B: X+12, Y+31
        Prize: X=3646, Y=3873

        Button A: X+49, Y+30
        Button B: X+13, Y+32
        Prize: X=3161, Y=7930

        Button A: X+92, Y+11
        Button B: X+49, Y+47
        Prize: X=1852, Y=386

        Button A: X+76, Y+22
        Button B: X+18, Y+63
        Prize: X=10332, Y=3330

        Button A: X+29, Y+79
        Button B: X+91, Y+74
        Prize: X=8448, Y=8754

        Button A: X+68, Y+26
        Button B: X+22, Y+68
        Prize: X=17792, Y=7910

        Button A: X+63, Y+27
        Button B: X+20, Y+45
        Prize: X=10536, Y=12419

        Button A: X+54, Y+21
        Button B: X+41, Y+63
        Prize: X=974, Y=567

        Button A: X+75, Y+42
        Button B: X+18, Y+74
        Prize: X=1830, Y=1664

        Button A: X+46, Y+22
        Button B: X+36, Y+68
        Prize: X=5880, Y=8632

        Button A: X+61, Y+17
        Button B: X+23, Y+57
        Prize: X=130, Y=11968

        Button A: X+97, Y+32
        Button B: X+19, Y+64
        Prize: X=4201, Y=2656

        Button A: X+48, Y+14
        Button B: X+11, Y+53
        Prize: X=17093, Y=2449

        Button A: X+72, Y+25
        Button B: X+16, Y+44
        Prize: X=6464, Y=12960

        Button A: X+73, Y+35
        Button B: X+14, Y+52
        Prize: X=12126, Y=14254

        Button A: X+18, Y+35
        Button B: X+52, Y+13
        Prize: X=10222, Y=4469

        Button A: X+35, Y+17
        Button B: X+17, Y+46
        Prize: X=17266, Y=2037

        Button A: X+41, Y+63
        Button B: X+39, Y+18
        Prize: X=12760, Y=13355

        Button A: X+11, Y+37
        Button B: X+90, Y+66
        Prize: X=8789, Y=8731

        Button A: X+25, Y+66
        Button B: X+53, Y+22
        Prize: X=19199, Y=7866

        Button A: X+15, Y+64
        Button B: X+53, Y+20
        Prize: X=15758, Y=14104

        Button A: X+56, Y+14
        Button B: X+20, Y+40
        Prize: X=2096, Y=3754

        Button A: X+81, Y+33
        Button B: X+62, Y+89
        Prize: X=8918, Y=7139

        Button A: X+14, Y+75
        Button B: X+66, Y+25
        Prize: X=7332, Y=9050

        Button A: X+67, Y+30
        Button B: X+11, Y+57
        Prize: X=17507, Y=12494

        Button A: X+13, Y+29
        Button B: X+33, Y+18
        Prize: X=11084, Y=9353

        Button A: X+22, Y+82
        Button B: X+69, Y+65
        Prize: X=6305, Y=11393

        Button A: X+48, Y+18
        Button B: X+28, Y+65
        Prize: X=4080, Y=2698

        Button A: X+25, Y+81
        Button B: X+57, Y+36
        Prize: X=1666, Y=3465

        Button A: X+13, Y+58
        Button B: X+76, Y+19
        Prize: X=18439, Y=3796

        Button A: X+62, Y+31
        Button B: X+12, Y+44
        Prize: X=17000, Y=4532

        Button A: X+58, Y+14
        Button B: X+12, Y+27
        Prize: X=4626, Y=8551

        Button A: X+72, Y+36
        Button B: X+17, Y+40
        Prize: X=4481, Y=4540

        Button A: X+11, Y+70
        Button B: X+79, Y+26
        Prize: X=5872, Y=18456

        Button A: X+48, Y+63
        Button B: X+86, Y+17
        Prize: X=9394, Y=6673

        Button A: X+55, Y+80
        Button B: X+45, Y+19
        Prize: X=4480, Y=3311

        Button A: X+11, Y+72
        Button B: X+79, Y+49
        Prize: X=2763, Y=2170

        Button A: X+69, Y+18
        Button B: X+23, Y+67
        Prize: X=4738, Y=6116

        Button A: X+51, Y+13
        Button B: X+62, Y+97
        Prize: X=4106, Y=5756

        Button A: X+38, Y+77
        Button B: X+57, Y+18
        Prize: X=14256, Y=11799

        Button A: X+12, Y+91
        Button B: X+95, Y+34
        Prize: X=1834, Y=4298

        Button A: X+74, Y+31
        Button B: X+31, Y+94
        Prize: X=7631, Y=10569

        Button A: X+61, Y+15
        Button B: X+16, Y+52
        Prize: X=18239, Y=5153

        Button A: X+29, Y+27
        Button B: X+21, Y+99
        Prize: X=2764, Y=3924

        Button A: X+77, Y+38
        Button B: X+16, Y+46
        Prize: X=4176, Y=6240

        Button A: X+56, Y+29
        Button B: X+20, Y+39
        Prize: X=6744, Y=16572

        Button A: X+42, Y+81
        Button B: X+54, Y+16
        Prize: X=7508, Y=1323

        Button A: X+50, Y+25
        Button B: X+14, Y+36
        Prize: X=10990, Y=610

        Button A: X+38, Y+53
        Button B: X+31, Y+12
        Prize: X=10134, Y=222

        Button A: X+12, Y+56
        Button B: X+69, Y+13
        Prize: X=11651, Y=8967

        Button A: X+66, Y+41
        Button B: X+38, Y+75
        Prize: X=7494, Y=9435

        Button A: X+14, Y+57
        Button B: X+80, Y+32
        Prize: X=406, Y=8349

        Button A: X+36, Y+54
        Button B: X+83, Y+23
        Prize: X=6458, Y=2582

        Button A: X+77, Y+46
        Button B: X+23, Y+91
        Prize: X=2890, Y=5744

        Button A: X+62, Y+24
        Button B: X+20, Y+65
        Prize: X=1188, Y=1426

        Button A: X+15, Y+39
        Button B: X+57, Y+42
        Prize: X=4962, Y=6423

        Button A: X+94, Y+69
        Button B: X+34, Y+90
        Prize: X=5468, Y=4404

        Button A: X+28, Y+12
        Button B: X+24, Y+62
        Prize: X=17664, Y=15738

        Button A: X+33, Y+16
        Button B: X+16, Y+71
        Prize: X=3228, Y=4411

        Button A: X+49, Y+30
        Button B: X+13, Y+36
        Prize: X=16441, Y=13664

        Button A: X+39, Y+16
        Button B: X+15, Y+31
        Prize: X=14498, Y=1446

        Button A: X+43, Y+19
        Button B: X+30, Y+51
        Prize: X=17239, Y=703

        Button A: X+21, Y+49
        Button B: X+59, Y+43
        Prize: X=5099, Y=3851

        Button A: X+12, Y+88
        Button B: X+51, Y+38
        Prize: X=2265, Y=6194

        Button A: X+81, Y+51
        Button B: X+25, Y+51
        Prize: X=4880, Y=6528

        Button A: X+64, Y+12
        Button B: X+21, Y+54
        Prize: X=13774, Y=15836

        Button A: X+25, Y+36
        Button B: X+34, Y+15
        Prize: X=2668, Y=15437

        Button A: X+34, Y+38
        Button B: X+78, Y+14
        Prize: X=3802, Y=3298

        Button A: X+13, Y+36
        Button B: X+43, Y+13
        Prize: X=8321, Y=4651

        Button A: X+51, Y+25
        Button B: X+13, Y+56
        Prize: X=15988, Y=12861

        Button A: X+23, Y+75
        Button B: X+32, Y+23
        Prize: X=1005, Y=2301

        Button A: X+29, Y+63
        Button B: X+41, Y+14
        Prize: X=16039, Y=13507

        Button A: X+18, Y+25
        Button B: X+36, Y+14
        Prize: X=12158, Y=6303

        Button A: X+32, Y+14
        Button B: X+41, Y+62
        Prize: X=18144, Y=11988

        Button A: X+60, Y+13
        Button B: X+12, Y+58
        Prize: X=15416, Y=3289

        Button A: X+12, Y+54
        Button B: X+80, Y+41
        Prize: X=10388, Y=13748

        Button A: X+99, Y+36
        Button B: X+22, Y+74
        Prize: X=10120, Y=7310

        Button A: X+58, Y+86
        Button B: X+40, Y+12
        Prize: X=1194, Y=11778

        Button A: X+21, Y+55
        Button B: X+53, Y+20
        Prize: X=592, Y=17730

        Button A: X+36, Y+18
        Button B: X+15, Y+49
        Prize: X=16451, Y=2339

        Button A: X+27, Y+52
        Button B: X+48, Y+27
        Prize: X=4268, Y=1926

        Button A: X+71, Y+23
        Button B: X+12, Y+39
        Prize: X=13157, Y=12527

        Button A: X+24, Y+36
        Button B: X+90, Y+24
        Prize: X=8136, Y=4656

        Button A: X+38, Y+17
        Button B: X+15, Y+49
        Prize: X=18923, Y=14090

        Button A: X+18, Y+21
        Button B: X+87, Y+26
        Prize: X=4800, Y=2731

        Button A: X+76, Y+27
        Button B: X+12, Y+64
        Prize: X=13136, Y=13657

        Button A: X+42, Y+15
        Button B: X+13, Y+37
        Prize: X=9534, Y=3834

        Button A: X+49, Y+12
        Button B: X+47, Y+82
        Prize: X=5721, Y=5710

        Button A: X+46, Y+19
        Button B: X+27, Y+56
        Prize: X=15045, Y=10856

        Button A: X+64, Y+12
        Button B: X+29, Y+83
        Prize: X=18758, Y=18618

        Button A: X+21, Y+44
        Button B: X+69, Y+42
        Prize: X=14129, Y=11908

        Button A: X+93, Y+46
        Button B: X+39, Y+69
        Prize: X=2094, Y=2875

        Button A: X+56, Y+19
        Button B: X+37, Y+61
        Prize: X=6632, Y=6901

        Button A: X+93, Y+12
        Button B: X+13, Y+83
        Prize: X=5465, Y=6967

        Button A: X+78, Y+60
        Button B: X+16, Y+99
        Prize: X=7050, Y=11925

        Button A: X+32, Y+21
        Button B: X+14, Y+43
        Prize: X=1536, Y=3172

        Button A: X+32, Y+86
        Button B: X+93, Y+12
        Prize: X=9407, Y=7436

        Button A: X+83, Y+58
        Button B: X+21, Y+52
        Prize: X=5158, Y=5396

        Button A: X+25, Y+86
        Button B: X+57, Y+43
        Prize: X=6416, Y=8600

        Button A: X+17, Y+52
        Button B: X+87, Y+72
        Prize: X=7636, Y=8216

        Button A: X+32, Y+86
        Button B: X+63, Y+12
        Prize: X=6547, Y=15346

        Button A: X+77, Y+45
        Button B: X+26, Y+72
        Prize: X=3035, Y=7227

        Button A: X+46, Y+11
        Button B: X+19, Y+55
        Prize: X=3122, Y=2563

        Button A: X+37, Y+60
        Button B: X+46, Y+14
        Prize: X=10324, Y=706

        Button A: X+76, Y+12
        Button B: X+15, Y+75
        Prize: X=8370, Y=19370

        Button A: X+69, Y+34
        Button B: X+23, Y+84
        Prize: X=6417, Y=5778

        Button A: X+94, Y+11
        Button B: X+39, Y+31
        Prize: X=8207, Y=2573

        Button A: X+14, Y+39
        Button B: X+82, Y+53
        Prize: X=9764, Y=3802

        Button A: X+49, Y+18
        Button B: X+15, Y+51
        Prize: X=13201, Y=8270

        Button A: X+31, Y+53
        Button B: X+23, Y+11
        Prize: X=9227, Y=5211

        Button A: X+75, Y+13
        Button B: X+21, Y+75
        Prize: X=15692, Y=10308

        Button A: X+47, Y+11
        Button B: X+62, Y+94
        Prize: X=3820, Y=1212

        Button A: X+13, Y+95
        Button B: X+21, Y+23
        Prize: X=1157, Y=5063

        Button A: X+11, Y+68
        Button B: X+61, Y+65
        Prize: X=5788, Y=10501

        Button A: X+64, Y+16
        Button B: X+19, Y+48
        Prize: X=18821, Y=4304

        Button A: X+56, Y+25
        Button B: X+39, Y+59
        Prize: X=7726, Y=6194

        Button A: X+33, Y+67
        Button B: X+58, Y+17
        Prize: X=14335, Y=3990

        Button A: X+19, Y+73
        Button B: X+58, Y+54
        Prize: X=5815, Y=9341

        Button A: X+25, Y+45
        Button B: X+88, Y+15
        Prize: X=4777, Y=4440

        Button A: X+35, Y+25
        Button B: X+25, Y+77
        Prize: X=4290, Y=8742

        Button A: X+92, Y+15
        Button B: X+81, Y+85
        Prize: X=12809, Y=6755

        Button A: X+12, Y+63
        Button B: X+81, Y+21
        Prize: X=8828, Y=2615

        Button A: X+21, Y+40
        Button B: X+56, Y+21
        Prize: X=15390, Y=2402

        Button A: X+39, Y+14
        Button B: X+34, Y+67
        Prize: X=2222, Y=4624
        """;
}
