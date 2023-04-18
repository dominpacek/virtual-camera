using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace virtual_camera;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Line line;

    public MainWindow()
    {
        InitializeComponent();
        FileReader.LoadScene(1);
    }
    
    
}