using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Models;
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
        public static Point GetCordsFromYCord(double yTargetCord, Point startPoint, Point endPoint)
        {
            double minNumber = Math.Min(startPoint.Y, endPoint.Y);
            double maxNumber = Math.Max(startPoint.Y, endPoint.Y);

            double percent = (yTargetCord - minNumber) / (maxNumber - minNumber);
            double yCord = BlendValues(minNumber, maxNumber, percent);
            double xCord = 0;
            if (startPoint.Y < endPoint.Y)
                xCord = BlendValues(startPoint.X, endPoint.X, percent);
            else
                xCord = BlendValues(endPoint.X, startPoint.X, percent);
            return new Point(xCord, yCord);
        }

        public static Point GetCordsFromXCord(double xTargetCord, Point startPoint, Point endPoint)
        {
            double minNumber = Math.Min(startPoint.X, endPoint.X);
            double maxNumber = Math.Max(startPoint.X, endPoint.X);

            double percent = (xTargetCord - minNumber) / (maxNumber - minNumber);
            double xCord = BlendValues(minNumber, maxNumber, percent);
            double yCord = 0;
            if (startPoint.X < endPoint.X)
                yCord = BlendValues(startPoint.Y, endPoint.Y, percent);
            else
                yCord = BlendValues(endPoint.Y, startPoint.Y, percent);
            return new Point(xCord, yCord);
        }

        public static Point Center(this Rect rect)
        {
            Point point = new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
            return point;
        }

        public static bool ValueInRange(this double value, double val1, double val2)
        {
            return ValueInRange(value, val1, val2, 0);
        }

        public static bool ValueInRange(this double value, double val1, double val2, double size)
        {
            if (value >= Math.Min(val1, val2) - size && value <= Math.Max(val1, val2) + size)
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
            double min = Math.Min(val1, val2);
            double max = Math.Max(val1, val2);
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        public static double BlendValues(double val1, double val2, double percent)
        {
            double newvalue = val1 + percent * (val2 - val1);
            return newvalue;
        }

        public static Point BlendPoints(Point point1, Point point2, double percent)
        {
            double xValue = BlendValues(point1.X, point2.X, percent);
            double yValue = BlendValues(point1.Y, point2.Y, percent);
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
            double distance = line.StartPoint.Distance(line.EndPoint);
            return distance;
        }

        public static double Distance(this Point point1, Point point2)
        {
            double distance = Point.Subtract(point1,point2).Length;
            return distance;
        }

        public static Point ScalePointPercent(this Point point, Point center, double xPercent, double yPercent)
        {
            Point direction = (Point)Point.Subtract(center, point);
            direction.X = direction.X * (1 - xPercent);
            direction.Y = direction.Y * (1 - yPercent);
            TranslateTransform transform = new TranslateTransform(direction.X, direction.Y);
            point = transform.Transform(point);

            return point;
        }

        public static double CalculateAngleDifference(this Point point1, Point point2)
        {
            double xDiff = point1.X - point2.X;
            double yDiff = point1.Y - point2.Y;
            double angle = (((Math.Atan2(yDiff, xDiff) * 180) / Math.PI)) % 180;
            return angle;
        }

        public static double CalculateAngleDifference(this Line line1, Line line2)
        {
            double angle1 = line1.StartPoint.CalculateAngleDifference(line1.EndPoint);
            double angle2 = line2.StartPoint.CalculateAngleDifference(line2.EndPoint);

            return CalculateAngleDifference(angle1, angle2);
        }

        public static double CalculateAngleDifference(double angle1, double angle2)
        {
            double angle = Math.Abs(angle1 - angle2) % 360;
            if (angle > 180)
                angle = 360 - angle;

            return angle;
        }
    }
}
