using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Tracers;
using _2DCollisionLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Models;
using _2DCollisionLibrary.Points;

namespace _2DCollisionLibrary.Collision
{
    public class CollisionManager : ICollisionManager
    {
        public Action<Point, Point, string, string> OnRayLineCreated { get; set; }
        public int EdgeLimit { get; set; }

        public CollisionManager(int edgeLimit)
        {
            EdgeLimit = edgeLimit;
        }

        public virtual bool CalculateCollision(IGeometry collisionElement, IGeometry collidable)
        {
            List<IGeometry> collidingObjects = new List<IGeometry>();

            if (collidable.Rect.IsOverlapping(collisionElement.Rect))
            {
                List<VertexConnection> collidingSides = CalculateCollidingSides(collisionElement, collidable, EdgeLimit);
                if (collidingSides.Count > 0)
                    return true;
            }
            return false;
        }

        private List<VertexConnection> CalculateCollidingSides(IGeometry collisionElement, IGeometry collidable, int edgeLimit)
        {
            List<VertexConnection> collidingSides = new List<VertexConnection>();

            foreach (var vertex in collisionElement.Vertices)
            {
                if (collidingSides.Count >= edgeLimit)
                    break;//Saves performance by not calculating all sides.

                foreach (var connection in vertex.VertexConnections)
                {
                    if (IsCollision(collidable, connection.Vertex1.Position.Point, connection.Vertex2.Position.Point, vertex.Name, collisionElement.Name))
                        collidingSides.Add(connection);   
                }
            }
            return collidingSides;
        }

        private bool IsCollision(IGeometry collidableElement, Point startPoint, Point endPoint, string connectionName, string collisionObjectName)
        {
            RayTracer rayTrace = new RayTracer(startPoint, endPoint);
            rayTrace.RayLineCreated = OnRayLineCreated;
            bool result = false;
            foreach (var vertex in collidableElement.Vertices)
            {
                foreach (var connection in vertex.VertexConnections)
                {
                    Line line = new Line(connection.Vertex1.Position.Point, connection.Vertex2.Position.Point);
                    rayTrace.Name = collisionObjectName;
                    if (rayTrace.IsCollision(line))
                        result = true;
                }
            }
            if (result)
                return true;
            else
                return false;
        }
    }
}
