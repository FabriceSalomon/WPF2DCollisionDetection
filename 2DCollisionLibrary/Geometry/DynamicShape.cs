using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Geometry
{
    public class DynamicShape : BaseGeometry
    {
        private Vertex[] vertices;
        private IGeometryAdapter geometryAdapter;

        public DynamicShape(IGeometryAdapter geometryAdapter)
        {
            UpdateShape(geometryAdapter);
        }

        private void CreateShape(Vertex[] vertices)
        {
            this.vertices = vertices;
            for (int i = 0; i < this.vertices.Length; i++)
            {
                vertices[i].Name = Name + "p" + i;
            }
        }

        public void UpdateShape(IGeometryAdapter geometryAdpt)
        {
            geometryAdapter = geometryAdpt;
            Name = geometryAdapter.Name;
            CreateShape(geometryAdapter.Vertices);
            geometryAdapter.ConnectVertices(vertices);
            Build();
        }

        public override void Refresh()
        {
            geometryAdapter.UpdateVertices();
        }

        public override Vertex[] GetVertices()
        {
            return vertices;
        }
    }
}
