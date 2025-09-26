using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Diagnostics;

namespace Asteroida.Avalonia;

public partial class JoyStick : UserControl
{
    public static readonly StyledProperty<AsteroidaGame.Player> PlayerProperty =
    AvaloniaProperty.Register<JoyStick, AsteroidaGame.Player>(nameof(Player));

    public AsteroidaGame.Player Player
    {
        get => GetValue(PlayerProperty);
        set => SetValue(PlayerProperty, value);
    }

    private Point _center;
    private Point _currentPoint;
    private bool _isDragging = false;
    private double _radius = 80;


    public JoyStick()
    {
        InitializeComponent();
        _center = new Point(100, 100);
        _currentPoint = _center;
        this.AttachedToVisualTree += (_, _) => InvalidateVisual();

    }

    public override void Render(DrawingContext context)
    {
        //Player.Angle = 90;
        base.Render(context);
        var brush = Brushes.Gray;
        var knobBrush = Brushes.Red;

        context.DrawEllipse(brush, null, _center, _radius, _radius);
        context.DrawEllipse(knobBrush, null, _currentPoint, 30, 30);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _isDragging = true;
        UpdatePosition(e.GetPosition(this));
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_isDragging)
        {
            UpdatePosition(e.GetPosition(this));
            Player.Angle = Math.Atan2(_currentPoint.Y - _center.Y, _currentPoint.X - _center.X) * 180 / Math.PI + 90;
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
        _currentPoint = _center;
        InvalidateVisual();
    }

    private void UpdatePosition(Point point)
    {
        var dx = point.X - _center.X;
        var dy = point.Y - _center.Y;
        var distance = Math.Sqrt(dx * dx + dy * dy);
        if (distance > _radius)
        {
            var scale = _radius / distance;
            dx *= scale;
            dy *= scale;
        }
        _currentPoint = new Point(_center.X + dx, _center.Y + dy);
        InvalidateVisual();

        var direction = new Vector(dx / _radius, dy / _radius);
        Debug.WriteLine($"Joystick Direction: {direction}");
    }


}