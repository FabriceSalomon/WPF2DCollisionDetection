using _2DCollisionLibrary.Collision;
using _2DCollisionLibrary.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    internal interface ITracer
    {
        string Name { get; set; }
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }
        Action<Point, Point, CollissionType, string> RayLineCreated { get; set; }

        double Distance { get; }

        bool IsCollision(Line collisionLine);
    }
}
