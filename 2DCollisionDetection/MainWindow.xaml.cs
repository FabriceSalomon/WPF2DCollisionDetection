using _2DCollisionDetection.CustomAdapters;
using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using _2DCollisionLibrary.Collision;
using _2DCollisionLibrary.Interfaces;
using _2DCollisionLibrary.Factories;

namespace _2DCollisionDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _rayCount;
        //Configure how often to calculate collision maps.
        private readonly int _dragCollisionRefresh = 2;

        private IViewGeometry SelectedViewGeometry { get; set; }
        private List<IViewGeometry> CollisionMap { get; set; }

        private double dragDistance = 0;
        public bool isDragging;
        private Point selectPosition;
        private Point refreshPosition;
        private IViewGeometry CollisionViewGeometry { get; set; }
        private IViewGeometry CollisionViewGeometry2 { get; set; }
        private IViewGeometry CollisionViewGeometry3 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
            this.MouseMove += MainWindow_MouseMove;
            this.grdHolder.SizeChanged += grdHolder_SizeChanged;
        }

        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            this.lblMouseCords.Text = "MouseCords: X:" + e.GetPosition(grdHolder).X + " Y:" + e.GetPosition(grdHolder).Y;
        }

        private void grdHolder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (CollisionMap != null)
            {
                var maxWidth = grdHolder.ActualWidth - 30;
                var maxHeight = grdHolder.ActualHeight - 50;
                foreach (var viewGeometry in CollisionMap)
                {
                    var xPosition = viewGeometry.Shapes.Min(o => o.Element.Margin.Left);
                    var yPosition = viewGeometry.Shapes.Min(o => o.Element.Margin.Top);

                    viewGeometry.Move(Math.Min(maxWidth, Math.Max(0, xPosition)), Math.Min(maxHeight, Math.Max(0, yPosition)));
                    viewGeometry.OnCheckCollision();

                    foreach (var item in viewGeometry.Shapes)
                        item.Geometry.Refresh();

                    OutlineEdges(viewGeometry);
                    DrawBoundingBox("", new Rect());
                }
                dragDistance = 0;
            }
        }

        private void Shape_MouseUp(ViewGeometry viewGeometry, object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var element = (FrameworkElement)sender;

            if (element != null)
            {
                OutlineEdges(viewGeometry);
                dragDistance = 0;

                element.ReleaseMouseCapture();

                rectangleInfo.UpdateData(element.Name, viewGeometry);
            }
        }

        private void Shape_MouseMove(ViewGeometry viewGeometry, object sender, MouseEventArgs e)
        {
            if (isDragging && sender != null)
            {
                var element = (FrameworkElement)sender;
                var point = e.GetPosition(grdHolder);

                var xPosition = point.X - selectPosition.X;
                var yPosition = point.Y - selectPosition.Y;

                var maxWidth = grdHolder.ActualWidth - 30;
                var maxHeight = grdHolder.ActualHeight - 50;

                dragDistance = Math.Abs(xPosition - refreshPosition.X) + Math.Abs(yPosition - refreshPosition.Y);
                viewGeometry.Move(Math.Min(maxWidth, Math.Max(0, xPosition)), Math.Min(maxHeight, Math.Max(0, yPosition)));
                
                if (dragDistance >= _dragCollisionRefresh)
                {
                    refreshPosition = new Point(xPosition, yPosition);
                    viewGeometry.OnCheckCollision();
                    dragDistance = 0;
                }
                rectangleInfo.UpdateData(element.Name, viewGeometry);
            }
        }

        private void Shape_MouseDown(ViewGeometry viewGeometry, object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (element != null)
            {
                SelectedViewGeometry = viewGeometry;
                element.CaptureMouse();
                selectPosition = e.GetPosition(element);
                refreshPosition = selectPosition;
                isDragging = true;

                rectangleInfo.UpdateData(element.Name, viewGeometry);
                if (dragDistance > _dragCollisionRefresh)
                    dragDistance = 0;
            }
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            CollisionMap = new List<IViewGeometry>();

            var collisionManager = new CollisionManager(2);
            collisionManager.OnRayLineCreated += DrawLine;

            IShapeFactory shapeFactory = new DynamicShapeFactory(CollisionMap, collisionManager, OnMouseUp, OnMouseDown, OnMouseMove);

            var viewGeometry1 = shapeFactory.CreateShape();
            shapeFactory.AddShape(viewGeometry1, rctOne);

            var viewGeometry2 = shapeFactory.CreateShape();
            shapeFactory.AddShape(viewGeometry2, rctThree);

            var viewGeometry3 = shapeFactory.CreateShape();
            shapeFactory.AddShape(viewGeometry3, rctFour);

            var viewGeometry4 = shapeFactory.CreateShape();
            shapeFactory.AddShape(viewGeometry4, polyTriangle);

            var viewGeometry5 = shapeFactory.CreateShape();
            shapeFactory.AddShape(viewGeometry5, polyOctagon);

            CollisionViewGeometry = shapeFactory.CreateShape();
            shapeFactory.AddShape(CollisionViewGeometry, rctTwo);
            shapeFactory.AddShape(CollisionViewGeometry, txtCollision);
            CollisionViewGeometry.CheckCollision += (results) =>
                {
                    if (results.Count > 0)
                        txtCollision.Text = "Colliding with " + results.Count() + " items";
                    else
                        txtCollision.Text = "No collisions";
                };

            CollisionViewGeometry2 = shapeFactory.CreateShape();
            shapeFactory.AddShape(CollisionViewGeometry2, rctTwo2);
            shapeFactory.AddShape(CollisionViewGeometry2, txtCollision2);
            CollisionViewGeometry2.CheckCollision += (results) =>
                {
                    if (results.Count > 0)
                        txtCollision2.Text = "Colliding with " + results.Count() + " items";
                    else
                        txtCollision2.Text = "No collisions";
                };

            CollisionViewGeometry3 = shapeFactory.CreateShape();
            shapeFactory.AddShape(CollisionViewGeometry3, rctTwo3);
            shapeFactory.AddShape(CollisionViewGeometry3, txtCollision3);
            CollisionViewGeometry3.MouseDown += (item) => 
            { 
                Shape_MouseDown(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs);
            };
            CollisionViewGeometry3.MouseUp += (item) => 
            { 
                Shape_MouseUp(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs); 
            };
            CollisionViewGeometry3.MouseMove += (item) =>
            { 
                if (isDragging) 
                    Shape_MouseMove(item.ViewGeometry, item.Element, (MouseEventArgs)item.MouseEventArgs);
            };
            CollisionViewGeometry3.CheckCollision += (items) =>
                {
                    if (items.Count > 0)
                        txtCollision3.Text = "Colliding with " + items.Count() + " items";
                    else
                        txtCollision3.Text = "No collisions";
                };

            CollisionMap.Add(viewGeometry1);
            CollisionMap.Add(viewGeometry2);
            CollisionMap.Add(viewGeometry3);
            CollisionMap.Add(viewGeometry4);
            CollisionMap.Add(viewGeometry5);
            CollisionMap.Add(CollisionViewGeometry);
            CollisionMap.Add(CollisionViewGeometry2);
            CollisionMap.Add(CollisionViewGeometry3);

            foreach (var item in CollisionMap)
            {
                OutlineEdges(item);
            }

            var timer = new Timer(3);
            timer.Elapsed += (param1, param2) =>
                {
                    //This way of invoking prevents crashing during shutdown.
                    Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action
                            (
                                () =>
                                {
                                    timer.Stop();

                                    CollisionViewGeometry3.Rotate(0.4f);
                                    OutlineEdges(CollisionViewGeometry3);
                                    CollisionViewGeometry3.OnCheckCollision();
                                    if(SelectedViewGeometry == CollisionViewGeometry3)
                                        DrawBoundingBox("", CollisionViewGeometry3.Geometry.Rect);
                                    timer.Start();
                                }
                            )
                        );
                };
            timer.Enabled = true;
        }

        private void OnMouseDown(IViewGeometryHolder item)
        {
            Shape_MouseDown(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs);
            DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
        }

        private void OnMouseUp(IViewGeometryHolder item)
        {
            Shape_MouseUp(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs);
            DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
        }

        private void OnMouseMove(IViewGeometryHolder item)
        {
            Shape_MouseMove(item.ViewGeometry, item.Element, (MouseEventArgs)item.MouseEventArgs);
            if (isDragging)
            {
                OutlineEdges(item.ViewGeometry);
                DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
            }
        }

        private void OutlineEdges(IViewGeometry viewGeometry)
        {
            foreach (var vertex in viewGeometry.Geometry.Vertices)
            {
                foreach (var connection in vertex.VertexConnections)
                {
                    DrawLine(connection.Vertex1.Position.Point, connection.Vertex2.Position.Point, CollissionType.TraceLine, connection.Name);
                }
            }
        }

        private void DrawBoundingBox(string id, Rect boundingBox)
        {
            DrawLine(boundingBox.TopLeft, boundingBox.TopRight, CollissionType.BoundingBox, "top" + id);
            DrawLine(boundingBox.TopRight, boundingBox.BottomRight, CollissionType.BoundingBox, "right" + id);
            DrawLine(boundingBox.BottomLeft, boundingBox.BottomRight, CollissionType.BoundingBox, "bottom" + id);
            DrawLine(boundingBox.BottomLeft, boundingBox.TopLeft, CollissionType.BoundingBox, "left" + id);
        }

        private void DrawLine(Point point1, Point point2, CollissionType type, string name)
        {
            if (type == CollissionType.XYLine)
            {
                if (name.Equals("rctTwo3txtCollision3", StringComparison.OrdinalIgnoreCase))
                    name += _rayCount;
                else
                    name = type.ToString() + _rayCount;

                _rayCount++;
                if (_rayCount > 30)
                    _rayCount = 0;
            }

            var line1 = grdHolder.Children.OfType<Line>().FirstOrDefault(p => p.Name == name + type);
            if (line1 == null)
            {
                line1 = new Line();
                grdHolder.Children.Add(line1);
                line1.Name = name + type;
            }
            line1.X1 = point1.X;
            line1.Y1 = point1.Y;
            line1.X2 = point2.X;
            line1.Y2 = point2.Y;

            if (type == CollissionType.TraceLine)
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90551a8b"));//purple
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90551a8b"));//purple
            }
            else if (type == CollissionType.CollisionLine)
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5000d6a0"));//teal
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5000d6a0"));//teal
            }
            else if (type == CollissionType.XYLine )
            {
                line1.X2 += 1;
                line1.Y2 += 1;
                line1.StrokeThickness = 15;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#800000FF"));//blue
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#800000FF"));//blue
            }
            else if (type == CollissionType.BoundingBox)
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#70ff8d00"));//orange
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#70ff8d00"));//orange
            }
        }
    }
}
