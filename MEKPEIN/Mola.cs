using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace MEKPEIN
{
    public class Mola
    {
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

        public static void SoVüStricherln()
        {
            var r = new Random();

            var maxdim = 1024;

            var m = new Mat(maxdim, maxdim, MatType.CV_16SC4);

            var vw = new VideoWriter();
            vw.Open("video.avi", FourCC.Prompt, 29, m.Size(), true);

            Scalar color = new Scalar(10000d, 10000d, 10000d, 10000d);

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
