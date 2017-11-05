using _2DCollisionLibrary.Models;
using _2DCollisionLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace _2DCollisionLibrary.Objects
{
    public class BoundingBox
    {
        public Rect Rect { get; set; }

        public Point Center { get { return Rect.Center(); } }

        public BoundingBox()
        {

        }

        public BoundingBox(params Vertex[] verticles)
        {
            UpdateBoundingBox(verticles);
        }

        public void UpdateBoundingBox(params Vertex[] verticles)
        {
            Rect = GetRectFromVerticles(verticles);
        }

        private Rect GetRectFromVerticles(params Vertex[] verticles)
        {
            if (verticles == null || verticles.Count() == 0)
                return GetRect(new Point(), new Point(), new Point(), new Point());

            double Top = verticles.Min(p => p.Position.Y);
            double Bottom = verticles.Max(p => p.Position.Y);
            double Left = verticles.Min(p => p.Position.X);
            double Right = verticles.Max(p => p.Position.X);
            return GetRect(new Point(Left, Top), new Point(Right, Top), new Point(Left, Bottom), new Point(Right, Bottom));
        }

        private static Rect GetRect(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        {
            double minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            double minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            double maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            double maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));
            return new Rect(minX, minY, (maxX - minX), (maxY - minY));
        }
    }
}
