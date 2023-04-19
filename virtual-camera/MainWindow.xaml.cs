using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using virtual_camera.Enums;
using virtual_camera.Transformations;

namespace virtual_camera;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Cuboid> _cuboids = FileReader.LoadScene(1);


    public MainWindow()
    {
        InitializeComponent();
        Window.Width = Camera.WINDOW_WIDTH;
        Window.Height = Camera.WINDOW_HEIGHT;
        CompositionTarget.Rendering += Render;
    }

    private void Render(Object? sender, EventArgs e)
    {
        Canvas.Children.Clear();
        var projectedCuboids = Projector.ProjectCuboids(_cuboids);

        foreach (var cuboid in projectedCuboids)
        {
            var w1 = cuboid.Walls[1].Points;
            var w4 = cuboid.Walls[4].Points;
            
            foreach (var polygon in cuboid.Walls)
            {
                polygon.Stroke = Brushes.CornflowerBlue;
                Canvas.Children.Add(polygon);
            }
        }
    }
   
    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
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
                Camera.ZoomIn();
                break;
            case Key.OemMinus:
                Camera.ZoomOut();
                break;
        }
    }
}