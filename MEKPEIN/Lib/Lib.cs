using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEKPEIN
{
    public class Lib
    {
        //ioen cv color = blue green red alpha

        public const double GoldenRatio = 1.61803398874989484820458683436;

        public static Point2d SplitGolden(int ab)
        {
            var x = ab / GoldenRatio;
            return new Point2d(x, ab - x);
        }

        public static int KeepInRangePerChance(int num, int min, int max, int factorOfChangeOfRange, int bouncyness)
        {
            // keeps number in range by reducing 
            // the chance of chaning into direction 
            // of border while approaching it

            throw new NotImplementedException();

            var range = (decimal)(max - min);
            var halfrange = range / 2;
            var qrange = halfrange / 2;

            if (num < max - qrange && num > min + qrange)
            {
                //allow change without chance
                return 0;
            }
            if (num > max - qrange)
            {
                // change with better chance to decrease
                return 0;
            }
            if (num < min + qrange)
            {
                // change iwht better chance to increase
                return 0;
            }

            return 0;
        }

         public static List<Point> MakeLine(int x0, int y0, int x1, int y1)
        {
            var line = new List<Point>();

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2; /* error value e_xy */

            while (true)
            {
                line.Add(new Point(x0, y0));
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 > dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
                if (e2 < dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
            }

            return line;
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
            return new Point2d(ContinuePointInRange(rx, x), ContinuePointInRange(ry, y));
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
        /// 3 | 2
        /// --+--
        /// 1 | 0
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
