//*******************************************
//
// (c) Copyright 2010 Cody Neuburger
//       All rights reserved.
//
//*******************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Stars
{
    public class Star
    {
        #region variables
        public Point center;
        public int numPoints;
        public Brush brush;
        public float rotation;
        public double radius;
        public double orgradius;
        public PointCollection Points;
        #endregion
        
        public Star()
        {
            brush = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            numPoints = 5;
            rotation = 0;
            radius = 50;
            setup();
        }
        public Star(Brush b, double r)
        {
            brush = b;
            numPoints = 5;
            rotation = 0;
            radius = r;
            setup();
        }
        public Star(Brush b, double r, int nPoints)
        {
            brush = b;
            radius = r;
            rotation = 0;
            numPoints = nPoints;
            setup();
        }
        void setup()
        {
            rotation += 90;
            float theta = 0;
            Points = new PointCollection();
            orgradius = radius;
            rotation = rotation * ((float)Math.PI / 180);
            numPoints *= 2;
            for (int i = 0; i < numPoints; i++)
            {
                Point p = new Point();
                radius = orgradius / (1.5 + .5 * Math.Pow(-1, i)); // 2,1,2,1,2,1,2...
                theta = ((2 * (float)Math.PI) / (float)numPoints) * (float)i + rotation;
                p.X = center.X + (radius * (float)Math.Cos(theta));
                p.Y = center.Y + (radius * (float)Math.Sin(theta));
                Points.Add(p);
            }
            numPoints /= 2;
        }
        public void rotate(float degrees)
        {
            rotation = degrees;
            setup();
        }
        public void changeNumPoints(int num)
        {
            numPoints = num;
            setup();
        }
        public void setCenter(Point p)
        {
            center = p;
            setup();
        }
        public void resize(double r)
        {
            radius = r;
            setup();
        }
    }
}
