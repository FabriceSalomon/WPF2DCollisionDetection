using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IViewGeometryShape
    {
        string Name { get; set; }
        FrameworkElement Element { get; }
        IGeometry Geometry { get; }
    }
}
