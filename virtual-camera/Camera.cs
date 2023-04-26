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
    private static double _zoomPercent = 0;
    
    public static double X { get; set; } = 0;
    public static double Y { get; set; } = 0;
    public static double Z { get; set; } = 0;
    
    public static double XRotation { get; set; } = 0;
    public static double YRotation { get; set; } = 0;
    public static double ZRotation { get; set; } = 0;



    public static bool TransparentMode { get; private set; } = true;


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

    public static void ResetPosition()
    {
        X = 0;
        Y = 0;
        Z = 0;
        XRotation = 0;
        YRotation = 0;
        ZRotation = 0;
        ResetZoom();
        _zoomPercent = 0;
    }
}