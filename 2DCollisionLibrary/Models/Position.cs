using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Models
{
    public class Position : IPosition
    {
        public Point Point { get; set; }
        public double X { get { return Point.X; } }
        public double Y { get { return Point.Y; } }

        public Position()
        {
            Point = new Point();
        }

        public Position(double x, double y)
        {
            Point = new Point(x, y);
        }

        public Position(Point point)
        {
            Point = point;
        }
    }
}
