using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidaGame
{
    public class Player : Asteroida
    {
        private double _angle;

        public double Angle { get { return _angle; } set { _angle = value; } }

        public double _angel { get; set; }

        public Player(double x, double y) : base(x, y, 20, 0)
        {

        }

        public (double, double) Rotate((double X, double Y) origin, (int X, int Y) offset)
        {
            double cos = Math.Cos(_angle);
            double sin = Math.Sin(_angle);

            double rotatedX = offset.X * cos - offset.Y * sin;
            double rotatedY = offset.X * sin + offset.Y * cos;

            return ((origin.X + rotatedX), (origin.Y + rotatedY));
        }

        public override void NewPosition(double speed)
        {
            speed++;
            double dx = Math.Cos(_angle) * speed;
            double dy = Math.Sin(_angle) * speed;

            X += dx;
            Y += dy;

            X = (X + 1200) % 1200;
            Y = (Y + 600) % 600;
        }

    }
}
