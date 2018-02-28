using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEKPEIN
{
    public class Lib
    {
        public const double GoldenRatio = 1.61803398874989484820458683436;

        public static Point2d SplitGolden(int ab)
        {
            var x = ab / GoldenRatio;
            return new Point2d(x, ab - x);
        }

        public static int KeepInRangePerChance(int num, int min, int max,int bouncyness)
        {
            // keeps number in range by reducing 
            // the chance of chaning into direction 
            // of border while approaching it
            throw new NotImplementedException("KeepInRangePerChance");
            return 0;
        }

        /// <summary>
        /// Like snake
        /// </summary>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point2d ContinuePointInRect(int rx, int ry, int x, int y)
        {
            return new Point2d(ContinuePointInRange(rx,x), ContinuePointInRange(ry,y));
        }

        /// <summary>
        /// value will be snakestyle 0 or greater but smaller given range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static int ContinuePointInRange(int range, int point)
        {
            if (point < 0)
                return point = range - (Math.Abs(point) % range);

            return point = point % range;
        }

        /// <summary>
        /// value will be snakestyle 0 or greater but smaller given range
        /// </summary>
        /// <param name="range"></param>
        /// <param name="point"></param>
        public static double ContinuePointInRange(double range, double point)
        {
            if (point < 0)
                return point = range - (Math.Abs(point) % range);

            return point = point % range;
        }

        /// <summary>
        /// returns list of points of circle starting from bottom
        /// </summary>
        /// <param name="xm"></param>
        /// <param name="ym"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<Point2d> PlotCircle(int xm, int ym, int r)
        {
            var q1 = new List<Point2d>();
            var q2 = new List<Point2d>();
            var q3 = new List<Point2d>();
            var q4 = new List<Point2d>();

            int x = -r, y = 0, err = 2 - 2 * r;           /* bottom left to top right */
            do
            {
                q1.Add(new Point2d(xm - x, ym + y));      /*   I. Quadrant +x +y */
                q2.Add(new Point2d(xm - y, ym - x));      /*  II. Quadrant -x +y */
                q3.Add(new Point2d(xm + x, ym - y));      /* III. Quadrant -x -y */
                q4.Add(new Point2d(xm + y, ym + x));      /*  IV. Quadrant +x -y */

                r = err;
                if (r <= y) err += ++y * 2 + 1;        /* e_xy+e_y < 0 */
                if (r > x || err > y)                  /* e_xy+e_x > 0 or no 2nd y-step */
                    err += ++x * 2 + 1;                /* -> x-step now */
            } while (x < 0);

            q1.AddRange(q2);
            q1.AddRange(q3);
            q1.AddRange(q4);

            return q1;
        }
    }
}
