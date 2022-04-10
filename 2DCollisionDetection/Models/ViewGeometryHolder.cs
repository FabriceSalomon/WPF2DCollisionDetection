using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Objects
{
    public class ViewGeometryHolder : IViewGeometryHolder
    {
        public ViewGeometry ViewGeometry { get; private set; }
        public object Element { get; private set; }
        public object MouseEventArgs { get; private set; }

        public ViewGeometryHolder(ViewGeometry viewGeometry, object element, object mouseEventArgs)
        {
            ViewGeometry = viewGeometry;
            Element = element;
            MouseEventArgs = mouseEventArgs;
        }
    }
}
