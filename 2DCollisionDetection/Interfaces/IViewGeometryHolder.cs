using _2DCollisionDetection.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IViewGeometryHolder
    {
        ViewGeometry ViewGeometry { get; }
        object Element { get; }
        object MouseEventArgs { get; }
    }
}
