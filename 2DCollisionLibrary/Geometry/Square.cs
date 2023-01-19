using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Objects;
using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using _2DCollisionLibrary.Adapters;

namespace _2DCollisionLibrary.Geometry
{
    public class Square : BaseGeometry
    {
        private static int _count;

        public Vertex TopLeft { get; set; }
        public Vertex TopRight { get; set; }
        public Vertex BottomLeft { get; set; }
        public Vertex BottomRight { get; set; }

        public Square(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
            : this(new Position(topLeft), new Position(topRight), new Position(bottomLeft), new Position(bottomRight))
        {
        }

        public Square(Position topLeft, Position topRight, Position bottomLeft, Position bottomRight)
        {
            Name = "Square" + _count;
            _count++;

            CreateShape();
            UpdateShape(topLeft, topRight, bottomLeft, bottomRight);
        }

        private void CreateShape()
        {
            TopLeft = new Vertex(new Point());
            TopRight = new Vertex(new Point());
            BottomLeft = new Vertex(new Point());
            BottomRight = new Vertex(new Point());

            TopLeft.Name = Name + "p1";
            TopRight.Name = Name + "p2";
            BottomLeft.Name = Name + "p3";
            BottomRight.Name = Name + "p4";

            TopLeft.AddConnection(TopRight);
            TopRight.AddConnection(BottomRight);
            BottomRight.AddConnection(BottomLeft);
            BottomLeft.AddConnection(TopLeft);
        }

        public void UpdateShape(Position topLeft, Position topRight, Position bottomLeft, Position bottomRight)
        {
            TopLeft.Position = topLeft;
            TopRight.Position = topRight;
            BottomLeft.Position = bottomLeft;
            BottomRight.Position = bottomRight;

            AttatchVerticesToGeometry(new Vertex[] { TopLeft, TopRight, BottomLeft, BottomRight });
            Build();
        }

        public override void Refresh()
        {
        }
    }
}
