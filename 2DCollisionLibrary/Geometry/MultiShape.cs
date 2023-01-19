using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Objects;
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

        public MultiShape(IVertex[] vertices)
        {
            Name = "Multishape" + _count;
            _count++;
            CreateShape(vertices);
            UpdateShape(vertices);
        }

        private void CreateShape(IVertex[] vertices)
        {
            AttatchVerticesToGeometry(vertices);
            for (int i = 0; i < GetVertices().Length; i++)
            {
                vertices[i].Name = Name + "p" + i;
            }
        }

        public void UpdateShape(IVertex[] vertices)
        {
            AttatchVerticesToGeometry(vertices);
            Build();
        }

        public override void Refresh()
        {
        }
    }
}
