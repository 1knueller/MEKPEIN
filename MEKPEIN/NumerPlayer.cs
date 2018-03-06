using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MEKPEIN
{
    /// <summary>
    /// This class can store values and shift them
    /// </summary>
    public class NumerPlayer
    {
        public NumerPlayer(Action<NumerPlayer> a)
        {
            A = a;
        }

        public int N = 0;
        public Action<NumerPlayer> A;
        public void Kick() { A.Invoke(this); }

        public Point Point { get; set; }
    }
}
