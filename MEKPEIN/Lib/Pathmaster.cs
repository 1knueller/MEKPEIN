using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MEKPEIN
{
    public class Pathmaster
    {
        public static string AppData_MEKPEIN => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"MEKPEIN");
        public static string Images => Path.Combine(AppData_MEKPEIN, "images");
    }
}
