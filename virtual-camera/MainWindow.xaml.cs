using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
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
    private List<Wall> _walls = new List<Wall>();
    

    public MainWindow()
    {
        InitializeComponent();
        Render();
        // CompositionTarget.Rendering += Rotato;
    }

    private void Rotato(object? sender, EventArgs e)
    {
        _cuboids[0] = _cuboids[0].Rotate(CameraRotation.CounterClockwise);
        Render();
    }
    
    private void Render()
    {
        Canvas.Children.Clear();
        _walls.Clear();
        var projectedCuboids = Projector.ProjectCuboids(_cuboids);

        foreach (var cuboid in projectedCuboids)
        {
            foreach (var wall in cuboid.GetWalls())
            {
                var polygon = wall.Projection;
                if (Camera.TransparentMode)  polygon.Fill = null;
                _walls.Add(wall);
            }
        }
        
        foreach (var wall in _walls)
        {
            Canvas.Children.Add(wall.Projection);
        }
        
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        ZoomBox.Text = $"Zoom: {Camera.GetZoomDisplay()}";
        CoordBox.Text = $"X: {Camera.X}, Y: {Camera.Y}, Z: {Camera.Z}";
        RotationBox.Text = $"Rotation: X{Camera.XRotation}°, Y{Camera.YRotation}°, Z{Camera.ZRotation}°";
    }
    
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        Camera.WindowWidth = sizeInfo.NewSize.Width;
        Camera.WindowHeight = sizeInfo.NewSize.Height;
    }
   
    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.T:
                Camera.ToggleTransparentMode();
                break;
            case Key.W:
                Camera.Z += Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Forward);
                break;
            case Key.S:
                Camera.Z -= Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Backward);
                break;
            case Key.A:
                Camera.X -= Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Left);
                break;
            case Key.D:
                Camera.X += Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Right);
                break;
            case Key.LeftShift:
                Camera.Y -= Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Down);
                break;
            case Key.Space:
                Camera.Y += Camera.TranslationStep;
                _cuboids = Translator.TranslateCuboids(_cuboids, CameraMoveDirection.Up);
                break;
            case Key.U:
                Camera.ZRotation -= Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.CounterClockwise);
                break;
            case Key.I:
                Camera.XRotation += Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Up);
                break;
            case Key.O:
                Camera.ZRotation += Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Clockwise);
                break;
            case Key.J:
                Camera.YRotation -= Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Left);
                break;
            case Key.K:
                Camera.XRotation -= Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Down);
                break;
            case Key.L:
                Camera.YRotation += Camera.RotationDegrees;
                _cuboids = Rotator.RotateCuboids(_cuboids, CameraRotation.Right);
                break;
            case Key.OemPlus:
                Camera.ZoomIn();
                break;
            case Key.OemMinus:
                Camera.ZoomOut();
                break;
            case Key.D0:
                Camera.ResetZoom();
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