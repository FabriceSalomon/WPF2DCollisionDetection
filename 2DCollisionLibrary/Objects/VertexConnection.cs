using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Objects
{
    public class VertexConnection : IVertexConnection
    {
        public string Name
        {
            get { return Vertex1.Name + "_" + Vertex2.Name; }
        }
        public IVertex Vertex1 { get; set; }
        public IVertex Vertex2 { get; set; }

        public VertexConnection(IVertex vertex1, IVertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }
    }
}
