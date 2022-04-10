using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Objects;
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
        Point TransformOrigin { get; }
        IPosition Position { get; }
        ITransformation Transformation { get; }
        void MoveTo(double xPos, double yPos);
        void MoveOffset(double xOffset, double yOffset);
        void Scale(double xScale, double yScale, double centerX, double centerY);
        void Rotate(double angle);
        Rect Rect { get; set; }
        IVertex[] Vertices { get; }
        void Build();
        void Refresh();
        Action GeometryChanged { get; set; }
    }
}
