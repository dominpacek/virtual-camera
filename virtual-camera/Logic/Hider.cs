using System.Collections.Generic;
using System.Diagnostics;
using virtual_camera.Objects;

namespace virtual_camera.Logic;

public static class Hider
{
   
    public static List<Wall> SortWalls(List<Wall> walls)
    {
        // Sort walls in order from farthest to closest

        var count = 0;
        walls.Sort((p1, p2) => p2.GetMaxZ().CompareTo(p1.GetMaxZ()));
        for (var i = 0; i < walls.Count; i++)
        {
            var p = walls[i];
            for (var j = i + 1; j < walls.Count; j++)
            {
                count++;
                var q = walls[j];
                if (!AreProjectionsOverlapping(p, q))
                {
                    continue;
                }
                // Swap the walls if p is closer than q
                if (p.CompareTo(q) > 0)
                {
                    (walls[j], walls[i]) = (walls[i], walls[j]);
                    i--;
                    break;
                }
            }
            if (count > walls.Count * walls.Count)
            {
                Debug.WriteLine("Too many iterations");
                break;
            }
        }

        return walls;
    }

    private static bool AreProjectionsOverlapping(Wall w1, Wall w2)
    {
        if (w1.Projection.Points.Count < 1 || w2.Projection.Points.Count < 1)
        {
            return false;
        }

        return w1.GetProjectionMaxX() > w2.GetProjectionMinX() && w2.GetProjectionMaxX() > w1.GetProjectionMinX() &&
               w1.GetProjectionMaxY() > w2.GetProjectionMinY() && w2.GetProjectionMaxY() > w1.GetProjectionMinY();
    }
}