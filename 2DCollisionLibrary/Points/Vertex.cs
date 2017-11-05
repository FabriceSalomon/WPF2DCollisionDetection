using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using _2DCollisionLibrary.Helpers;

namespace _2DCollisionLibrary.Models
{
    public class Vertex
    {
        public string Name { get; set; }
        public Position Position { get; set; }
        public List<VertexConnection> VertexConnections { get; private set; }

        public Vertex(Point point)
        {
            VertexConnections = new List<VertexConnection>();
            Position = new Position(point);
        }

        public Vertex AddConnection(Vertex vertex)
        {
            VertexConnections.Add(new VertexConnection(this, vertex));
            return this;
        }

        public Vertex RemoveConnection(Vertex vertex)
        {
            VertexConnections.Remove(VertexConnections.FirstOrDefault(p => p.Vertex1 == vertex || p.Vertex2 == vertex));
            return this;
        }

        public Vertex ClearConnections()
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
            double hyppLenght = origin.Distance(Position.Point);
            double hyppAngle = origin.CalculateAngleDifference(Position.Point);

            double targetHyppAngle = hyppAngle + angle;
            double targetOppLenght = Math.Cos(Utility2DMath.ToRadians(90 - targetHyppAngle)) * hyppLenght;
            double targetAdjLenght = Math.Tan(Utility2DMath.ToRadians(90 - targetHyppAngle)) * targetOppLenght;

            Position.Point = new Point(origin.X - targetAdjLenght, origin.Y - targetOppLenght);
        }
    }
}
