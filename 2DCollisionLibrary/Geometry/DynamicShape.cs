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
        private Vertex[] _vertices;
        private IGeometryAdapter _geometryAdapter;

        public DynamicShape(IGeometryAdapter geometryAdapter)
        {
            UpdateShape(geometryAdapter);
        }

        private void CreateShape(Vertex[] vertices)
        {
            _vertices = vertices;
            for (int i = 0; i < _vertices.Length; i++)
                vertices[i].Name = Name + "p" + i;
        }

        public void UpdateShape(IGeometryAdapter geometryAdapter)
        {
            _geometryAdapter = geometryAdapter;
            Name = _geometryAdapter.Name;
            CreateShape(_geometryAdapter.Vertices);
            _geometryAdapter.ConnectVertices(_vertices);
            Build();
        }

        public override void Refresh()
        {
            _geometryAdapter.UpdateVertices();
        }

        public override Vertex[] GetVertices()
        {
            return _vertices;
        }
    }
}
