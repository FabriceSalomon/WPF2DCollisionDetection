using _2DCollisionLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2DCollisionLibrary.Helpers
{
    public class TriangleHelper
    {
        private double? _adjacentLenght;
        private double? _hypotenuseLenght;
        private double? _oppositeLenght;
        private double? _hypotenuseAngle;
        public double AdjacentLenght
        {
            get
            {
                if (_adjacentLenght != null)
                    return _adjacentLenght.Value;

                if (_hypotenuseAngle != null && _hypotenuseLenght != null)
                    return (Math.Cos(Utility2DMath.ToRadians(_hypotenuseAngle.Value)) * _hypotenuseLenght.Value);

                return double.NaN;
            }
        }
        public double HypotenuseLenght
        {
            get
            {
                if (_hypotenuseLenght != null)
                    return _hypotenuseLenght.Value;

                if (_adjacentLenght != null && _oppositeLenght != null)
                    return Math.Sqrt(Math.Pow(_adjacentLenght.Value, 2) + Math.Pow(_oppositeLenght.Value, 2));

                return double.NaN;
            }
        }
        public double OppositeLenght
        {
            get
            {
                if (_oppositeLenght != null)
                    return _oppositeLenght.Value;

                if (_hypotenuseAngle != null && _hypotenuseLenght != null)
                    return Math.Cos(Utility2DMath.ToRadians(90 - _hypotenuseAngle.Value)) * -_hypotenuseLenght.Value;

                return double.NaN;
            }
        }
        public double HypotenuseAngle
        {
            get
            {
                if (_hypotenuseAngle != null)
                    return _hypotenuseAngle.Value;

                if (_adjacentLenght != null && _hypotenuseLenght != null && _oppositeLenght != null)
                {
                    var invert = (_oppositeLenght.Value < 0 ? -1 : 1);
                    return (180 - Utility2DMath.ToDegrees(Math.Acos(_adjacentLenght.Value / _hypotenuseLenght.Value))) * invert;
                }

                return double.NaN;
            }
        }
        public Point Origin { get; set; }

        public TriangleHelper(double? adjacent, double? hypotenuse, double? opposite, double? hypotenuseAngle, Point origin)
        {
            _adjacentLenght = adjacent;
            _hypotenuseLenght = hypotenuse;
            _oppositeLenght = opposite;
            _hypotenuseAngle = hypotenuseAngle;
            Origin = origin;
            Update();
        }

        private void Update()
        {
            if (AdjacentLenght != double.NaN)
                _adjacentLenght = AdjacentLenght;
            if (HypotenuseLenght != double.NaN)
                _hypotenuseLenght = HypotenuseLenght;
            if (OppositeLenght != double.NaN)
                _oppositeLenght = OppositeLenght;
            if (HypotenuseAngle != double.NaN)
                _hypotenuseAngle = HypotenuseAngle;
        }
    }
}
