using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Interfaces
{
    public interface IPosition
    {
        Point Point { get; set; }
        double X { get; }
        double Y { get; }

    }
}
