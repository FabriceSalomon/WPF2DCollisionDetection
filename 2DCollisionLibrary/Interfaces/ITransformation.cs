using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Interfaces
{
    public interface ITransformation
    {
        double Rotation { get; set; }
        double ScaleX { get; set; }
        double ScaleY { get; set; }
    }
}
