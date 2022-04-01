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
    public class MultiShape : BaseGeometry
    {
        private static int _count;
        private Vertex[] _vertices;

        public MultiShape(Vertex[] vertices)
        {
            Name = "Multishape" + _count;
            _count++;
            CreateShape(vertices);
            UpdateShape(vertices);
        }

        private void CreateShape(Vertex[] vertices)
        {
            _vertices = vertices;
            for (int i = 0; i < _vertices.Length; i++)
            {
                vertices[i].Name = Name + "p" + i;
            }
        }

        public void UpdateShape(Vertex[] vertices)
        {
            _vertices = vertices;
            Build();
        }

        public override void Refresh()
        {
        }

        public override Vertex[] GetVertices()
        {
            return _vertices;
        }
    }
}
