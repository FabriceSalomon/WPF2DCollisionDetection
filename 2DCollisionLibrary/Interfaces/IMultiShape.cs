using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IMultiShape : IGeometry, IBoundingBox
    {
        Rect Rect { get; set; }
        void UpdateShape(IVertex[] vertices);

        IVertex[] GetVertices();
    }
}
