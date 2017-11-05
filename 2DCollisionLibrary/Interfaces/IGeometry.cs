using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Models;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IGeometry
    {
        string Name { get; set; }
        Point TransformOrigin { get; set; }
        Position Position { get; set; }
        Transformation Transformation { get; set; }
        void MoveTo(double xPos, double yPos);
        void MoveOffset(double xOffset, double yOffset);
        void Scale(double xScale, double yScale, double centerX, double centerY);
        void Rotate(double angle);
        Rect Rect { get; set; }
        Vertex[] Vertices { get; }
        void Build();
        void Refresh();
        Action GeometryChanged { get; set; }
    }
}
