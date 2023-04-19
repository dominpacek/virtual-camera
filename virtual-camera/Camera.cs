using System.Collections.Generic;

namespace virtual_camera;

public class Camera
{
    public const int WINDOW_WIDTH = 1200;
    public const int WINDOW_HEIGHT = 800;
    public const double TranslationStep = 10;
    public const double RotationDegrees = 3;
    private static double _viewPlaneDistance = 1000;
    private const double vpdStep = 100;

    private List<Cuboid> _cuboids = FileReader.LoadScene(1);

    public static double GetViewPlaneDistance()
    {
        return _viewPlaneDistance;
    }

    public static void ZoomIn()
    {
        if (_viewPlaneDistance < 3000)
        {
            _viewPlaneDistance += vpdStep;
        }
    }

    public static void ZoomOut()
    {
        if (_viewPlaneDistance > 300)
        {
            _viewPlaneDistance -= vpdStep;
        }
    }
}