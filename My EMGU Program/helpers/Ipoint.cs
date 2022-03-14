using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_EMGU_Program.helpers
{
    internal class Ipoint
    {
        float x;
        float y;
        private float scaleX;
        private float scaleY;

        Ipoint(float x,float y)
        {
            this.x = x; 
            this.y = y;
        }
    }
}
