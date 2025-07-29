using Asteroida.Avalonia;
using Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroida.Avalonia.ViewModels
{
    public class Player : tAsteroida
    {
        private Point _playerPosition = new(400, 300);
        private double _angle;


        public Point PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
        public double Angle { get { return _angle; } set { _angle = value; } }


        public Player()
        {

        }

        public Player(double x, double y)
        {
            _playerPosition = new(x, y);
        }
    }
}
