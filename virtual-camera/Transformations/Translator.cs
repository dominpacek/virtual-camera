using System.Collections.Generic;
using System.Windows.Media.Media3D;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using virtual_camera.Enums;

namespace virtual_camera.Transformations;

public static class Translator
{

    public static List<Cuboid> TranslateCuboids(IEnumerable<Cuboid> cuboids, CameraMoveDirection direction)
    {
        var translatedCuboids = new List<Cuboid>();
        foreach (var cuboid in cuboids)
        {
            translatedCuboids.Add(cuboid.Translate(direction));
        }

        return translatedCuboids;
    }


    
    public static Point3D TranslatePoint(Point3D point, CameraMoveDirection direction)
    {
        var translationMatrix = Matrices.GetTranslationMatrix(direction, Camera.TranslationStep);
        
        Vector<double> pointVector = new DenseVector(new[] {point.X, point.Y, point.Z, 1});
        Vector<double> translatedVector = translationMatrix * pointVector;

        var resultPoint = new Point3D(translatedVector[0], translatedVector[1], translatedVector[2]);
        return resultPoint;

    }
}