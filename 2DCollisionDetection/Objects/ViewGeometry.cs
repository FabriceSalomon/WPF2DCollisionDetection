using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Objects;
using System.Windows.Media;

namespace _2DCollisionDetection.Objects
{
    public class ViewGeometry : IViewGeometry
    {
        public Action<IViewGeometryHolder> MouseUp { get; set; }
        public Action<IViewGeometryHolder> MouseDown { get; set; }
        public Action<IViewGeometryHolder> MouseMove { get; set; }
        public Action<List<IViewGeometry>> CheckCollision { get; set; }
        public bool ShowBoundingBox { get; set; }

        public List<IViewGeometry> CurrentCollisions { get; set; }
        private List<IViewGeometry> PreviousCollisions { get; set; }
        public List<IViewGeometry> CollisionMap { get; set; }
        public ICollisionManager CollisionManager { get; set; }
        public string Name
        {
            get
            {
                return string.Join("", Shapes.Select(p => p.Name));
            }
        }
        public MultiShape Geometry { get; private set; }
        public List<IViewGeometryShape> Shapes { get; set; }
        private Dictionary<string, Point> ElementRelativePositioning { get; set; }

        public ViewGeometry(List<IViewGeometry> collisionMap, ICollisionManager collisionManager, bool showBoundingBox = false)
        {
            Shapes = new List<IViewGeometryShape>();
            ElementRelativePositioning = new Dictionary<string, Point>();
            CollisionMap = collisionMap;
            Geometry = new MultiShape(new Vertex[0]);
            CollisionManager = collisionManager;
            PreviousCollisions = new List<IViewGeometry>();
            CurrentCollisions = new List<IViewGeometry>();
            ShowBoundingBox = showBoundingBox;
        }

        private void RebuildGeometry()
        {
            var vertexGroups = new List<IVertex>();
            foreach (var shape in Shapes)
            {
                vertexGroups.AddRange(shape.Geometry.Vertices);
            }
            Geometry.UpdateShape(vertexGroups.ToArray());
            Geometry.Name = Name;
        }

        public IViewGeometry AddShape(FrameworkElement element, IGeometry geometry)
        {
            var elementShape = new ViewGeometryShape(element, geometry);
            elementShape.Element.PreviewMouseDown += Element_PreviewMouseDown;
            elementShape.Element.PreviewMouseUp += Element_PreviewMouseUp;
            elementShape.Element.PreviewMouseMove += Element_PreviewMouseMove;
            elementShape.Geometry.GeometryChanged += RebuildGeometry;
            Shapes.Add(elementShape);

            ElementRelativePositioning = GetElementRelativePosititions(Shapes.Select(p => p.Element).ToArray());
            RebuildGeometry();

            return this;
        }

        public ViewGeometry RemoveShape(IViewGeometryShape shape)
        {
            Shapes.Remove(shape);
            ElementRelativePositioning = GetElementRelativePosititions(Shapes.Select(p => p.Element).ToArray());
            RebuildGeometry();

            return this;
        }

        public ViewGeometry ClearShapes()
        {
            foreach (var shape in Shapes.ToList())
            {
                Shapes.Remove(shape);
            }
            RebuildGeometry();
            return this;
        }

        public void RefreshGeometry()
        {
            foreach (var shape in Shapes)
            {
                shape.Geometry.Refresh();
            }
            Geometry.Refresh();
            Geometry.UpdateBoundingBox(Geometry.Vertices.ToArray());
        }

        private void Element_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(new ViewGeometryHolder(this, sender, e));
        }

        private void Element_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(new ViewGeometryHolder(this, sender, e));
        }

        private void Element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove != null && ((FrameworkElement)sender).IsMouseCaptured)
                MouseMove(new ViewGeometryHolder(this, sender, e));
        }

        private Dictionary<string, Point> GetElementRelativePosititions(FrameworkElement[] elements)
        {
            Dictionary<string, Point> relativePositioning = new Dictionary<string, Point>();
            for (int i = 0; i < elements.Length; i++)
            {
                for (int j = 0; j < elements.Length; j++)
                {
                    double xOffset = elements[i].Margin.Left - elements[j].Margin.Left;
                    double yOffset = elements[i].Margin.Top - elements[j].Margin.Top;
                    relativePositioning.Add(elements[j].Name + "_" + elements[i].Name, new Point(xOffset, yOffset));
                }
            }
            return relativePositioning;
        }

        public List<IViewGeometry> OnCheckCollision()
        {
            var collisions = new List<IViewGeometry>();
            foreach (var collision in CollisionMap.Where(p => Name != p.Name).ToDictionary(p => p.Geometry, o => o))
            {
                if (CollisionManager.IsCollision(Geometry, collision.Key))
                    collisions.Add(collision.Value);
            }

            foreach (var item in collisions)
            {
                if (!CurrentCollisions.Any(p => p == item))
                    CurrentCollisions.Add(item);

                if (!item.CurrentCollisions.Any(p => p == this))
                    item.CurrentCollisions.Add(this);

                if (item.CheckCollision != null)
                    item.CheckCollision(item.CurrentCollisions);
            }
            foreach (var item in PreviousCollisions)
            {
                if (!collisions.Any(p => p == item))
                {
                    CurrentCollisions.Remove(item);
                    item.CurrentCollisions.Remove(this);
                    if (item.CheckCollision != null)
                        item.CheckCollision(item.CurrentCollisions);
                }
            }
            PreviousCollisions = CurrentCollisions.ToList();

            if (CheckCollision != null)
                CheckCollision(CurrentCollisions);
            return collisions;
        }

        public void Move(double xPos, double yPos)
        {
            var movingShape = Shapes.FirstOrDefault();
            if (movingShape != null)
            {
                movingShape.Element.Margin = new Thickness(xPos, yPos, 0, 0);
                foreach (var shape in Shapes.Where(p => p != movingShape))
                {
                    var offset = ElementRelativePositioning[movingShape.Element.Name + "_" + shape.Element.Name];
                    shape.Element.Margin = new Thickness(xPos + offset.X, yPos + offset.Y, 0, 0);
                }
                RefreshGeometry();
            }
        }

        public void Rotate(double angle)
        {
            foreach (var shape in Shapes)
            {
                RotateTransform rotateTransform = null;
                if (shape.Element.RenderTransform is RotateTransform)
                    rotateTransform = (RotateTransform)shape.Element.RenderTransform;
                else if (shape.Element.RenderTransform is TransformGroup)
                    rotateTransform = (RotateTransform)((TransformGroup)shape.Element.RenderTransform).Children.FirstOrDefault(p => p is RotateTransform);
                else
                {
                    rotateTransform = new RotateTransform();
                    shape.Element.RenderTransform = rotateTransform;
                }
                rotateTransform.Angle -= angle;
            }
            RefreshGeometry();
        }
    }
}
