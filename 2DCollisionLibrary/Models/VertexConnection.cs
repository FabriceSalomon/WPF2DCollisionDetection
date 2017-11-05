using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Models
{
    public class VertexConnection
    {
        public string Name
        {
            get { return Vertex1.Name + "_" + Vertex2.Name; }
        }
        public Vertex Vertex1 { get; set; }
        public Vertex Vertex2 { get; set; }

        public VertexConnection(Vertex vertex1, Vertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }
    }
}
