using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media.Media3D;
using virtual_camera.Objects;

namespace virtual_camera.Logic;

/*
 * Logic for hiding objects that are behind other objects in the scene.
*/
public static class PlaneMath
{
    public static int IsPointInFrontOfPlaneFromOrigin(Point3D point, List<Point3D> planeCoordinates)
    {
        // -1 means that the point is behind the plane from the camera's perspective
        // 1 means that the point is on the observer side
        // 0 -> point is on the plane

        var plane = GetPlaneFromPoints(planeCoordinates);

        var pointVector = new Vector4((float)point.X, (float)point.Y, (float)point.Z, 1);

        var resultPoint = Vector4.Dot(plane, pointVector);

        if (resultPoint == 0)
        {
            return 0;
        }

        var resultOrigin = Vector4.Dot(plane, new Vector4(0, 0, 0, 1));

        if ((resultPoint > 0) == (resultOrigin > 0)) return 1;
        return -1;
    }

    private static int IsPointInFrontOfPlane(Point3D point, Vector4 plane)
    {
        // 1 - in front
        // 0 - on plane
        // -1 - behind plane

        var pointVector = new Vector4((float)point.X, (float)point.Y, (float)point.Z, 1);

        var result = Vector4.Dot(plane, pointVector);

        return result switch
        {
            0 => 0,
            > 1 => 1,
            _ => -1
        };
    }

    public static bool IsWallFullyBehindPlane(Wall wall, Vector4 plane)
    {
        var points = wall.Points;
        var a = IsPointInFrontOfPlane(points[0], plane);
        var b = IsPointInFrontOfPlane(points[1], plane);
        var c = IsPointInFrontOfPlane(points[2], plane);
        var d = IsPointInFrontOfPlane(points[3], plane);

        return a <= 0 && b <= 0 && c <= 0 && d <= 0;
    }
    
    public static bool IsWallFullyInFrontOfPlane(Wall wall, Vector4 plane)
    {
        var points = wall.Points;
        var a = IsPointInFrontOfPlane(points[0], plane);
        var b = IsPointInFrontOfPlane(points[1], plane);
        var c = IsPointInFrontOfPlane(points[2], plane);
        var d = IsPointInFrontOfPlane(points[3], plane);

        return a >= 0 && b >= 0 && c >= 0 && d >= 0;
    }


    public static Point3D? GetIntersectionOfEdgeAndPlane(Vector4 plane, Point3D e1, Point3D e2)
    {
        var a = plane.X;
        var b = plane.Y;
        var c = plane.Z;
        var d = plane.W;

        var dirX = e2.X - e1.X;
        var dirY = e2.Y - e1.Y;
        var dirZ = e2.Z - e1.Z;

        var dotProduct = a * dirX + b * dirY + c * dirZ;

        if (Math.Abs(dotProduct) < double.Epsilon)
        {
            // the line is parallel to the plane 
            return null;
        }

        var t = -((a * e1.X + b * e1.Y + c * e1.Z + d) / dotProduct);

        // calculate the intersection point
        var intersectX = e1.X + t * dirX;
        var intersectY = e1.Y + t * dirY;
        var intersectZ = e1.Z + t * dirZ;

        return new Point3D(intersectX, intersectY, intersectZ);
    }
    
    public static double PlaneOffset(Point3D point, List<Point3D> planePoints)
    {
        var plane = GetPlaneFromPoints(planePoints);
        return (plane.X * point.X + plane.Y * point.Y + plane.Z * point.Z + plane.W) * plane.W;
    }
    


    public static Vector4 GetPlaneFromPoints(List<Point3D> plane)
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