using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Models;
using System.Windows.Media;

namespace _2DCollisionDetection.Objects
{
    public class ViewGeometry
    {
        public Action<ViewGeometryHolder> MouseUp;
        public Action<ViewGeometryHolder> MouseDown;
        public Action<ViewGeometryHolder> MouseMove;
        public Action<List<ViewGeometry>> CheckCollision;
        public bool ShowBoundingBox { get; set; }

        private List<ViewGeometry> CurrentCollisions { get; set; }
        private List<ViewGeometry> PreviousCollisions { get; set; }
        public List<ViewGeometry> CollisionMap { get; set; }
        public ICollisionManager CollisionManager { get; set; }
        public string Name
        {
            get
            {
                return string.Join("", Shapes.Select(p => p.Name));
            }
        }
        public MultiShape Geometry { get; private set; }
        public List<ViewGeometryShape> Shapes { get; set; }
        private Dictionary<string, Point> ElementRelativePositioning { get; set; }

        public ViewGeometry(List<ViewGeometry> collisionMap, ICollisionManager collisionManager, bool showBoundingBox = false)
        {
            Shapes = new List<ViewGeometryShape>();
            ElementRelativePositioning = new Dictionary<string, Point>();
            CollisionMap = collisionMap;
            Geometry = new MultiShape(new Vertex[0]);
            CollisionManager = collisionManager;
            PreviousCollisions = new List<ViewGeometry>();
            CurrentCollisions = new List<ViewGeometry>();
            ShowBoundingBox = showBoundingBox;
        }

        private void RebuildGeometry()
        {
            List<Vertex> vertexGroups = new List<Vertex>();
            foreach (var shape in Shapes)
            {
                vertexGroups.AddRange(shape.Geometry.Vertices);
            }
            Geometry.UpdateShape(vertexGroups.ToArray());
            Geometry.Name = Name;
        }

        public ViewGeometry AddShape(FrameworkElement element, IGeometry geometry)
        {
            ViewGeometryShape elementShape = new ViewGeometryShape(element, geometry);
            elementShape.Element.PreviewMouseDown += Element_PreviewMouseDown;
            elementShape.Element.PreviewMouseUp += Element_PreviewMouseUp;
            elementShape.Element.PreviewMouseMove += Element_PreviewMouseMove;
            elementShape.Geometry.GeometryChanged += RebuildGeometry;
            Shapes.Add(elementShape);

            ElementRelativePositioning = GetElementRelativePosititions(Shapes.Select(p => p.Element).ToArray());
            RebuildGeometry();

            return this;
        }

        public ViewGeometry RemoveShape(ViewGeometryShape shape)
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
            Geometry.UpdateBoundingBox(Geometry.Vertices);
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

        public List<ViewGeometry> OnCheckCollision()
        {
            List<ViewGeometry> collisions = new List<ViewGeometry>();
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
            ViewGeometryShape movingShape = Shapes.FirstOrDefault();
            if (movingShape != null)
            {
                movingShape.Element.Margin = new Thickness(xPos, yPos, 0, 0);
                foreach (var shape in Shapes.Where(p => p != movingShape))
                {
                    Point offset = ElementRelativePositioning[movingShape.Element.Name + "_" + shape.Element.Name];
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
