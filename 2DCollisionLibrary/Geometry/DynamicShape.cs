using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Geometry
{
    public class DynamicShape : BaseGeometry
    {
        private IGeometryAdapter _geometryAdapter;

        public DynamicShape(IGeometryAdapter geometryAdapter)
        {
            UpdateShape(geometryAdapter);
        }

        private void CreateShape(IVertex[] vertices)
        {
            AttatchVerticesToGeometry(vertices);
            for (int i = 0; i < GetVertices().Length; i++)
                vertices[i].Name = Name + "p" + i;
        }

        public void UpdateShape(IGeometryAdapter geometryAdapter)
        {
            _geometryAdapter = geometryAdapter;
            Name = _geometryAdapter.Name;
            CreateShape(_geometryAdapter.Vertices);
            _geometryAdapter.ConnectVertices(GetVertices());
            Build();
        }

        public override void Refresh()
        {
            _geometryAdapter.UpdateVertices();
        }
    }
}
