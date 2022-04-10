using _2DCollisionLibrary.Objects;
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
        IVertex[] Vertices { get; set; }
        IVertex[] ConnectVertices(params IVertex[] vertices);
        void UpdateVertices();
    }
}
