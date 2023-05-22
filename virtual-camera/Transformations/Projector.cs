using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace virtual_camera.Transformations;

public static class Projector
{

    public static IEnumerable<Cuboid> ProjectCuboids(IEnumerable<Cuboid> cuboids)
    {
        var projectedCuboids = cuboids.ToList();
        foreach (var cuboid in projectedCuboids)
        {
            cuboid.Project();
        }
        projectedCuboids.Sort();

        return projectedCuboids;
    }

    public static Point3D ProjectPoint(Point3D point)
    {
        var vpd = Camera.ViewPlaneDistance;
        var projectionMatrix = Matrices.GetProjectionMatrix(vpd);

        Vector<double> pointVector = new DenseVector(new[] { point.X, point.Y, point.Z, 1 });

        Vector<double> projectedVector = projectionMatrix * pointVector;

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