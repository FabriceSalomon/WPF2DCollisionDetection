using _2DCollisionDetection.CustomAdapters;
using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Factories
{
    public class DynamicShapeFactory : ShapeFactory
    {
        private List<IViewGeometry> _collisionMap;
        private ICollisionManager _collisionManager;
        private Action<IViewGeometryHolder> _mouseUp;
        private Action<IViewGeometryHolder> _mouseDown;
        private Action<IViewGeometryHolder> _mouseMove;

        public DynamicShapeFactory(List<IViewGeometry> collisionMap, ICollisionManager collisionManager, Action<IViewGeometryHolder> mouseUp, Action<IViewGeometryHolder> mouseDown, Action<IViewGeometryHolder> mouseMove)
        {
            _collisionMap = collisionMap;
            _collisionManager = collisionManager;
            _mouseUp = mouseUp;
            _mouseDown = mouseDown;
            _mouseMove = mouseMove;
        }
        public override IViewGeometry CreateShape()
        {
            var viewGeometry = new ViewGeometry(_collisionMap, _collisionManager);
            viewGeometry.MouseDown += _mouseDown;
            viewGeometry.MouseUp += _mouseUp;
            viewGeometry.MouseMove += _mouseMove;
            return viewGeometry;
        }
        public override void AddShape(IViewGeometry viewGeometry, FrameworkElement element)
        {
            viewGeometry.AddShape(element, new DynamicShape(new GeometryViewAdapter(element)));
        }
    }
}
