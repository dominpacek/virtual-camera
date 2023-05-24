using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using virtual_camera.Logic;

namespace virtual_camera.Objects;

public class Wall : IComparable<Wall>
{
    public readonly Polygon Projection;
    public readonly List<Point3D> Points;
    private Vector4? _plane;

    public Wall(List<Point3D> points, Brush edgeBrush, Brush wallBrush)
    {
        if (points.Count != 4)
        {
            throw new ArgumentException($"Wall must have 4 vertices. Got {points.Count}");
        }

        Projection = CreateProjection(points, edgeBrush, wallBrush);
        Points = points;
    }

    private Polygon CreateProjection(List<Point3D> points3D, Brush edgeBrush, Brush wallBrush)
    {
        var a = Projector.ProjectPoint(points3D[0]);
        var b = Projector.ProjectPoint(points3D[1]);
        var c = Projector.ProjectPoint(points3D[2]);
        var d = Projector.ProjectPoint(points3D[3]);

        var verticesOutOfSight = 0;
        if (a.Z < 0) verticesOutOfSight++;
        if (b.Z < 0) verticesOutOfSight++;
        if (c.Z < 0) verticesOutOfSight++;
        if (d.Z < 0) verticesOutOfSight++;

        if (verticesOutOfSight > 0)
        {
            return new Polygon();
        }

        var polygonPoints = new PointCollection
        {
            new Point(a.X, a.Y),
            new Point(b.X, b.Y),
            new Point(c.X, c.Y),
            new Point(d.X, d.Y)
        };

        var poly = new Polygon
        {
            Points = polygonPoints,
            Stroke = edgeBrush,
            Fill = wallBrush
        };

        return poly;
    }


    public double GetProjectionMinX()
    {
        return Projection.Points.Select(point => point.X).Min();
    }

    public double GetProjectionMaxX()
    {
        return Projection.Points.Select(point => point.X).Max();
    }

    public double GetProjectionMinY()
    {
        return Projection.Points.Select(point => point.Y).Min();
    }

    public double GetProjectionMaxY()
    {
        return Projection.Points.Select(point => point.Y).Max();
    }

    public Vector4 GetPlane()
    {
        _plane ??= PlaneMath.GetPlaneFromPoints(Points);

        return _plane.Value;
    }


    public int CompareTo(Wall? other)
    {
        // -1 means that this wall is further from the camera than the other wall
        // 1 means this wall is closer to the camera than the other
        // used for sorting from furthest to closest for Painter's algorithm

        var P = this;
        var Q = other;

        if (P.Points.Min(p => p.Z) > Q.Points.Max(p => p.Z))
        {
            return -1;
        }

        if (Q.Points.Max(p => p.Z) < P.Points.Min(p => p.Z))
        {
            return -1;
        }

        if ((P.Points.Min(p => p.X) > Q.Points.Max(p => p.X) || Q.Points.Min(p => p.X) > P.Points.Min(p => p.X)) &&
            P.Points.Min(p => p.Y) > Q.Points.Max(p => p.Y) || Q.Points.Min(p => p.Y) > P.Points.Min(p => p.Y))
        {
            return 0;
        }

        // Behind plane
        double res = 0;
        foreach (var v in P.Points)
        {
            res += PlaneMath.PlaneOffset(v, Q.Points);
        }

        if (res < 0) return -1;
        if (res == 0) return 0;


        // Ahead of plane
        res = 0;
        foreach (var v in Q.Points)
        {
            res += PlaneMath.PlaneOffset(v, P.Points);
        }

        if (res > 0) return -1;
        if (res == 0) return 0;


        // Ahead plane
        res = 0;
        foreach (var v in P.Points)
        {
            res += PlaneMath.PlaneOffset(v, Q.Points);
        }

        if (res > 0) return 1;
        if (res == 0) return 0;

        
        // Behind plane
        res = 0;
        foreach (var v in Q.Points)
        {
            res += PlaneMath.PlaneOffset(v, P.Points);
        }

        if (res < 0) return 1;
        if (res == 0) return 0;


        return 1;
    }


    public double GetMinZ()
    {
        return Points.Select(point => point.Z).Min();
    }

    public double GetMaxZ()
    {
        return Points.Select(point => point.Z).Max();
    }


    private Point3D GetCenter()
    {
        var x = Points.Sum(point => point.X) / 4;
        var y = Points.Sum(point => point.Y) / 4;
        var z = Points.Sum(point => point.Z) / 4;

        return new Point3D(x, y, z);
    }
}