using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Tracers
{
    public class RayTracer : BaseTracer
    {
        public RayTracer(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public override bool IsCollision(Line collisionEdge)
        {
            var rayTraceCord = LineIntersectionPoint(new Line(StartPoint, EndPoint), collisionEdge);
            if (!double.IsNaN(rayTraceCord.X) && !double.IsNaN(rayTraceCord.Y))
            {
                if (RayLineCreated != null)
                    RayLineCreated(rayTraceCord, rayTraceCord, "XYLine", Name);
                return true;
            }
            return false;
        }
    }
}
