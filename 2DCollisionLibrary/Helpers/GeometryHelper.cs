using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Helpers
{
    public static class GeometryHelper
    {
        public static void MoveTo(this IEnumerable<IVertex> vertices, double xPos, double yPos)
        {
            var boundingBox = new BoundingBox();
            boundingBox.UpdateBoundingBox(vertices.ToArray());
            var xOffset = xPos - boundingBox.Rect.TopLeft.X;
            var yOffset = yPos - boundingBox.Rect.TopLeft.Y;
            foreach (var vertex in vertices)
                vertex.Move(xOffset, yOffset);
        }

        public static void Rotate(this IEnumerable<IVertex> vertices, Point transformOrigin, double fromAngle, double toAngle)
        {
            var rotateTo = toAngle - fromAngle;
            foreach (var vertex in vertices)
            {
                vertex.Rotate(transformOrigin, rotateTo);
            }
        }

        public static void Scale(this IEnumerable<IVertex> vertices, double xScale, double yScale, double centerX, double centerY)
        {
            var boundingBox = new BoundingBox();
            boundingBox.UpdateBoundingBox(vertices.ToArray());
            var origin = new Point(boundingBox.Rect.Left + (boundingBox.Rect.Width * centerX), boundingBox.Rect.Top + (boundingBox.Rect.Height * centerY));
            foreach (var vertex in vertices)
            {
                var direction = Point.Subtract(origin, vertex.Position.Point);
                vertex.Move(direction.X * (1 - xScale), direction.Y * (1 - yScale));
            }
        }
    }
}
