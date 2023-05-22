using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using virtual_camera.Enums;

namespace virtual_camera;

public static class Matrices
{
    private static int _lastVpd;
    private static Matrix<double> _lastProjectionMatrix;
    
    public static Matrix<double> GetProjectionMatrix(int vpd)
    {
        if (_lastVpd == vpd)
        {
            return _lastProjectionMatrix;
        }

        _lastVpd = vpd;
        _lastProjectionMatrix = Matrix<double>.Build.DenseOfArray(new[,]
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 1.0 / vpd, 0 }
        });
        return _lastProjectionMatrix;
    }

    private static Matrix<double> GetTranslationMatrix(double dx, double dy, double dz)
    {
        return Matrix<double>.Build.DenseOfArray(new[,]
        {
            { 1, 0, 0, dx },
            { 0, 1, 0, dy },
            { 0, 0, 1, dz },
            { 0, 0, 0, 1 }
        });
    }

    public static Matrix<double> GetTranslationMatrix(CameraMoveDirection direction, double step)
    {
        switch (direction)
        {
            case CameraMoveDirection.Left:
                return GetTranslationMatrix(step, 0, 0);
            case CameraMoveDirection.Right:
                return GetTranslationMatrix(-step, 0, 0);
            case CameraMoveDirection.Up:
                return GetTranslationMatrix(0, step, 0);
            case CameraMoveDirection.Down:
                return GetTranslationMatrix(0, -step, 0);
            case CameraMoveDirection.Backward:
                return GetTranslationMatrix(0, 0, step);
            case CameraMoveDirection.Forward:
                return GetTranslationMatrix(0, 0, -step);
            default:
                Debug.WriteLine($"Invalid direction in GetTranslationMatrix: {direction}");
                return GetTranslationMatrix(0, 0, 0);
        }
    }

    public static Matrix<double> GetRotationMatrix(CameraRotation rotation, double degrees)
    {
        if (rotation is CameraRotation.Up or CameraRotation.Right or CameraRotation.Clockwise)
        {
            degrees = -degrees;
        }

        var angle = (Math.PI / 180) * degrees;
        var cos = double.Cos(angle);
        var sin = double.Sin(angle);

        return rotation switch
        {
            CameraRotation.Left or CameraRotation.Right => Matrix<double>.Build.DenseOfArray(new[,]
            {
                { cos, 0, sin, 0 },
                { 0, 1, 0, 0 },
                { -sin, 0, cos, 0 },
                { 0, 0, 0, 1 }
            }),
            CameraRotation.Up or CameraRotation.Down => Matrix<double>.Build.DenseOfArray(new[,]
            {
                { 1, 0, 0, 0 },
                { 0, cos, -sin, 0 },
                { 0, sin, cos, 0 },
                { 0, 0, 0, 1 }
            }),
            _ => Matrix<double>.Build.DenseOfArray(new[,]
            {
                { cos, -sin, 0, 0 },
                { sin, cos, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            })
        };
    }
}