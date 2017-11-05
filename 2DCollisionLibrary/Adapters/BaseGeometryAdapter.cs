using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Adapters
{
    public class BaseGeometryAdapter : IGeometryAdapter
    {
        public string Name { get; set; }
        public Vertex[] Vertices { get; set; }

        public BaseGeometryAdapter()
        {
            Vertices = new Vertex[0];
        }

        public BaseGeometryAdapter(string name, params Vertex[] vertices)
        {
            Name = name;
            Vertices = vertices;
        }

        public virtual void UpdateVertices()
        {
        }

        public virtual Vertex[] ConnectVertices(params Vertex[] vertices)
        {
            for (int i = 0; i < vertices.Count(); i++)
            {
                Vertex currentVertex = vertices[i];
                if (i + 1 < vertices.Length)
                {
                    Vertex nextVertex = vertices[i + 1];
                    currentVertex.AddConnection(nextVertex);
                }
                else
                {
                    currentVertex.AddConnection(vertices[0]);
                }
            }

            return vertices;
        }

        protected Vertex[] ConvertToVertices(params Point[] points)
        {
            Vertex[] vertices = points.ToList().ConvertAll(p => new Vertex(p)).ToArray();
            return vertices;
        }
    }
}
