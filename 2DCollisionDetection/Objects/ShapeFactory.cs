using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Factories
{
    public abstract class ShapeFactory : IShapeFactory
    {
        public abstract IViewGeometry CreateShape();
        public abstract void AddShape(IViewGeometry viewGeometry, FrameworkElement element);
    }
}
