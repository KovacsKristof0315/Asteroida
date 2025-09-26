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

namespace Asteroida.Avalonia
{
    public class GameCanvas : Control
    {
        public static readonly StyledProperty<IEnumerable<AsteroidaGame.Asteroida>> ItemsProperty =
        AvaloniaProperty.Register<GameCanvas, IEnumerable<AsteroidaGame.Asteroida>>(nameof(Items));

        private AsteroidaGame.Player player = null!;
        public IEnumerable<AsteroidaGame.Asteroida> Items
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

            context.DrawRectangle(Brushes.Black, null, new Rect(Bounds.Size));

            if (Items != null)
            {
                foreach (var asteroid in Items.Where(x => !(x is AsteroidaGame.Player)))
                {
                    context.DrawEllipse(
                        Brushes.Gray,
                        new Pen(Brushes.White, 1),
                        new Point(asteroid.X, asteroid.Y),
                       asteroid.R, asteroid.R);
                }

                player = (AsteroidaGame.Player)Items.Last();

                var geo = new StreamGeometry();
                using (var ctx = geo.Open())
                {

                    (double, double) ppont = player.Rotate((player.X, player.Y), (0, -10));
                    Point pont1 = new Point(ppont.Item1, ppont.Item2);
                    ctx.BeginFigure(pont1, true);

                    ppont = player.Rotate((player.X, player.Y), (8, 10));
                    pont1 = new Point(ppont.Item1, ppont.Item2);
                    ctx.LineTo(pont1);

                    ppont = player.Rotate((player.X, player.Y), (-8, 10));
                    pont1 = new Point(ppont.Item1, ppont.Item2);
                    ctx.LineTo(pont1);
                    ctx.EndFigure(true);
                }
                geo.Transform = new RotateTransform(90, player.X, player.Y); // rotate 45° around player.X/Y

                context.DrawGeometry(Brushes.White, new Pen(Brushes.White), geo);
            }

        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == ItemsProperty)
            {
                if (change.OldValue is INotifyCollectionChanged oldColl)
                    oldColl.CollectionChanged -= OnItemsChanged;

                if (change.NewValue is INotifyCollectionChanged newColl)
                    newColl.CollectionChanged += OnItemsChanged;

                InvalidateVisual();
            }
        }

        private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
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

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            timer.Tick += (_, _) => InvalidateVisual();
            timer.Start();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.A) player.Angle -= 0.1;
            if (e.Key == Key.Right || e.Key == Key.D) player.Angle += 0.1;
        }
    }
}
