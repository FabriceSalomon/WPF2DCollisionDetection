using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IGeometryViewAdapter
    {
        FrameworkElement View { get; }
        IVertex[] CreateVertices();

        void UpdateVertices();

        Point[] GetPoints();
    }
}
