using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MEKPEIN
{
    public class Mola
    {
        static Scalar _RED = new Scalar(-30000d, -30000d, 30000d, 30000);

        public static void JakobsLieblingsScript()
        {
            double distanceThresh = 120;
            int dotcount = 200;
            int maxchange = 4;
            var doubleup = false;

            var foregroundCol = ElPalletto.WHITE;
            var bgcolor = ElPalletto.BLACK;

            // stores
            int c, d = 0;
            double dis, opfac = 0;
            List<Point> linedots;

            var lp = new ElPalletto();
            var rand = new Random();

            var lens = Lib.SplitGolden(2000);
            var m = new Mat((int)lens.Y, (int)lens.X, MatType.CV_16SC4);
            m.SetTo(bgcolor);

            //var vw = new VideoWriter();
            //vw.Open("video.avi", FourCC.Prompt, 30, m.Size(), true);

            lp._Color = foregroundCol;
            lp._Color2 = bgcolor;

            // draw random positioned dots
            Point[] dots = new Point[dotcount];
            for (int i = 0; i < dotcount; i++)
            {
                dots[i] = new Point2d(rand.Next(0, (int)lens.X), rand.Next(0, (int)lens.Y));
            }

            for (int i = 0; i < 400; i++)
            {
                //EVOLVE
                if (i % 5 == 0)
                {
                    distanceThresh++;
                }
                if (i % 50 == 0)
                {
                    maxchange++;
                }
                if (i % 100 == 0)
                {
                    //doubleup = !doubleup;

                    //var ctmp = foregroundCol;
                    //foregroundCol = bgcolor;
                    //bgcolor = ctmp;
                }

                for (int j = 0; j < dotcount; j++)
                {
                    c = rand.Next(-maxchange, maxchange + 1);
                    d = rand.Next(-maxchange, maxchange + 1);

                    dots[j].X = dots[j].X + c;
                    dots[j].Y = dots[j].Y + d;
                }

                // Connect those dots mit lines if their Abstand is less than
                // (wert abhängig von canvas Größe) - opacity mapped auf linelenght..
                foreach (var di in dots)
                {
                    foreach (var dj in dots)
                    {
                        if (di == dj)
                            continue;

                        dis = di.DistanceTo(dj);
                        if (dis < distanceThresh)
                        {
                            opfac = dis / distanceThresh;

                            lp._Color = foregroundCol;

                            linedots = Lib.MakeLine(dj.X, dj.Y, di.X, di.Y);
                            if (foregroundCol == ElPalletto.BLACK)
                                lp.Lighten(opfac);
                            else
                                lp.Darken(opfac);

                            foreach (var dot in linedots)
                            {
                                m.Circle(dot, 0, lp._Color, -1);
                            }
                        }
                    }
                }

                Cv2.ImShow("so geiles fensterl", m);
                Cv2.WaitKey(33);
                //vw.Write(m);

                //// wipe
                //if (doubleup && i % 2 == 0)
                //    m.SetTo(bgcolor);
                //else if (!doubleup)
                //    m.SetTo(bgcolor);
            }

            //vw.Release();
            //Ich müsst es noch irgendwo als processingpatch herumliegen haben
        }

        public static void ImmerDerNaseNach()
        {
            var lenab = 1524;
            var strokeLen = 5;
            var brushSize = 20;
            var rotationfactor = 15; // neue maximale rotation in Anteilen von Kreis
            var rotation = 1d / rotationfactor;

            var lens = Lib.SplitGolden(lenab);
            var m = new Mat((int)lens.Y, (int)lens.X, MatType.CV_16SC4);

            var vw = new VideoWriter();
            vw.Open("video.avi", FourCC.Prompt, 29, m.Size(), true);

            Scalar color = new Scalar(30000d, 30000d, 0, 0);
            var colorchangeconst = 1500;

            Point p1 = new Point(lens.X / 2, lens.Y / 2);

            var r = new Random();

            // this is the direction in the circle ranging from 0 to less than 1;
            var direction = r.NextDouble();

            for (int i = 0; i < 30000; i++)
            {
                // COLOR CHANGE
                var chan = r.Next(0, 4); // pick color channel
                var addremnum = r.Next(0, 2) == 0 ? -1 : 1; // add or remove value
                color[chan] = Math.Clamp(color[chan] + (colorchangeconst * addremnum), Int16.MinValue, Int16.MaxValue);

                var circle = Lib.PlotCircle(p1.X, p1.Y, strokeLen);

                var newStep = circle[(int)(circle.Count * direction)];
                newStep = Lib.ContinuePointInRect((int)lens.X, (int)lens.Y, (int)newStep.X, (int)newStep.Y);

                // anmalen
                m.Circle(newStep, brushSize, color, thickness: -1);

                // weiter drehen
                var posnegativemultiplier = r.Next(0, 2) == 0 ? -1 : 1;
                direction = Lib.ContinuePointInRange(1, direction + (rotation * posnegativemultiplier));

                p1 = newStep;

                if (i % 5 == 0)
                {
                    posnegativemultiplier = r.Next(0, 2) == 0 ? -1 : 1;
                    brushSize = Math.Clamp(brushSize + posnegativemultiplier, 5, 30);
                }

                if (i % 66 == 0)
                {
                    vw.Write(m);

                    //Cv2.ImShow("sickname2", m);
                    //Cv2.WaitKey(10);
                }
            }

            vw.Release();

            //Cv2.ImShow("sickname", m);
            //Cv2.WaitKey();
        }

        internal static void SuperSchnit()
        {
            var cryzyform = 2; // 0.5,1,2,3,4

            var lp = new ElPalletto();
            var rand = new Random();

            var brushSizeNP = new NumerPlayer(np =>
            {
                var fac = rand.Next(0, 2) == 0 ? -1 : 1;
                np.N = Math.Clamp(np.N + (fac * rand.Next(0, 4)), 2, 37);
            });
            brushSizeNP.N = 11;

            var radiusInitNp = new NumerPlayer(np =>
            {
                var fac = rand.Next(0, 2) == 0 ? -1 : 1;
                np.N = (int)Math.Clamp(1 + np.N + (fac * np.N * 0.1), 20d, 4000d);
            });
            radiusInitNp.N = 300;

            var r = radiusInitNp.N;
            var quad = 0; //quadrant 0,1,2,3

            var lenab = 2000;
            var lens = Lib.SplitGolden(lenab);
            var m = new Mat((int)lens.Y, (int)lens.X, MatType.CV_16SC4);

            var vw = new VideoWriter();
            vw.Open("video.avi", FourCC.Prompt, 29, m.Size(), true);

            var p1 = new Point((int)lens.X / 2, (int)lens.Y / 2);

            lp._Color = new Scalar(-20000d, 30000d, -29990d, 30000d);
            lp._Color2 = new Scalar(-20000d, 30000d, -29990d, 30000d);

            for (int i = 0; i < 1000000; i++)
            {
                if (r <= brushSizeNP.N / cryzyform)
                {
                    r = radiusInitNp.N;
                    lp._Color = lp._Color2;

                    radiusInitNp.Kick();
                    brushSizeNP.Kick();

                    if (i % 4 == 0)
                    {
                        p1 = new Point2d(rand.Next((int)lens.X), rand.Next((int)lens.Y));
                        lp.RandomizeColorWithAlpha();
                        lp._Color2 = lp._Color;
                    }
                }

                var circle = Lib.PlotCircle(p1.X, p1.Y, r);
                var circpixcountquad = (int)((decimal)circle.Count / 4);

                var segment = circle.GetRange(circpixcountquad * quad, circpixcountquad);

                int j = 0;
                foreach (var p in segment)
                {
                    m.Circle(p, 1, lp._Color, thickness: brushSizeNP.N);
                    lp.CCChanDir();

                    j++;
                    if (j % 150 == 0)
                        vw.Write(m);
                }

                //lp.Darken(0.1);
                var rcut = Lib.SplitGolden(r);
                r = (int)rcut.X;

                p1 = segment.LastOrDefault();

                if (quad == 0)
                    p1.Y = p1.Y - r;
                if (quad == 1)
                    p1.X = p1.X + r;
                if (quad == 2)
                    p1.Y = p1.Y + r;
                if (quad == 3)
                    p1.X = p1.X - r;

                quad = (quad + 1) % 4;

                Cv2.ImShow("supafenster", m);
                Cv2.WaitKey(1);
            }
            vw.Release();
        }

        public static void SoVüStricherln()
        {
            var r = new Random();

            var maxdim = 1024;

            var m = new Mat(maxdim, maxdim, MatType.CV_16SC4);

            var vw = new VideoWriter();
            vw.Open("video.avi", FourCC.Prompt, 29, m.Size(), true);

            Scalar color = new Scalar(10000d, 10000d, 0d, 0d);

            var colorchangeconst = 1000;

            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 0);

            for (int i = 0; i < 10000; i++)
            {
                var chan = r.Next(0, 4); // pick color channel
                var addremnum = r.Next(0, 2) == 0 ? -1 : 1; // add or remove value

                color[chan] = Math.Clamp(color[chan] + (colorchangeconst * addremnum), Int16.MinValue, Int16.MaxValue);

                p2.X = r.Next(0, maxdim);
                p2.Y = r.Next(0, maxdim);

                m.Line(p1, p2, color, 1);

                p1 = p2;

                if (i % 33 == 0)
                {
                    vw.Write(m);

                    Cv2.ImShow("sickname", m);
                    Cv2.WaitKey(10);
                }
            }

            vw.Release();

            //Cv2.ImShow("sickname", m);
            //Cv2.WaitKey();
        }
    }
}
