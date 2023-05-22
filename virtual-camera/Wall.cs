using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace virtual_camera;

public class Wall
{
    public readonly Polygon Projection;
    public readonly List<Point3D> Points;

    public Wall(Polygon projection, List<Point3D> points)
    {
        Projection = projection;
        Points = points;
    }
    
}