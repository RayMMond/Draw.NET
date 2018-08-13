using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractLine : AbstractPrimitive
    {

        protected PointF startPoint;
        /// <summary>
        /// 线段起点
        /// </summary>
        public virtual PointF StartPoint
        {
            get { return startPoint; }
            set
            {
                if (value != startPoint)
                {
                    startPoint = value;
                    OnPropertyChanged(nameof(StartPoint));
                }
            }
        }



        protected PointF endPoint;
        /// <summary>
        /// 线段终点
        /// </summary>
        public virtual PointF EndPoint
        {
            get { return endPoint; }
            set
            {
                if (value != endPoint)
                {
                    endPoint = value;
                    OnPropertyChanged(nameof(EndPoint));
                }
            }
        }


        public override RectangleF GetBounds()
        {
            //TODO:ZSW 实现获取边界
            throw new NotImplementedException();
        }

        public List<AbstractLine> GetDashLines()
        {
            return GetDashLines(startPoint, endPoint);
        }

        public static List<AbstractLine> GetDashLines(PointF s, PointF e)
        {
            var list = new List<AbstractLine>();

            var len = (int)Math.Abs(e.X - s.X);
            if (len == 0)
            {
                len = (int)Math.Abs(e.Y - s.Y);
            }


            for (int j = 0; j < len; j = j + 8)
            {
                if (s.X == e.X)
                {
                    list.Add(new DashLine()
                    {
                        startPoint = new PointF(s.X, s.Y + j),
                        endPoint = new PointF(s.X, s.Y + j + 4)
                    });
                }
                else if (s.Y == e.Y)
                {
                    list.Add(new DashLine()
                    {
                        startPoint = new PointF(s.X + j, s.Y),
                        endPoint = new PointF(s.X + j + 4, s.Y)
                    });
                }
            }
            var l = new DashLine()
            {
                startPoint = Point.Empty,
                endPoint = e,
            };

            if (list.Count > 1)
            {
                l.startPoint = list.Last().endPoint;
            }
            else
            {
                l.startPoint = s;
            }

            list.Add(l);
            return list;
        }

        class DashLine : AbstractLine
        {
            public override void Draw(object state)
            {
                return;
            }

            public override void Prepare(object state)
            {
                return;
            }
        }
    }
}
