﻿using System.Collections.Generic;
using System.Windows.Media.Media3D;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using virtual_camera.Enums;

namespace virtual_camera.Transformations;

public static class Rotator
{

    public static List<Cuboid> RotateCuboids(IEnumerable<Cuboid> cuboids, CameraRotation rotation)
    {
        var rotatedCuboids = new List<Cuboid>();
        foreach (var cuboid in cuboids)
        {
            rotatedCuboids.Add(cuboid.Rotate(rotation));
        }

        return rotatedCuboids;
    }


    public static Point3D RotatePoint(Point3D point, CameraRotation rotation)
    {
        var rotationMatrix = Matrices.GetRotationMatrix(rotation, Camera.RotationDegrees);

        Vector<double> pointVector = new DenseVector(new double[] { point.X, point.Y, point.Z, 1 });
        Vector<double> rotatedVector = rotationMatrix * pointVector;

        var resultPoint = new Point3D(rotatedVector[0], rotatedVector[1], rotatedVector[2]);
        return resultPoint;
    }
}