using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace My_EMGU_Program.models
{
    class Object2d
    {
        public List<Lines> lines= new List<Lines>();
        public Color color;

        List<Point> points = new List<Point>();
        public int type = 0;

        public void addLine(Lines line)
        {
            lines.Add(line);
        }

        
        public List<Lines> getLines()
        {
            return lines;
        }

        public List<Point> getPoints()
        {
            return points;
        }

        public double calcArea()
        {
            double area = 0;
            Point startPoint = lines[0].getLines()[0];
            int i, j;


            double prevx = 0.0;
            double prevy = 0.0;

            lines.ForEach(it =>
            {
               // List<Point> pp = it.getLines();
                List<Point> point = it.getLines();
                if (point[1] == null)
                {
                    point[1] = startPoint;
                }
                double xdiff = (point[1].X - point[0].X) * it.scaleX;
                double ydiff = (point[1].Y - point[0].Y) * it.scaleY;

                area += (prevx * ydiff);
                area -= (prevy * xdiff);
                prevx += xdiff;
                prevy += ydiff;

            });

            area /= 2;
            return (area>0)?area:-area;

        }

        public double getLength()
        {
            double length = 0.0f;

            lines.ForEach(p =>
            {
                List<Point> pp = p.getLines(); 
                length += Math.Sqrt(
                         (Math.Pow(Math.Abs(pp[0].X - pp[1].X) * p.scaleX, 2.0))
                         +
                          (Math.Pow(Math.Abs(pp[0].Y - pp[1].Y) * p.scaleY, 2.0)));

            });

            return length;
        }


        public List<Point> getAllPoints()
        {
            List<Point> ppo = new List<Point>();
            lines.ForEach(l =>
                l.getLines().ForEach(it =>
                    {
                        if (it != null)
                        {
                            ppo.Add(it);
                        }
                    }
                )
           );
           return ppo;
        }
           
        

        public void addPoints(Point line)
        {
            points.Add(line);
        }

    }
}
