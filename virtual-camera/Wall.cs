using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using virtual_camera.Transformations;

namespace virtual_camera;

public class Wall : IComparable<Wall>
{
    public readonly Polygon Projection;
    public readonly List<Point3D> Points;

    public Wall(Polygon projection, List<Point3D> points)
    {
        if (points.Count != 4)
        {
            throw new ArgumentException($"Wall must have 4 vertices. Got {points.Count}");
        }
        
        Projection = projection;
        Points = points;
    }

    public int CompareTo(Wall? other)
    {
        // -1 means that this wall is further from the camera than the other wall
        // used for sorting from furthest to closest for Painter's algorithm
        
        // if (GetCenterZ() > other.GetCenterZ()) return -1;
        // return 1;
        
        
        return PlaneMath.IsPointInFrontOfPlane(GetCenter(), other.Points);
    }

    private double GetCenterZ()
    {
        var sum = Points.Sum(point => point.Z);
        return sum / 4;
    }

    private Point3D GetCenter()
    {
        var x = Points.Sum(point => point.X) / 4;
        var y = Points.Sum(point => point.Y) / 4;
        var z = Points.Sum(point => point.Z) / 4;

        return new Point3D(x, y, z);
    }

}