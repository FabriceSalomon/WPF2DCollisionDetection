using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IVertex
    {
        string Name { get; set; }
        Position Position { get; set; }
        List<IVertexConnection> VertexConnections { get; }

        Vertex AddConnection(Vertex vertex);
        Vertex RemoveConnection(Vertex vertex);

        Vertex ClearConnections();

        void Move(double xOffset, double yOffset);

        void Rotate(Point origin, double angle);
    }
}
