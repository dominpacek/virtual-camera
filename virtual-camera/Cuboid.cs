using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using virtual_camera.Enums;
using virtual_camera.Transformations;

namespace virtual_camera;

public class Cuboid
{
    public readonly Point3D[] Vertices;
    public Polygon[] Walls;
    public readonly Color Color = Colors.CornflowerBlue;

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
        Vertices = vertices;
    }

    // Construct a cuboid by an array of vertices
    private Cuboid(Point3D[] vertices)
    {
        var n = vertices.Length;
        if (n != 8)
        {
            throw new System.ArgumentException($"Cuboid must have 8 vertices. Got {n}");
        }

        Vertices = vertices;
    }

    public Cuboid Project()
    {
        var projectedVertices = new List<Point3D>();
        foreach (var vertex in Vertices)
        {
            projectedVertices.Add(Projector.ProjectPoint(vertex));
        }
        var projectedCuboid = new Cuboid(projectedVertices.ToArray());
        projectedCuboid.GenerateWalls();
        return projectedCuboid;
    }
    
    public Cuboid Translate(CameraMoveDirection direction)
    {
        var translatedVertices = new List<Point3D>();
        foreach (var vertex in Vertices)
        {
            translatedVertices.Add(Translator.TranslatePoint(vertex, direction));
        }
        return new Cuboid(translatedVertices.ToArray());
    }

    public Cuboid Rotate(CameraRotation rotation)
    {
        var rotatedVertices = new List<Point3D>();
        foreach (var vertex in Vertices)
        {
            rotatedVertices.Add(Rotator.RotatePoint(vertex, rotation));
        }
        return new Cuboid(rotatedVertices.ToArray());
    }
    
    public void GenerateWalls()
    {
        Walls = new Polygon[6];

        Walls[0] = CreatePolygonWithVertices(0,1,2,3);
        Walls[1] = CreatePolygonWithVertices(0,1,6,7);
        Walls[2] = CreatePolygonWithVertices(0,3,4,7);
        Walls[3] = CreatePolygonWithVertices(1,2,5,6);
        Walls[4] = CreatePolygonWithVertices(2,3,4,5);
        Walls[5] = CreatePolygonWithVertices(4,5,6,7);
    }

    // Create a polygon with the given vertices (by index)
    private Polygon CreatePolygonWithVertices(int a, int b, int c, int d)
    {
        var poly = new Polygon();
        
        var points = new PointCollection
        {
            new Point(Vertices[a].X, Vertices[a].Y),
            new Point(Vertices[b].X, Vertices[b].Y),
            new Point(Vertices[c].X, Vertices[c].Y),
            new Point(Vertices[d].X, Vertices[d].Y)
        };

        poly.Points = points;
        return poly;
    }
}