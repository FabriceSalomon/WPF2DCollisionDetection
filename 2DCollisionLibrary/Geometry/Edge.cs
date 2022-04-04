using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DCollisionLibrary.Geometry
{
    public class Edge : BaseGeometry
    {
        private static int _count;
        private Vertex[] _vertices;
        public Vertex StartPoint { get; set; }
        public Vertex EndPoint { get; set; }

        public Edge()
            : this(new Point(), new Point())
        {
        }

        public Edge(Point startPoint, Point endPoint)
            : this(new Position(startPoint), new Position(endPoint))
        {
        }

        public Edge(Position startPoint, Position endPoint)
        {
            Name = "Edge" + _count;
            _count++;

            CreateShape();
            UpdateShape(startPoint, endPoint);
        }

        private void CreateShape()
        {
            StartPoint = new Vertex(new Point());
            StartPoint.Name = Name + "p1";
            EndPoint = new Vertex(new Point());
            EndPoint.Name = Name + "p2";

            StartPoint.AddConnection(EndPoint);
        }

        public void UpdateShape(Position startPoint, Position endPoint)
        {
            StartPoint.Position = startPoint;
            EndPoint.Position = endPoint;

            _vertices = new Vertex[] { StartPoint, EndPoint };
            Build();
        }

        public override void Refresh()
        {
        }

        public override IVertex[] GetVertices()
        {
            return _vertices;
        }
    }
}
