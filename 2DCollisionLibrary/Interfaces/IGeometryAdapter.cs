using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IGeometryAdapter
    {
        string Name { get; set; }
        Vertex[] Vertices { get; set; }
        Vertex[] ConnectVertices(params Vertex[] vertices);
        void UpdateVertices();
    }
}
