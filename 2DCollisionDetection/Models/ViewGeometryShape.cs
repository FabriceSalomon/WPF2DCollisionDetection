using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Models
{
    public class ViewGeometryShape
    {
        public string Name { get; set; }

        public FrameworkElement Element { get; private set; }
        public IGeometry Geometry { get; private set; }

        public ViewGeometryShape(FrameworkElement element, IGeometry geometry)
        {
            Name = element.Name;
            Element = element;
            Geometry = geometry;
            Geometry.Name = element.Name;
        }
    }
}
