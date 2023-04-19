using System.Collections.Generic;
using System.Windows.Shapes;

namespace virtual_camera;

public static class CameraProperties
{
    public const int WINDOW_WIDTH = 1200;
    public const int WINDOW_HEIGHT = 800;
    public const double TranslationStep = 25;
    public const double RotationDegrees = 3;
    private static int _viewPlaneDistance = 800;
    private const int VpdStep = 100;
    private static bool _transparentMode = true;
    
    
    public static int GetViewPlaneDistance()
    {
        return _viewPlaneDistance;
    }

    public static void ZoomIn()
    {
        if (_viewPlaneDistance < 3000)
        {
            _viewPlaneDistance += VpdStep;
        }
    }

    public static void ZoomOut()
    {
        if (_viewPlaneDistance > 300)
        {
            _viewPlaneDistance -= VpdStep;
        }
    }
    
    public static void ResetZoom()
    {
        _viewPlaneDistance = 800;
    }
    
    public static void ToggleTransparentMode()
    {
        _transparentMode = !_transparentMode;
    }
    
    public static bool GetTransparentMode()
    {
        return _transparentMode;
    }
}