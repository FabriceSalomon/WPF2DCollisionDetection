using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using _2DCollisionLibrary.Objects;
using _2DCollisionLibrary.Adapters;

namespace _2DCollisionLibrary.Geometry
{
    public abstract class BaseGeometry : BoundingBox, IGeometry
    {
        public string Name { get; set; }
        public Point TransformOrigin { get; private set; }
        public IPosition Position { get; private set; }
        public ITransformation Transformation { get; private set; }
        public IVertex[] Vertices { get; private set; }
        public Action GeometryChanged { get; set; }

        public BaseGeometry()
        {
            Name = "";
            Position = new Position();
            Vertices = new Vertex[0];
            TransformOrigin = new Point();
            Transformation = new Transformation();
        }

        public abstract IVertex[] GetVertices();
        public abstract void Refresh();

        public void Build()
        {
            Vertices = GetVertices();
            UpdateBoundingBox(Vertices);
            Position.Point = new Point(Rect.Center().X, Rect.Center().Y);

            if (GeometryChanged != null)
                GeometryChanged();
        }

        public virtual void MoveOffset(double xOffset, double yOffset)
        {
            MoveTo(Position.X + xOffset, Position.Y + yOffset);
        }

        public virtual void MoveTo(double xPos, double yPos)
        {
            Vertices.MoveTo(xPos, yPos);
            Position.Point = new Point(xPos, yPos);
            UpdateBoundingBox(Vertices);
        }

        public virtual void Rotate(double angle)
        {
            Vertices.Rotate(TransformOrigin, Transformation.Rotation, angle);
            Transformation.Rotation = angle;
            UpdateBoundingBox(Vertices);
        }

        public virtual void Scale(double xScale, double yScale, double centerX, double centerY)
        {
            Vertices.Scale(xScale / Transformation.ScaleX, yScale / Transformation.ScaleY, centerX, centerY);
            Transformation.ScaleX = xScale;
            Transformation.ScaleY = yScale;
            UpdateBoundingBox(Vertices);
        }
    }
}
