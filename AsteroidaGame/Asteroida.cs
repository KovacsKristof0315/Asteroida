using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidaGame
{
    public class Asteroida
    {
        protected double _x;
        protected double _y;
        protected double _r;
        protected int _dir;
        private Random r = new Random();

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(nameof(X)); }
        }


        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(nameof(Y)); }
        }

        public int Dir
        {
            get => _dir;
            set { _dir = value; OnPropertyChanged(nameof(Dir)); }
        }

        public double R { get { return _r; } set { _r = value; OnPropertyChanged(nameof(R)); } }
        public Asteroida(double x, double y, double r, int dir)
        {
            _x = x;
            _y = y;
            _r = r;
            _dir = dir;
        }

        public bool calculateCollison(Asteroida other)
        {

            if (_x < other.X + _r && _y < other.Y + _r && _x > other.X - _r && _y > other.Y - _r)
            {
                _dir *= -1;
                if (other is Player)
                {
                    _x += r.Next(200, 600);
                    _y = 0;
                }
                return true;
            }
            return false;
        }

        public virtual void NewPosition(double speed)
        {
            if (_dir == -1)
                _x = _x < 0 ? 1200 : _x - speed;
            else
                _x = _x > 1200 ? 0 : _x + speed;
            _y = _y > 600 ? 0 : _y + speed;
        }
        public void RaisePositionChanged()
        {
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
