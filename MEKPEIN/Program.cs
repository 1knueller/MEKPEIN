using System;
using System.Collections.Generic;
using MEKPEIN.soup;
using OpenCvSharp;

namespace MEKPEIN
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new GoogleSoup();
            var t = x.Ting();
            t.Wait();

            //Hirni.BasicBrain.CrazyTest();


            //Mola.JakobsLieblingsScript();
            //Mola.ImmerDerNaseNach();
            //Mola.SuperSchnit();
            //Mola.SoVüStricherln();
        }
    }
}
