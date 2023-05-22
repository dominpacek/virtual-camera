using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using virtual_camera.Enums;
using virtual_camera.Transformations;

namespace virtual_camera;

public class Cuboid : IComparable<Cuboid>
{
    private readonly Point3D[] _vertices;
    private List<Wall> _walls;
    private Brush _edgeBrush = Brushes.CornflowerBlue;
    private Brush _wallBrush = Brushes.DodgerBlue;

    // Construct a cuboid by two opposite points
    // for loading from file
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
        _vertices = vertices;
    }

    // Construct a cuboid by an array of vertices
    private Cuboid(Point3D[] vertices)
    {
        var n = vertices.Length;
        if (n != 8)
        {
            throw new ArgumentException($"Cuboid must have 8 vertices. Got {n}");
        }

        _vertices = vertices;
    }

    public void Project()
    {
        _walls = new List<Wall>(6);

        _walls.Add(CreateWallWithVertices(0, 1, 2, 3));
        _walls.Add(CreateWallWithVertices(0, 1, 6, 7));
        _walls.Add(CreateWallWithVertices(0, 3, 4, 7));
        _walls.Add(CreateWallWithVertices(1, 2, 5, 6));
        _walls.Add(CreateWallWithVertices(2, 3, 4, 5));
        _walls.Add(CreateWallWithVertices(4, 5, 6, 7));
        
        _walls.Sort();
    }

    public Cuboid Translate(CameraMoveDirection direction)
    {
        var translatedVertices = new List<Point3D>();
        foreach (var vertex in _vertices)
        {
            translatedVertices.Add(Translator.TranslatePoint(vertex, direction));
        }

        return new Cuboid(translatedVertices.ToArray()) { _edgeBrush = _edgeBrush, _wallBrush = _wallBrush};
    }

    public Cuboid Rotate(CameraRotation rotation)
    {
        var rotatedVertices = new List<Point3D>();
        foreach (var vertex in _vertices)
        {
            rotatedVertices.Add(Rotator.RotatePoint(vertex, rotation));
        }

        return new Cuboid(rotatedVertices.ToArray()) { _edgeBrush = _edgeBrush, _wallBrush = _wallBrush};
    }
    

    private Wall CreateWallWithVertices(int indexA, int indexB, int indexC, int indexD)
    {
        var poly = CreatePolygonWithVertices(indexA, indexB, indexC, indexD);
        var points = new List<Point3D>()
        {
            _vertices[indexA],
            _vertices[indexB],
            _vertices[indexC],
            _vertices[indexD]
        };

        return new Wall(poly, points);
    }
    
    // Create a polygon with the given vertices (by index)
    private Polygon CreatePolygonWithVertices(int indexA, int indexB, int indexC, int indexD)
    {
        var a = Projector.ProjectPoint(_vertices[indexA]);
        var b = Projector.ProjectPoint(_vertices[indexB]);
        var c = Projector.ProjectPoint(_vertices[indexC]);
        var d = Projector.ProjectPoint(_vertices[indexD]);

        var verticesOutOfSight = 0;
        if (a.Z < 0) verticesOutOfSight++;
        if (b.Z < 0) verticesOutOfSight++;
        if (c.Z < 0) verticesOutOfSight++;
        if (d.Z < 0) verticesOutOfSight++;

        if (verticesOutOfSight > 0)
        {
            return new Polygon();
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

        return poly;
    }

    public List<Wall> GetWalls()
    {
        return _walls;
    }

    public void SetColor(int i)
    {
        _edgeBrush = (i % 8) switch
        {
            1 => Brushes.YellowGreen,
            2 => Brushes.ForestGreen,
            3 => Brushes.OrangeRed,
            4 => Brushes.MediumOrchid,
            5 => Brushes.CornflowerBlue,
            6 => Brushes.Gold,
            7 => Brushes.Red,
            _ => Brushes.Blue
        };
        _wallBrush = (i % 8) switch
        {
            1 => Brushes.GreenYellow,
            2 => Brushes.Green,
            3 => Brushes.DarkOrange,
            4 => Brushes.DarkOrchid,
            5 => Brushes.DodgerBlue,
            6 => Brushes.Yellow,
            7 => Brushes.DarkRed,
            _ => Brushes.DarkBlue
        };
    }
    
    private double GetCenterZ()
    {
        double sum = 0;
        foreach (var wall in _walls)
        {
            foreach (var vertex in wall.Points)
            {
                sum += vertex.Z;
            }

        }

        return sum / 6*4;
    }
    
    public int CompareTo(Cuboid? other)
    {
        if (GetCenterZ() > other.GetCenterZ()) return -1;
        return 1;
    }
    
}