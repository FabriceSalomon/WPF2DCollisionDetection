using _2DCollisionDetection.Objects;
using _2DCollisionLibrary.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2DCollisionDetection
{
    /// <summary>
    /// Interaction logic for DetailedRectangle.xaml
    /// </summary>
    public partial class ElementInfoControl : UserControl
    {
        public ElementInfoControl()
        {
            InitializeComponent();
        }

        public void UpdateData(string name, ViewGeometry viewGeometry)
        {
            lblName.Text = "Name: " + name;
            lbl11.Text = "11: X:" + viewGeometry.Geometry.Rect.TopLeft.X.ToString("0.0") + " Y:" + viewGeometry.Geometry.Rect.TopLeft.Y.ToString("0.0");
            lbl12.Text = "12: X:" + viewGeometry.Geometry.Rect.TopRight.X.ToString("0.0") + " Y:" + viewGeometry.Geometry.Rect.TopRight.Y.ToString("0.0");
            lbl21.Text = "21: X:" + viewGeometry.Geometry.Rect.BottomLeft.X.ToString("0.0") + " Y:" + viewGeometry.Geometry.Rect.BottomLeft.Y.ToString("0.0");
            lbl22.Text = "22: X:" + viewGeometry.Geometry.Rect.BottomRight.X.ToString("0.0") + " Y:" + viewGeometry.Geometry.Rect.BottomRight.Y.ToString("0.0");
            lblHeight.Text = "Height: " + viewGeometry.Geometry.Rect.Height.ToString("0.0");
            lblWidth.Text = "Width: " + viewGeometry.Geometry.Rect.Width.ToString("0.0");
        }
    }
}
