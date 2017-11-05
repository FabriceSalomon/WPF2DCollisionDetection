using _2DCollisionDetection.CustomAdapters;
using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Adapters;
using _2DCollisionLibrary.Helpers;
using _2DCollisionLibrary.Geometry;
using _2DCollisionLibrary.Models;
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

namespace _2DCollisionDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int rayCount = 0;
        //Configure how often to calculate collision maps.
        private const int dragCollisionRefresh = 2;

        private List<ViewGeometry> CollisionMap { get; set; }

        private double dragDistance = 0;
        public bool isDragging;
        private Point selectPosition;
        private Point refreshPosition;
        private ViewGeometry CollisionViewGeometry { get; set; }
        private ViewGeometry CollisionViewGeometry2 { get; set; }
        private ViewGeometry CollisionViewGeometry3 { get; set; }

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
                double maxWidth = grdHolder.ActualWidth - 30;
                double maxHeight = grdHolder.ActualHeight - 50;
                foreach (var viewGeometry in CollisionMap)
                {
                    double xPosition = viewGeometry.Shapes.Min(o => o.Element.Margin.Left);
                    double yPosition = viewGeometry.Shapes.Min(o => o.Element.Margin.Top);

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
            FrameworkElement element = (FrameworkElement)sender;

            if (element != null)
            {
                OutlineEdges(viewGeometry);
                dragDistance = 0;

                element.ReleaseMouseCapture();

                this.rectangleInfo.UpdateData(element.Name, viewGeometry);
            }
        }

        private void Shape_MouseMove(ViewGeometry viewGeometry, object sender, MouseEventArgs e)
        {
            if (isDragging && sender != null)
            {
                FrameworkElement element = (FrameworkElement)sender;
                Point point = e.GetPosition(grdHolder);

                double xPosition = point.X - selectPosition.X;
                double yPosition = point.Y - selectPosition.Y;

                double maxWidth = grdHolder.ActualWidth - 30;
                double maxHeight = grdHolder.ActualHeight - 50;

                dragDistance = Math.Abs(xPosition - refreshPosition.X) + Math.Abs(yPosition - refreshPosition.Y);
                viewGeometry.Move(Math.Min(maxWidth, Math.Max(0, xPosition)), Math.Min(maxHeight, Math.Max(0, yPosition)));
                
                if (dragDistance >= dragCollisionRefresh)
                {
                    refreshPosition = new Point(xPosition, yPosition);
                    viewGeometry.OnCheckCollision();
                    dragDistance = 0;
                }
                this.rectangleInfo.UpdateData(element.Name, viewGeometry);
            }
        }

        private void Shape_MouseDown(ViewGeometry viewGeometry, object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            if (element != null)
            {
                element.CaptureMouse();
                selectPosition = e.GetPosition(element);
                refreshPosition = selectPosition;
                isDragging = true;

                this.rectangleInfo.UpdateData(element.Name, viewGeometry);
                if (dragDistance > dragCollisionRefresh)
                    dragDistance = 0;
            }
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            CollisionMap = new List<ViewGeometry>();

            Action<ViewGeometry.ViewGeometryHolder> onMouseDown = (item) =>
            {
                Shape_MouseDown(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs);
                DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
            };
            Action<ViewGeometry.ViewGeometryHolder> onMouseUp = (item) =>
            {
                Shape_MouseUp(item.ViewGeometry, item.Element, (MouseButtonEventArgs)item.MouseEventArgs);
                OutlineEdges(item.ViewGeometry);
                DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
            };
            Action<ViewGeometry.ViewGeometryHolder> onMouseMove = (item) =>
            {
                Shape_MouseMove(item.ViewGeometry, item.Element, (MouseEventArgs)item.MouseEventArgs);
                if (isDragging)
                {
                    OutlineEdges(item.ViewGeometry);
                    DrawBoundingBox("", item.ViewGeometry.Geometry.Rect);
                }
            };

            CollisionManager collisionManager = new CollisionManager(2);
            collisionManager.OnRayLineCreated += DisplayRayLine;

            ViewGeometry viewGeometry1 = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctOne, new DynamicShape(new GeometryViewAdapter(rctOne)));
            viewGeometry1.MouseDown += onMouseDown;
            viewGeometry1.MouseUp += onMouseUp;
            viewGeometry1.MouseMove += onMouseMove;

            ViewGeometry viewGeometry2 = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctThree, new DynamicShape(new GeometryViewAdapter(rctThree)));
            viewGeometry2.MouseDown += onMouseDown;
            viewGeometry2.MouseUp += onMouseUp;
            viewGeometry2.MouseMove += onMouseMove;

            ViewGeometry viewGeometry3 = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctFour, new DynamicShape(new GeometryViewAdapter(rctFour)));
            viewGeometry3.MouseDown += onMouseDown;
            viewGeometry3.MouseUp += onMouseUp;
            viewGeometry3.MouseMove += onMouseMove;

            ViewGeometry viewGeometry4 = new ViewGeometry(CollisionMap, collisionManager).AddShape(polyTriangle, new DynamicShape(new GeometryViewAdapter(polyTriangle)));
            viewGeometry4.MouseDown += onMouseDown;
            viewGeometry4.MouseUp += onMouseUp;
            viewGeometry4.MouseMove += onMouseMove;

            ViewGeometry viewGeometry5 = new ViewGeometry(CollisionMap, collisionManager).AddShape(polyOctagon, new DynamicShape(new GeometryViewAdapter(polyOctagon)));
            viewGeometry5.MouseDown += onMouseDown;
            viewGeometry5.MouseUp += onMouseUp;
            viewGeometry5.MouseMove += onMouseMove;

            CollisionViewGeometry = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctTwo, new DynamicShape(new GeometryViewAdapter(rctTwo))).AddShape(txtCollision, new DynamicShape(new GeometryViewAdapter(txtCollision)));
            CollisionViewGeometry.MouseDown += onMouseDown;
            CollisionViewGeometry.MouseUp += onMouseUp;
            CollisionViewGeometry.MouseMove += onMouseMove;
            CollisionViewGeometry.CheckCollision += (results) =>
                {
                    if (results.Count > 0)
                        txtCollision.Text = "Colliding with " + results.Count() + " items";
                    else
                        txtCollision.Text = "No collisions";
                };

            CollisionViewGeometry2 = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctTwo2, new DynamicShape(new GeometryViewAdapter(rctTwo2))).AddShape(txtCollision2, new DynamicShape(new GeometryViewAdapter(txtCollision2)));
            CollisionViewGeometry2.MouseDown += onMouseDown;
            CollisionViewGeometry2.MouseUp += onMouseUp;
            CollisionViewGeometry2.MouseMove += onMouseMove;
            CollisionViewGeometry2.CheckCollision += (results) =>
                {
                    if (results.Count > 0)
                        txtCollision2.Text = "Colliding with " + results.Count() + " items";
                    else
                        txtCollision2.Text = "No collisions";
                };

            CollisionViewGeometry3 = new ViewGeometry(CollisionMap, collisionManager).AddShape(rctTwo3, new DynamicShape(new GeometryViewAdapter(rctTwo3))).AddShape(txtCollision3, new DynamicShape(new GeometryViewAdapter(txtCollision3)));
            CollisionViewGeometry3.MouseDown += (result) => { Shape_MouseDown(result.ViewGeometry, result.Element, (MouseButtonEventArgs)result.MouseEventArgs); };
            CollisionViewGeometry3.MouseUp += (result) => { Shape_MouseUp(result.ViewGeometry, result.Element, (MouseButtonEventArgs)result.MouseEventArgs); };
            CollisionViewGeometry3.MouseMove += (result) => { if (isDragging) Shape_MouseMove(result.ViewGeometry, result.Element, (MouseEventArgs)result.MouseEventArgs); };
            CollisionViewGeometry3.CheckCollision += (results) =>
                {
                    if (results.Count > 0)
                        txtCollision3.Text = "Colliding with " + results.Count() + " items";
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

            Timer timer = new Timer(3);
            timer.Elapsed += (param1, param2) =>
                {
                    //This way of invoking prevents crashing during shutdown.
                    Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action
                            (
                                () =>
                                {
                                    timer.Stop();

                                    ViewGeometry.Shape shape = CollisionViewGeometry3.Shapes.FirstOrDefault(p => p.Element == rctTwo3);
                                    CollisionViewGeometry3.Rotate(shape, 0.4f);
                                    OutlineEdges(CollisionViewGeometry3);
                                    CollisionViewGeometry3.OnCheckCollision();
                                    DrawBoundingBox("rotating", CollisionViewGeometry3.Geometry.Rect);
                                    timer.Start();
                                }
                            )
                        );
                };
            timer.Enabled = true;
        }

        private void OutlineEdges(ViewGeometry viewGeometry)
        {
            foreach (var vertex in viewGeometry.Geometry.Vertices)
            {
                foreach (var connection in vertex.VertexConnections)
                {
                    DisplayRayLine(connection.Vertex1.Position.Point, connection.Vertex2.Position.Point, "TraceLine", connection.Name);
                }
            }
        }

        private void DrawBoundingBox(string id, Rect boundingBox)
        {
            DisplayRayLine(boundingBox.TopLeft, boundingBox.TopRight, "BoundingBox", "top" + id);
            DisplayRayLine(boundingBox.TopRight, boundingBox.BottomRight, "BoundingBox", "right" + id);
            DisplayRayLine(boundingBox.BottomLeft, boundingBox.BottomRight, "BoundingBox", "bottom" + id);
            DisplayRayLine(boundingBox.BottomLeft, boundingBox.TopLeft, "BoundingBox", "left" + id);
        }

        private void DisplayRayLine(Point point1, Point point2, string type, string name)
        {
            if (type.ToLower() == "xyline")
            {
                if (name == "rctTwo3txtCollision3")
                    name += rayCount;
                else
                    name = type + rayCount;

                rayCount++;
                if (rayCount > 30)
                    rayCount = 0;
            }

            Line line1 = grdHolder.Children.OfType<Line>().FirstOrDefault(p => p.Name == name + type);
            if (line1 == null)
            {
                line1 = new Line();
                grdHolder.Children.Add(line1);
            }
            line1.Name = name + type;
            line1.X1 = point1.X;
            line1.Y1 = point1.Y;
            line1.X2 = point2.X;
            line1.Y2 = point2.Y;

            if (type.ToLower() == "traceline")
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90551a8b"));//purple
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#90551a8b"));//purple
            }
            else if (type.ToLower() == "collisionline")
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5000d6a0"));//teal
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5000d6a0"));//teal
            }
            else if (type.ToLower() == "xyline")
            {
                line1.X2 += 1;
                line1.Y2 += 1;
                line1.StrokeThickness = 15;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#800000FF"));//blue
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#800000FF"));//blue
            }
            else if (type.ToLower() == "boundingbox")
            {
                line1.StrokeThickness = 2;
                line1.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#70ff8d00"));//orange
                line1.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#70ff8d00"));//orange
            }
        }
    }
}
