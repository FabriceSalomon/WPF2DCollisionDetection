using _2DCollisionLibrary.Collision;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Tracers
{
    public abstract class BaseTracer
    {
        public string Name { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Action<Point, Point, CollissionType, string> RayLineCreated { get; set; }

        public Double Distance
        {
            get
            {
                return StartPoint.Distance(EndPoint);
            }
        }

        public abstract bool IsCollision(Line collisionLine);

        protected virtual Point LineIntersectionPoint(Line line1, Line line2)
        {
            var lines = new Line[] { line1, line2 }.OrderByDescending(p => Math.Abs(p.StartPoint.X - p.EndPoint.X)).ToArray();
            var line = lines[0];
            var staticLine = lines[1];

            if (line.EndPoint.X < line.StartPoint.X)
                line = new Line(line.EndPoint, line.StartPoint);
            if (staticLine.EndPoint.X < staticLine.StartPoint.X)
                staticLine = new Line(staticLine.EndPoint, staticLine.StartPoint);

            var staticLineOpp = staticLine.EndPoint.Y - staticLine.StartPoint.Y;
            var staticLineAdj = staticLine.EndPoint.X - staticLine.StartPoint.X;
            var staticLineAccelerationRate = staticLineOpp / Math.Abs(staticLineAdj);

            var lineOpp = line.EndPoint.Y - line.StartPoint.Y;
            var lineAdj = line.EndPoint.X - line.StartPoint.X;
            var lineAccelerationRate = lineOpp / Math.Abs(lineAdj);

            //When the lines have the same xCord we can start comparing how fast the gap between them is closing and where they will collide.
            var distance = staticLine.StartPoint.X - line.StartPoint.X;
            var heightDifference = (line.StartPoint.Y + (distance * lineAccelerationRate)) - staticLine.StartPoint.Y;
            var heightLossRatio = staticLineAccelerationRate - lineAccelerationRate;

            var xIntersectionPoint = staticLine.StartPoint.X + (heightDifference / heightLossRatio);
            var yIntersectionPoint = line.StartPoint.Y + (((heightDifference / heightLossRatio) + distance) * lineAccelerationRate);

            if (!xIntersectionPoint.ValueInRange(staticLine.StartPoint.X, staticLine.EndPoint.X, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!yIntersectionPoint.ValueInRange(staticLine.StartPoint.Y, staticLine.EndPoint.Y, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!xIntersectionPoint.ValueInRange(line.StartPoint.X, line.EndPoint.X, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!yIntersectionPoint.ValueInRange(line.StartPoint.Y, line.EndPoint.Y, 0.2f))
                return new Point(double.NaN, double.NaN);
            else
                return new Point(xIntersectionPoint, yIntersectionPoint);
        }
    }
}
