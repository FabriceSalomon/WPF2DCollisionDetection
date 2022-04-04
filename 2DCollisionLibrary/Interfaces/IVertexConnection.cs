using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IVertexConnection
    {
        string Name { get; }
        IVertex Vertex1 { get; set; }
        IVertex Vertex2 { get; set; }
    }
}
