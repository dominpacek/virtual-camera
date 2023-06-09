﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using virtual_camera.Objects;

namespace virtual_camera.Logic;

public static class Projector
{

    public static IEnumerable<Cuboid> ProjectCuboids(IEnumerable<Cuboid> cuboids)
    {
        var projectedCuboids = cuboids.ToList();
        foreach (var cuboid in projectedCuboids)
        {
            cuboid.Project();
        }

        return projectedCuboids;
    }

    public static Point3D ProjectPoint(Point3D point)
    {
        var vpd = Camera.ViewPlaneDistance;
        var projectionMatrix = Matrices.GetProjectionMatrix(vpd);

        var pointVector = new DenseVector(new[] { point.X, point.Y, point.Z, 1 });

        var projectedVector = projectionMatrix * pointVector;

        var x = projectedVector[0];
        var y = projectedVector[1];
        var z = projectedVector[2];
        if (z == 0)
        {
            z = 1;
        }

        return new Point3D((x * vpd) / z, (y * vpd) / z, z);
    }
}