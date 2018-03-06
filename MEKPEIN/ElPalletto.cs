using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEKPEIN
{
    public class ElPalletto
    {
        Random _R = new Random();
        public Scalar _Color = new Scalar(10000d, 10000d, 0d, 0d);
        public Scalar _Color2 = new Scalar(10000d, 10000d, 0d, 0d);

        double colorchangeconst = 1000d;

        public static Scalar BLACK = new Scalar(Int16.MinValue, Int16.MinValue, Int16.MinValue, Int16.MaxValue);
        public static Scalar WHITE = new Scalar(Int16.MaxValue, Int16.MaxValue, Int16.MaxValue, Int16.MaxValue);
        
        /// <summary>
        /// Changes a channel of the color in a direction by the const
        /// </summary>
        public Scalar CCChanDir()
        {
            var chan = _R.Next(0, 4); // pick color channel
            var addremnum = _R.Next(0, 2) == 0 ? -1 : 1; // add or remove value

            return _Color[chan] = Math.Clamp(_Color[chan] + (colorchangeconst * addremnum), Int16.MinValue, Int16.MaxValue);
        }

        /// <summary>
        /// factor from and including 0...1
        /// 0 = no change, 1 = black
        /// </summary>
        public Scalar Darken(double shadeFactor)
        {
            _Color.Val0 = ((_Color.Val0 + Int16.MaxValue) * (1 - shadeFactor)) - Int16.MaxValue;
            _Color.Val1 = ((_Color.Val1 + Int16.MaxValue) * (1 - shadeFactor)) - Int16.MaxValue;
            _Color.Val2 = ((_Color.Val2 + Int16.MaxValue) * (1 - shadeFactor)) - Int16.MaxValue;

            return _Color;
        }

        /// <summary>
        /// factor from and including 0...1
        /// 0 = no change, 1 = white
        public Scalar Lighten(double factor)
        {
            return Layer(WHITE, factor);
        }

        /// <summary>
        /// layers a color over our current one with given alpha </summary>
        public Scalar Layer(Scalar layer, double alpha)
        {
            _Color.Val0 += (layer.Val0 - _Color.Val0) * alpha;
            _Color.Val1 += (layer.Val1 - _Color.Val1) * alpha;
            _Color.Val2 += (layer.Val2 - _Color.Val2) * alpha;
            return _Color;
        }

        public Scalar RandomizeColor()
        {
            var a = _Color.Val3;
            _Color = new Scalar(
                _R.Next(Int16.MinValue, Int16.MaxValue),
                _R.Next(Int16.MinValue, Int16.MaxValue),
                _R.Next(Int16.MinValue, Int16.MaxValue),
                a);
            return _Color;
        }

        public Scalar RandomizeColorWithAlpha()
        {
            _Color = new Scalar(
                _R.Next(Int16.MinValue, Int16.MaxValue),
                _R.Next(Int16.MinValue, Int16.MaxValue),
                _R.Next(Int16.MinValue, Int16.MaxValue),
                _R.Next(Int16.MinValue, Int16.MaxValue));
            return _Color;
        }
    }
}