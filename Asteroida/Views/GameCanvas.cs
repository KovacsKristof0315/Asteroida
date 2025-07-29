using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asteroida.Avalonia.ViewModels;
using System.Reflection.Metadata;

namespace Asteroida
{
    public class GameCanvas : Control
    {
        public static readonly StyledProperty<IEnumerable<tAsteroida>> ItemsProperty =
        AvaloniaProperty.Register<GameCanvas, IEnumerable<tAsteroida>>(nameof(Items));
        private Player player = null!;
        public IEnumerable<tAsteroida> Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
   
        public GameCanvas()
        {
            Focusable = true;
            this.KeyDown += OnKeyDown;
          
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // Background
            context.DrawRectangle(Brushes.Black, null, new Rect(Bounds.Size));

            // Draw all asteroids
            if (Items != null)
            {
                foreach (var asteroid in Items.Where(x=>  !(x is Player)))
                {
                    context.DrawEllipse(
                        Brushes.Gray,
                        new Pen(Brushes.White, 1),
                        new Point(asteroid.X, asteroid.Y),
                        20, 20);
                }
                player = (Player)Items.Last();

                var geo = new StreamGeometry();
                using (var ctx = geo.Open())
                {
                    ctx.BeginFigure(Rotate(player.PlayerPosition, new Vector(0, -10), player.Angle), true);
                    ctx.LineTo(Rotate(player.PlayerPosition, new Vector(8, 10), player.Angle));
                    ctx.LineTo(Rotate(player.PlayerPosition, new Vector(-8, 10), player.Angle));
                    ctx.EndFigure(true);
                }

                context.DrawGeometry(Brushes.White, new Pen(Brushes.White), geo);

                var dir = new Vector(Math.Cos(player.Angle), Math.Sin(player.Angle)) * 2;
                player.PlayerPosition += dir;

                player.PlayerPosition = new Point(
                    (player.PlayerPosition.X + Bounds.Width) % Bounds.Width,
                    (player.PlayerPosition.Y + Bounds.Height) % Bounds.Height
                );

            }

        }

        private Point Rotate(Point origin, Vector offset, double _angle)
        {
            var cos = Math.Cos(_angle);
            var sin = Math.Sin(_angle);
            return origin + new Vector(
                offset.X * cos - offset.Y * sin,
                offset.X * sin + offset.Y * cos
            );
            
        }
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == ItemsProperty)
            {
                InvalidateVisual(); 
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if (Items is INotifyCollectionChanged coll)
            {
                coll.CollectionChanged += (_, _) => InvalidateVisual();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Regular redraw for animation
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            timer.Tick += (_, _) => InvalidateVisual();
            timer.Start();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) player.Angle -= 0.1;
            if (e.Key == Key.Right) player.Angle += 0.1;
        }
    }
}
