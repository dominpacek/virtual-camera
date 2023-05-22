using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media.Media3D;

namespace virtual_camera.Transformations;

/// Logic for hiding objects that are behind other objects in the scene.
public static class PlaneMath
{
    public static int IsPointInFrontOfPlane(Point3D point, List<Point3D> plane)
    {
        // -1 means that the point is behind the plane from the camera's perspective
        // 1 means that the point is on the observer side

        var sameSide = CheckIfSameSideAsOrigin(GetPlaneFromPoints(plane), point);
        if (sameSide) return 1;

        return -1;
    }

    private static bool CheckIfSameSideAsOrigin(Vector4 plane, Point3D point)
    {
        var pointVector = new Vector4((float)point.X, (float)point.Y, (float)point.Z, 1);

        var resultPoint = Vector4.Dot(plane, pointVector);
        var resultOrigin = Vector4.Dot(plane, new Vector4(0, 0, 0, 1));

        return (resultPoint > 0) == (resultOrigin > 0);
    }


    private static Vector4 GetPlaneFromPoints(List<Point3D> plane)
    {
        var x1 = (float)plane[0].X;
        var y1 = (float)plane[0].Y;
        var z1 = (float)plane[0].Z;

        var x2 = (float)plane[1].X;
        var y2 = (float)plane[1].Y;
        var z2 = (float)plane[1].Z;

        var x3 = (float)plane[2].X;
        var y3 = (float)plane[2].Y;
        var z3 = (float)plane[2].Z;

        var a1 = x2 - x1;
        var b1 = y2 - y1;
        var c1 = z2 - z1;
        var a2 = x3 - x1;
        var b2 = y3 - y1;
        var c2 = z3 - z1;
        var a = b1 * c2 - b2 * c1;
        var b = a2 * c1 - a1 * c2;
        var c = a1 * b2 - b1 * a2;
        var d = (-a * x1 - b * y1 - c * z1);

        return new Vector4(a, b, c, d);
    }
}