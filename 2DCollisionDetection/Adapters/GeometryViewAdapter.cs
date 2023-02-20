using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _2DCollisionDetection.CustomAdapters
{
    public class GeometryViewAdapter : BaseGeometryAdapter, IGeometryViewAdapter
    {
        public FrameworkElement View { get; private set; }

        public GeometryViewAdapter(FrameworkElement view)
        {
            Name = view.Name;
            View = view;

            Vertices = ConvertToVertices(GetPoints());
        }

        public override Point[] GetPoints()
        {
            if (View is Polygon)
                return GetPointsFromPoly((Polygon)View);

            return GetPointsFromSquare(View);
        }

        private Point[] GetPointsFromSquare(FrameworkElement view)
        {
            var parent = VisualTreeHelper.GetParent(view) as UIElement;
            var transform = view.TransformToAncestor(parent);

            var topLeft = transform.Transform(new Point(0, 0));
            var topRight = transform.Transform(new Point(view.ActualWidth, 0));
            var bottomRight = transform.Transform(new Point(view.ActualWidth, View.ActualHeight));
            var bottomLeft = transform.Transform(new Point(0, view.ActualHeight));

            return new Point[] { topLeft, topRight, bottomRight, bottomLeft };
        }

        private Point[] GetPointsFromPoly(Polygon view)
        {
            var parent = VisualTreeHelper.GetParent(view) as UIElement;
            var transform = view.TransformToAncestor(parent);
            return view.Points.Select(p => transform.Transform(p)).ToArray();
        }
    }
}
