using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Media3D;

namespace virtual_camera;

public class FileReader
{
    private static IEnumerable<string> ReadFile(string path)
    {
        var lines = System.IO.File.ReadAllLines(path);
        return lines.Where(l => l[0] != '#');
    }

    public static List<Cuboid> LoadScene(int sceneNumber)
    {
        Debug.WriteLine($"Loading scene {sceneNumber}...");
        var fileContent = ReadFile("Scenes/scene" + sceneNumber + ".txt");
        var cuboids = GenerateCuboids(fileContent);
        return cuboids;
    }

    private static List<Cuboid> GenerateCuboids(IEnumerable<string> lines)
    {
        var cuboids = new List<Cuboid>();
        if (lines.Count() % 2 != 0)
        {
            Debug.WriteLine("[GenerateCuboids] Error: There's a point without a pair in the scene file");
            return cuboids;
        }

        for (var i = 0; i < lines.Count(); i += 2)
        {
            var lineA = lines.ElementAt(i).Split(",");
            if (lineA.Length != 3)
            {
                Debug.WriteLine($"[GenerateCuboids] Error: Point {i} is not in the correct format");
                continue;
            }

            var lineB = lines.ElementAt(i + 1).Split(",");

            if (lineB.Length != 3)
            {
                Debug.WriteLine($"[GenerateCuboids] Error: Point {i + 1} is not in the correct format");
                continue;
            }

            var a = new Point3D(double.Parse(lineA[0]), double.Parse(lineA[1]), double.Parse(lineA[2]));
            var b = new Point3D(double.Parse(lineB[0]), double.Parse(lineB[1]), double.Parse(lineB[2]));

            if (a.X == b.X || a.Y == b.Y || a.Z == b.Z)
            {
                Debug.WriteLine(
                    $"[GenerateCuboids] Error: Cuboid {i / 2 + 1} can't be constructed by two points on the same axis");
                Debug.WriteLine($"A: {a}, B: {b}");
                continue;
            }

            var cube = new Cuboid(a, b);

            cuboids.Add(cube);
            Debug.WriteLine($"Success: Cuboid {i} generated");
        }

        return cuboids;
    }
}