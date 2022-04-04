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
    public enum CollissionType
    {
        BoundingBox,
        TraceLine,
        XYLine,
        CollisionLine
    }
    public class CollisionManager : ICollisionManager
    {
        public Action<Point, Point, CollissionType, string> OnRayLineCreated { get; set; }
        public int EdgeLimit { get; set; }

        public CollisionManager(int edgeLimit)
        {
            EdgeLimit = edgeLimit;
        }

        public virtual bool IsCollision(IGeometry collisionElement, IGeometry collidable)
        {
            if (collidable.Rect.IsOverlapping(collisionElement.Rect))
            {
                var collidingSides = CalculateCollidingSides(collisionElement, collidable, EdgeLimit);
                return collidingSides.Count > 0;
            }
            return false;
        }

        private List<IVertexConnection> CalculateCollidingSides(IGeometry collisionElement, IGeometry collidable, int edgeLimit)
        {
            var collidingSides = new List<IVertexConnection>();

            foreach (var vertex in collisionElement.Vertices)
            {
                if (collidingSides.Count >= edgeLimit)
                    break;//Saves performance by not calculating all sides.

                foreach (var connection in vertex.VertexConnections)
                {
                    if (IsCollision(collidable, connection.Vertex1.Position.Point, connection.Vertex2.Position.Point, collisionElement.Name))
                        collidingSides.Add(connection);
                }
            }
            return collidingSides;
        }

        private bool IsCollision(IGeometry collidableElement, Point startPoint, Point endPoint, string collisionObjectName)
        {
            var rayTrace = new RayTracer(startPoint, endPoint);
            rayTrace.RayLineCreated = OnRayLineCreated;
            foreach (var vertex in collidableElement.Vertices)
            {
                foreach (var connection in vertex.VertexConnections)
                {
                    var line = new Line(connection.Vertex1.Position.Point, connection.Vertex2.Position.Point);
                    rayTrace.Name = collisionObjectName;
                    if (rayTrace.IsCollision(line))
                        return true;
                }
            }
            return false;
        }
    }
}
