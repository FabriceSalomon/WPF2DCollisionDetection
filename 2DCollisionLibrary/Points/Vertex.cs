using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;

namespace _2DCollisionLibrary.Objects
{
    public class Vertex : IVertex
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public List<IVertexConnection> VertexConnections { get; private set; }

        public Vertex(Point point)
        {
            VertexConnections = new List<IVertexConnection>();
            Position = new Position(point);
        }

        public IVertex AddConnection(IVertex vertex)
        {
            VertexConnections.Add(new VertexConnection(this, vertex));
            return this;
        }

        public IVertex RemoveConnection(IVertex vertex)
        {
            VertexConnections.Remove(VertexConnections.FirstOrDefault(p => p.Vertex1 == vertex || p.Vertex2 == vertex));
            return this;
        }

        public IVertex ClearConnections()
        {
            VertexConnections.Clear();
            return this;
        }

        public void Move(double xOffset, double yOffset)
        {
            Position.Point = new Point(Position.X + xOffset, Position.Y + yOffset);
        }

        public void Rotate(Point origin, double angle)
        {
            var invert = Position.X < 0 ? -1 : 1;
            var targetHyppAngle = angle;
            var targetHyppLenght = origin.Distance(Position.Point) * invert;
            var triangulate = new TriangleHelper(null, targetHyppLenght, null, targetHyppAngle, origin);

            Position.Point = new Position(triangulate.AdjacentLenght, triangulate.OppositeLenght).Point;
        }
    }
}
