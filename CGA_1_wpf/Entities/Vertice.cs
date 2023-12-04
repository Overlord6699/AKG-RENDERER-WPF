using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Entities
{
    public struct Vertice
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Z { get; set; }

        public Vertice(int x, int y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
