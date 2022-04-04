using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IViewGeometry
    {
        Action<IViewGeometryHolder> MouseUp { get; set; }
        Action<IViewGeometryHolder> MouseDown { get; set; }
        Action<IViewGeometryHolder> MouseMove { get; set; }
        Action<List<IViewGeometry>> CheckCollision { get; set; }
        bool ShowBoundingBox { get; set; }
        List<IViewGeometry> CollisionMap { get; set; }
        ICollisionManager CollisionManager { get; set; }
        string Name { get; }
        IMultiShape Geometry { get; }
        List<IViewGeometryShape> Shapes { get; set; }
        List<IViewGeometry> CurrentCollisions { get; set; }

        List<IViewGeometry> OnCheckCollision();
        void Move(double xPos, double yPos);

        void Rotate(double angle);
        IViewGeometry AddShape(FrameworkElement element, IGeometry geometry);
    }
}
