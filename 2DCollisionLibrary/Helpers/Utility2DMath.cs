using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Objects;
using _2DCollisionLibrary.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _2DCollisionLibrary.Helpers
{
    public static class Utility2DMath
    {
        public static Point Center(this Rect rect)
        {
            var point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            return point;
        }

        public static bool ValueInRange(this double value, double val1, double val2, double hitboxSize = 0)
        {
            if (value >= Math.Min(val1, val2) - hitboxSize && value <= Math.Max(val1, val2) + hitboxSize)
                return true;

            return false;
        }

        public static Point ClampPoint(this Point targetPoint, Point point1, Point point2)
        {
            if (targetPoint.X < Math.Min(point1.X, point2.X) || targetPoint.X > Math.Max(point1.X, point2.X))
                targetPoint.X = ClampValue(targetPoint.X, point1.X, point2.X);
            if (targetPoint.Y < Math.Min(point1.Y, point2.Y) || targetPoint.Y > Math.Max(point1.Y, point2.Y))
                targetPoint.Y = ClampValue(targetPoint.Y, point1.Y, point2.Y);

            return targetPoint;
        }

        public static double ClampValue(double value, double val1, double val2)
        {
            var min = Math.Min(val1, val2);
            var max = Math.Max(val1, val2);
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        public static double BlendValues(double val1, double val2, double percent)
        {
            var newvalue = val1 + percent * (val2 - val1);
            return newvalue;
        }

        public static Point BlendPoints(Point point1, Point point2, double percent)
        {
            var xValue = BlendValues(point1.X, point2.X, percent);
            var yValue = BlendValues(point1.Y, point2.Y, percent);
            return new Point(xValue, yValue);
        }

        public static bool IsOverlapping(this Rect rect1, Rect rect2)
        {
            if (rect1.Left < rect2.Right && rect1.Right > rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top)
                return true;

            return false;
        }

        public static double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }

        public static double ToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static double CalculateLenght(this Line line)
        {
            var distance = line.StartPoint.Distance(line.EndPoint);
            return distance;
        }

        public static double Distance(this Point point1, Point point2)
        {
            var distance = Point.Subtract(point1, point2).Length;
            return distance;
        }

        public static Point ScalePointPercent(this Point point, Point center, double xPercent, double yPercent)
        {
            var direction = (Point)Point.Subtract(center, point);
            direction.X = direction.X * (1 - xPercent);
            direction.Y = direction.Y * (1 - yPercent);
            var transform = new TranslateTransform(direction.X, direction.Y);
            point = transform.Transform(point);

            return point;
        }

        public static double CalculateAngleDifference(this Point point1, Point point2)
        {
            var xDiff = point1.X - point2.X;
            var yDiff = point1.Y - point2.Y;
            var angle = (((Math.Atan2(yDiff, xDiff) * 180) / Math.PI)) % 180;
            return angle;
        }

        public static double CalculateAngleDifference(this Line line1, Line line2)
        {
            var angle1 = line1.StartPoint.CalculateAngleDifference(line1.EndPoint);
            var angle2 = line2.StartPoint.CalculateAngleDifference(line2.EndPoint);

            return CalculateAngleDifference(angle1, angle2);
        }

        public static double CalculateAngleDifference(double angle1, double angle2)
        {
            var angle = Math.Abs(angle1 - angle2) % 360;
            if (angle > 180)
                return 360 - angle;

            return angle;
        }

        public static double CalculateSine(ILine line)
        {
            var lineOpp = line.EndPoint.Y - line.StartPoint.Y;
            var lineAdj = line.EndPoint.X - line.StartPoint.X;
            return CalculateSine(lineOpp, lineAdj);
        }

        public static double CalculateSine(double opposite, double adjacent)
        {
            var sine = opposite / Math.Abs(adjacent);
            return sine;
        }
    }
}
