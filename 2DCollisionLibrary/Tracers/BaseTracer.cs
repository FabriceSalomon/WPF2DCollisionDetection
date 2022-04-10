using _2DCollisionLibrary.Collision;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Tracers
{
    public abstract class BaseTracer : ITracer
    {
        public string Name { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Action<Point, Point, CollissionType, string> RayLineCreated { get; set; }

        public double Distance
        {
            get
            {
                return StartPoint.Distance(EndPoint);
            }
        }

        public abstract bool IsCollision(Line collisionLine);

        private void GetStaticLine(out ILine staticLine, out ILine collissionLine, params ILine[] lines)
        {
            lines = lines.OrderByDescending(p => Math.Abs(p.StartPoint.X - p.EndPoint.X)).ToArray();
            collissionLine = lines.First();
            staticLine = lines.Last();
        }
        private void ReArrangeLine(ILine line)
        {
            if (line.EndPoint.X < line.StartPoint.X)
                line.Invert();
        }
        protected virtual Point LineIntersectionPoint(ILine line1, ILine line2)
        {
            //For these calculations the longer line is always the static line, and the short one the collission line.
            GetStaticLine(out ILine collissionLine, out ILine staticLine, line1, line2);
            ReArrangeLine(collissionLine);
            ReArrangeLine(staticLine);

            var staticLineSine = Utility2DMath.CalculateSine(staticLine);
            var lineSine = Utility2DMath.CalculateSine(collissionLine);

            //When the lines have the same xCord we can start comparing how fast the gap between them is closing and where they will collide.
            var distance = staticLine.StartPoint.X - collissionLine.StartPoint.X;
            var heightDifference = (collissionLine.StartPoint.Y + (distance * lineSine)) - staticLine.StartPoint.Y;
            var heightLossRatio = staticLineSine - lineSine;

            var xIntersectionPoint = staticLine.StartPoint.X + (heightDifference / heightLossRatio);
            var yIntersectionPoint = collissionLine.StartPoint.Y + (((heightDifference / heightLossRatio) + distance) * lineSine);

            if (!xIntersectionPoint.ValueInRange(staticLine.StartPoint.X, staticLine.EndPoint.X, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!yIntersectionPoint.ValueInRange(staticLine.StartPoint.Y, staticLine.EndPoint.Y, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!xIntersectionPoint.ValueInRange(collissionLine.StartPoint.X, collissionLine.EndPoint.X, 0.2f))
                return new Point(double.NaN, double.NaN);
            else if (!yIntersectionPoint.ValueInRange(collissionLine.StartPoint.Y, collissionLine.EndPoint.Y, 0.2f))
                return new Point(double.NaN, double.NaN);
            else
                return new Point(xIntersectionPoint, yIntersectionPoint);
        }
    }
}
