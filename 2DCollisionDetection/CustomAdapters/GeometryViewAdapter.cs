using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Models;
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
    public class GeometryViewAdapter : BaseGeometryAdapter
    {
        public FrameworkElement View { get; private set; }

        public GeometryViewAdapter(FrameworkElement view)
        {
            Name = view.Name;
            View = view;

            Vertices = CreateVertices();
        }

        public IVertex[] CreateVertices()
        {
            return ConvertToVertices(GetPoints());
        }

        public override void UpdateVertices()
        {
            var points = GetPoints();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Position.Point = points[i];
            }
        }

        public Point[] GetPoints()
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
