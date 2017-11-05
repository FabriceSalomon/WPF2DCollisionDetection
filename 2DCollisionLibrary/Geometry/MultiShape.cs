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
        private static int count;
        private Vertex[] vertices;

        public MultiShape(Vertex[] vertices)
        {
            Name = "Multishape" + count;
            count++;
            CreateShape(vertices);
            UpdateShape(vertices);
        }

        private void CreateShape(Vertex[] vertices)
        {
            this.vertices = vertices;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                vertices[i].Name = Name + "p" + i;
            }
        }

        public void UpdateShape(Vertex[] vertices)
        {
            this.vertices = vertices;
            Build();
        }

        public override void Refresh()
        {
        }

        public override Vertex[] GetVertices()
        {
            return vertices;
        }
    }
}
