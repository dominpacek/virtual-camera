using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using virtual_camera.Enums;
using virtual_camera.Transformations;

namespace virtual_camera;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Cuboid> _cuboids = FileReader.LoadScene(1);
    private List<Polygon> _walls = new List<Polygon>();


    public MainWindow()
    {
        InitializeComponent();
        Window.Width = CameraProperties.WINDOW_WIDTH;
        Window.Height = CameraProperties.WINDOW_HEIGHT;
        Render();
    }

    private void Render()
    {
        Canvas.Children.Clear();
        _walls.Clear();
        var projectedCuboids = Projector.ProjectCuboids(_cuboids);

        foreach (var cuboid in projectedCuboids)
        {
            foreach (var polygon in cuboid.GetWalls())
            {
                if (!CameraProperties.GetTransparentMode())
                {
                    polygon.Fill = polygon.Stroke;
                }
                _walls.Add(polygon);
            }
        }
        
        foreach (var wall in _walls)
        {
            Canvas.Children.Add(wall);
        }
    }
   
    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.T:
                CameraProperties.ToggleTransparentMode();
                break;
            case Key.W:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Forward);
                break;
            case Key.S:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Backward);
                break;
            case Key.A:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Left);
                break;
            case Key.D:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Right);
                break;
            case Key.LeftShift:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Down);
                break;
            case Key.Space:
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Up);
                break;
            case Key.U:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.CounterClockwise);
                break;
            case Key.I:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Up);
                break;
            case Key.O:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Clockwise);
                break;
            case Key.J:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Left);
                break;
            case Key.K:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Down);
                break;
            case Key.L:
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Right);
                break;
            case Key.OemPlus:
                CameraProperties.ZoomIn();
                break;
            case Key.OemMinus:
                CameraProperties.ZoomOut();
                break;
            case Key.D0:
                CameraProperties.ResetZoom();
                break;
            case Key.D1:
                _cuboids = FileReader.LoadScene(1);
                break;
            case Key.D2:
                _cuboids = FileReader.LoadScene(2);
                break;
            case Key.D3:
                _cuboids = FileReader.LoadScene(3);
                break;
            case Key.D4:
                _cuboids = FileReader.LoadScene(4);
                break;
            case Key.D9:
                _cuboids = FileReader.LoadScene(9);
                break;
        }
        Render();
    }
}