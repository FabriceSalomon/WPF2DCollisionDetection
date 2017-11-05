using _2DCollisionLibrary.Adapters;
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

        public Vertex[] CreateVertices()
        {
            return ConvertToVertices(GetPoints());
        }

        public override void UpdateVertices()
        {
            Point[] points = GetPoints();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Position.Point = points[i];
            }
        }

        public Point[] GetPoints()
        {
            Point[] points;
            if (View is Polygon)
                points = GetPointsFromPoly((Polygon)View);
            else
                points = GetPointsFromSquare(View);

            return points;
        }

        private Point[] GetPointsFromSquare(FrameworkElement view)
        {
            UIElement parent = VisualTreeHelper.GetParent(view) as UIElement;
            GeneralTransform transform = view.TransformToAncestor(parent);

            Point topLeft = transform.Transform(new Point(0, 0));
            Point topRight = transform.Transform(new Point(view.ActualWidth, 0));
            Point bottomRight = transform.Transform(new Point(view.ActualWidth, View.ActualHeight));
            Point bottomLeft = transform.Transform(new Point(0, view.ActualHeight));

            return new Point[] { topLeft, topRight , bottomRight, bottomLeft };
        }

        private Point[] GetPointsFromPoly(Polygon view)
        {
            UIElement parent = VisualTreeHelper.GetParent(view) as UIElement;
            GeneralTransform transform = view.TransformToAncestor(parent);
            return view.Points.Select(p => transform.Transform(p)).ToArray();
        }
    }
}
