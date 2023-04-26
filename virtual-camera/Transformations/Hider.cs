using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace virtual_camera.Transformations;

/// Logic for hiding objects that are behind other objects in the scene.
public static class Hider
{
    // private static Matrix FindPlaneOfRectangle(Polygon polygon)
    // {
    //     var points = polygon.Points;
    //     if (points.Count != 4)
    //     {
    //         throw new ArgumentException("Rectangle must have 4 points");
    //     }
    //     
    //     var dx = points[1].X - points[0].X;
    //     var dy = points[1].Y - points[0].Y;
    //     var dz = points[1].Z - points[0].Z;
    //     //wrong spot here theres no Z
    // }
    
}