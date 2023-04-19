﻿using System;
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
    private readonly Point3D[] _vertices;
    public List<Polygon> Walls;
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

    public Cuboid Project()
    {
        var projectedVertices = new List<Point3D>();
        foreach (var vertex in _vertices)
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
        foreach (var vertex in _vertices)
        {
            translatedVertices.Add(Translator.TranslatePoint(vertex, direction));
        }
        return new Cuboid(translatedVertices.ToArray());
    }

    public Cuboid Rotate(CameraRotation rotation)
    {
        var rotatedVertices = new List<Point3D>();
        foreach (var vertex in _vertices)
        {
            rotatedVertices.Add(Rotator.RotatePoint(vertex, rotation));
        }
        return new Cuboid(rotatedVertices.ToArray());
    }
    
    // Only for projected cuboids
    private void GenerateWalls()
    {
        Walls = new List<Polygon>(6);

        CreatePolygonWithVertices(0,1,2,3);
        CreatePolygonWithVertices(0,1,6,7);
        CreatePolygonWithVertices(0,3,4,7);
        CreatePolygonWithVertices(1,2,5,6);
        CreatePolygonWithVertices(2,3,4,5);
        CreatePolygonWithVertices(4,5,6,7);
    }

    // Create a polygon with the given vertices (by index)
    private void CreatePolygonWithVertices(int indexA, int indexB, int indexC, int indexD)
    {
        var a = _vertices[indexA];
        var b = _vertices[indexB];
        var c = _vertices[indexC];
        var d = _vertices[indexD];
        

        if (a.Z < 0 || b.Z < 0 || c.Z < 0 || d.Z < 0)
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
            Points = points
        };
        Walls.Add(poly);
    }
}