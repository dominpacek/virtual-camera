using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using virtual_camera.Enums;
using virtual_camera.Transformations;

namespace virtual_camera;

public class Cuboid
{
    private readonly Point3D[] _vertices3D;
    private List<Polygon> _walls2D;
    private Brush _edgeBrush = Brushes.CornflowerBlue;
    private Brush _wallBrush = Brushes.DodgerBlue;

    // Construct a cuboid by two opposite points
    public Cuboid(Point3D a, Point3D b)
    {
        Point3D[] vertices = new Point3D[8];
        vertices[0] = a;
        vertices[1] = new Point3D(b.X, a.Y, a.Z);
        vertices[2] = new Point3D(b.X, a.Y, b.Z);
        vertices[3] = new Point3D(a.X, a.Y, b.Z);
        vertices[4] = new Point3D(a.X, b.Y, b.Z);
        vertices[5] = b;
        vertices[6] = new Point3D(b.X, b.Y, a.Z);
        vertices[7] = new Point3D(a.X, b.Y, a.Z);
        _vertices3D = vertices;
    }

    // Construct a cuboid by an array of vertices
    private Cuboid(Point3D[] vertices3D)
    {
        var n = vertices3D.Length;
        if (n != 8)
        {
            throw new ArgumentException($"Cuboid must have 8 vertices. Got {n}");
        }

        _vertices3D = vertices3D;
    }

    public Cuboid Project()
    {
        var projectedVertices = new List<Point3D>();
        foreach (var vertex in _vertices3D)
        {
            projectedVertices.Add(Projector.ProjectPoint(vertex));
        }

        var projectedCuboid = new Cuboid(projectedVertices.ToArray())
        {
            _edgeBrush = _edgeBrush,
            _wallBrush = _wallBrush
        };
        projectedCuboid.GenerateWalls();
        return projectedCuboid;
    }

    public Cuboid Translate(CameraMoveDirection direction)
    {
        var translatedVertices = new List<Point3D>();
        foreach (var vertex in _vertices3D)
        {
            translatedVertices.Add(Translator.TranslatePoint(vertex, direction));
        }

        return new Cuboid(translatedVertices.ToArray()) { _edgeBrush = _edgeBrush, _wallBrush = _wallBrush};
    }

    public Cuboid Rotate(CameraRotation rotation)
    {
        var rotatedVertices = new List<Point3D>();
        foreach (var vertex in _vertices3D)
        {
            rotatedVertices.Add(Rotator.RotatePoint(vertex, rotation));
        }

        return new Cuboid(rotatedVertices.ToArray()) { _edgeBrush = _edgeBrush, _wallBrush = _wallBrush};
    }

    // Only for projected cuboids
    private void GenerateWalls()
    {
        _walls2D = new List<Polygon>(6);

        CreatePolygonWithVertices(0, 1, 2, 3);
        CreatePolygonWithVertices(0, 1, 6, 7);
        CreatePolygonWithVertices(0, 3, 4, 7);
        CreatePolygonWithVertices(1, 2, 5, 6);
        CreatePolygonWithVertices(2, 3, 4, 5);
        CreatePolygonWithVertices(4, 5, 6, 7);
    }

    // Create a polygon with the given vertices (by index)
    private void CreatePolygonWithVertices(int indexA, int indexB, int indexC, int indexD)
    {
        var a = _vertices3D[indexA];
        var b = _vertices3D[indexB];
        var c = _vertices3D[indexC];
        var d = _vertices3D[indexD];

        var verticesOutOfSight = 0;
        if (a.Z < 0) verticesOutOfSight++;
        if (b.Z < 0) verticesOutOfSight++;
        if (c.Z < 0) verticesOutOfSight++;
        if (d.Z < 0) verticesOutOfSight++;

        if (verticesOutOfSight > 0)
        {
            return;
        }

        var points = new PointCollection
        {
            new Point(a.X, a.Y),
            new Point(b.X, b.Y),
            new Point(c.X, c.Y),
            new Point(d.X, d.Y)
        };

        var poly = new Polygon
        {
            Points = points,
            Stroke = _edgeBrush,
            Fill = _wallBrush
        };

        _walls2D.Add(poly);
    }

    public List<Polygon> GetWalls()
    {
        return _walls2D;
    }

    public void SetColor(int i)
    {
        _edgeBrush = (i % 8) switch
        {
            1 => Brushes.YellowGreen,
            2 => Brushes.ForestGreen,
            3 => Brushes.OrangeRed,
            4 => Brushes.MediumOrchid,
            5 => Brushes.Blue,
            6 => Brushes.Gold,
            7 => Brushes.Red,
            _ => Brushes.CornflowerBlue
        };
        _wallBrush = (i % 8) switch
        {
            1 => Brushes.GreenYellow,
            2 => Brushes.Green,
            3 => Brushes.DarkOrange,
            4 => Brushes.DarkOrchid,
            5 => Brushes.DarkBlue,
            6 => Brushes.Yellow,
            7 => Brushes.DarkRed,
            _ => Brushes.DodgerBlue
        };
    }
}