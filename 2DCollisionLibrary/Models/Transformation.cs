using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DCollisionLibrary.Models
{
    public class Transformation : ITransformation
    {
        public double Rotation { get; set; }
        public double ScaleX { get; set; }
        public double ScaleY { get; set; }

        public Transformation()
        {
            ScaleX = 1;
            ScaleY = 1;
        }
    }
}
