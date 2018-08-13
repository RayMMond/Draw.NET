using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractEllipse : AbstractPrimitive, IEllipse
    {
        protected float width;
        protected float height;
        protected PointF location;


        protected AbstractEllipse(PointF location, SizeF size)
            : base()
        {
            this.location = location;
            width = size.Width;
            height = size.Height;
        }



        /// <summary>
        /// 椭圆的长半轴
        /// </summary>
        public virtual float Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value != width)
                {
                    width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }


        /// <summary>
        /// 椭圆的短半轴
        /// </summary>
        public float Height
        {
            get { return height; }
            set
            {
                if (value != height)
                {
                    height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public SizeF Size
        {
            get { return new SizeF(width, height); }
        }


        public virtual PointF Location
        {
            get { return location; }
            set
            {
                if (value != location)
                {

                    location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public override RectangleF GetBounds()
        {
            return new RectangleF(Location, Size);
        }


        protected List<EllipsePoint> ComputerEllipseBoundPoint()
        {
            var list = new List<EllipsePoint>();

            var xc = (int)(Location.X - width / 2);
            var yc = (int)(Location.Y - height / 2);
            int sqa = (int)(Width * Width);
            int sqb = (int)(Height * Height);
            int x = 0;
            int y = (int)Height;
            int d = 2 * sqb - 2 * ((int)Height) * sqa + sqa;

            list.Add(EllipsePlot(xc, yc, x, y));

            int px = (int)Math.Round((double)sqa / Math.Sqrt((double)(sqa + sqb)));

            while (x <= px)
            {
                if (d < 0)
                {
                    d += 2 * sqb * (2 * x + 3);
                }
                else
                {
                    d += 2 * sqb * (2 * x + 3) - 4 * sqa * (y - 1);
                    y--;
                }
                x++;
                list.Add(EllipsePlot(xc, yc, x, y));
            }

            d = sqb * (x * x + x) + sqa * (y * y - y) - sqa * sqb;

            while (y >= 0)
            {
                list.Add(EllipsePlot(xc, yc, x, y));
                y--;
                if (d < 0)
                {
                    x++;
                    d = d - 2 * sqa * y - sqa + 2 * sqb * x + 2 * sqb;
                }
                else
                {
                    d = d - 2 * sqa * y - sqa;
                }
            }

            return list;
        }

        private EllipsePoint EllipsePlot(int xc, int yc, int x, int y)
        {

            var tempEllipsePoint = new EllipsePoint();
            var Newx0 = x + xc;
            var Newy0 = y + yc;

            tempEllipsePoint.x0 = Newx0;
            tempEllipsePoint.y0 = Newy0;

            var Newx1 = x + xc;
            var Newy1 = yc - y;

            tempEllipsePoint.x1 = Newx1;
            tempEllipsePoint.y1 = Newy1;

            var Newx2 = xc - x;
            var Newy2 = yc - y;

            tempEllipsePoint.x2 = Newx2;
            tempEllipsePoint.y2 = Newy2;


            var Newx3 = xc - x;
            var Newy3 = yc + y;

            tempEllipsePoint.x3 = Newx3;

            tempEllipsePoint.y3 = Newy3;

            return tempEllipsePoint;

        }

        /// <summary>
        ///椭圆四个象限的坐标点
        /// </summary>
        public struct EllipsePoint
        {
            public float x0;

            public float y0;

            public float x1;

            public float y1;

            public float x2;

            public float y2;

            public float x3;

            public float y3;
        }
    }
}
