using System;
using System.Linq;

namespace LEDBackpackController
{
    public class Brightnesses
    {
        public byte[] Values = { 0, 0, 0, 0, 0, 0 };
        public Brightnesses(byte strips_front, byte strip_edge, byte strip_strap_left, 
                            byte strip_strap_right, byte strip_circles, byte matrix)
        {
            Values[0] = strips_front;
            Values[1] = strip_edge;
            Values[2] = strip_strap_left;
            Values[3] = strip_strap_right;
            Values[4] = strip_circles;
            Values[5] = matrix;
        }
    }
}