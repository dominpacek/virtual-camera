namespace virtual_camera;

public static class Camera
{
    public static double WindowWidth { get; set; } = 1200;
    public static double WindowHeight { get; set; } = 800;

    public const double TranslationStep = 25;
    public const double RotationDegrees = 3;
    private const int ZoomStep = 100;

    private const int DefaultVpd = 800;
    public static int ViewPlaneDistance { get; private set; } = DefaultVpd;
    private static double _zoomPercent;
    

    public static bool TransparentMode { get; private set; }


    public static void ZoomIn()
    {
        if (ViewPlaneDistance >= DefaultVpd * 4) return;
        _zoomPercent += ZoomStep / (DefaultVpd * 0.01);
        ViewPlaneDistance += ZoomStep;
    }

    public static void ZoomOut()
    {
        if (ViewPlaneDistance <= DefaultVpd / 2) return;
        _zoomPercent -= ZoomStep / (DefaultVpd * 0.01);
        ViewPlaneDistance -= ZoomStep;
    }

    public static void ResetZoom()
    {
        _zoomPercent = 0;
        ViewPlaneDistance = DefaultVpd;
    }

    public static void ToggleTransparentMode()
    {
        TransparentMode = !TransparentMode;
    }

    public static string GetZoomDisplay()
    {
        return $"{_zoomPercent}%";
    }
    
}