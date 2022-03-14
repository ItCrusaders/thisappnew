using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace My_EMGU_Program.models
{
    class Lines : Shapes
    {
        private Point point1;
        private Point point2;
        public float scaleX;
        public float scaleY;


        public List<Point> getLines()
        {
            return new List<Point> { point1, point2 };//.Add(point1).Add(point2);
        }

        public void setPoint(Point x,float sx,float sy)
        {
            if(point1 == null)
                point1 = new Point(x.X, x.Y);
            else if (point2==null)
            {
                this.scaleX = sx;
                this.scaleY = sy;
                point2 = new Point(x.X, x.Y);
            }
        }

        public void setPoint1(Point x, float sx, float sy)
        {
         //   if (point1 == null)
                point1 = new Point(x.X, x.Y);
           
        }

        public void setPoint2(Point x, float sx, float sy)
        {
            
                this.scaleX = sx;
                this.scaleY = sy;
                point2 = new Point(x.X, x.Y);
            
        }

    }
}
