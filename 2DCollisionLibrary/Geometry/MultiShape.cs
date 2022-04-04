using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DCollisionLibrary.Geometry
{
    public class MultiShape : BaseGeometry, IMultiShape
    {
        private static int _count;
        private IVertex[] _vertices;

        public MultiShape(Vertex[] vertices)
        {
            Name = "Multishape" + _count;
            _count++;
            CreateShape(vertices);
            UpdateShape(vertices);
        }

        private void CreateShape(IVertex[] vertices)
        {
            _vertices = vertices;
            for (int i = 0; i < _vertices.Length; i++)
            {
                vertices[i].Name = Name + "p" + i;
            }
        }

        public void UpdateShape(IVertex[] vertices)
        {
            _vertices = vertices;
            Build();
        }

        public override void Refresh()
        {
        }

        public override IVertex[] GetVertices()
        {
            return _vertices;
        }
    }
}
