﻿using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Adapters
{
    public abstract class BaseGeometryAdapter : IGeometryAdapter
    {
        public string Name { get; set; }
        public IVertex[] Vertices { get; set; }

        public BaseGeometryAdapter()
        {
            Vertices = new Vertex[0];
        }

        public BaseGeometryAdapter(string name, params IVertex[] vertices)
        {
            Name = name;
            Vertices = vertices;
        }

        public virtual void UpdateVertices()
        {
            var points = GetPoints();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Position.Point = points[i];
            }
        }

        public abstract Point[] GetPoints();
        
        public virtual IVertex[] ConnectVertices(params IVertex[] vertices)
        {
            for (int i = 0; i < vertices.Count(); i++)
            {
                if (i + 1 < vertices.Length)
                    vertices[i].AddConnection(vertices[i + 1]);
                else
                    vertices[i].AddConnection(vertices[0]);
            }

            return vertices;
        }

        protected IVertex[] ConvertToVertices(params Point[] points)
        {
            var vertices = points.ToList().ConvertAll(p => new Vertex(p)).ToArray();
            return vertices;
        }
    }
}
