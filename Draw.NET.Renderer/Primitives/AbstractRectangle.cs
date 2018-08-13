using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractRectangle : AbstractPrimitive, IRectangle
    {

        protected AbstractRectangle(PointF location, SizeF size)
        {
            this.location = location;
            rectWidth = size.Width;
            rectHeight = size.Height;
        }

        protected AbstractRectangle(RectangleF rect)
        {
            location = rect.Location;
            rectWidth = rect.Width;
            rectHeight = rect.Height;
        }



        protected PointF location;
        /// <summary>
        /// 矩形左上角坐标
        /// </summary>
        public PointF Location
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

        protected float rectHeight;

        /// <summary>
        /// 矩形高度
        /// </summary>
        public float RectHeight
        {
            get { return rectHeight; }
            set
            {
                if (value != rectHeight)
                {
                    rectHeight = value;
                    OnPropertyChanged(nameof(RectHeight));
                }
            }
        }


        protected float rectWidth;

        /// <summary>
        /// 矩形宽度
        /// </summary>
        public float RectWidth
        {
            get { return rectWidth; }
            set
            {
                if (value != rectWidth)
                {
                    rectWidth = value;
                    OnPropertyChanged(nameof(RectWidth));
                }
            }
        }

        protected DrawModel drawModel = DrawModel.LeftTop;

        public DrawModel DrawModel
        {
            get { return drawModel; }
            set
            {
                if (value != drawModel)
                {
                    drawModel = value;
                    OnPropertyChanged(nameof(DrawModel));
                }
            }
        }

        public SizeF RectSize
        {
            get
            {
                return new SizeF(RectWidth, rectHeight);
            }
        }

        public PointF Center
        {
            get
            {
                return Location + new SizeF(rectWidth / 2, rectHeight / 2);
            }
            set
            {
                Location = value - new SizeF(rectWidth / 2, rectHeight / 2);
            }
        }

        public float Left { get { return location.X; } }

        public float Right { get { return location.X + rectWidth; } }


        public float Top { get { return location.Y; } }


        public float Bottom { get { return location.Y + rectHeight; } }


        #region IText 接口

        #endregion

        public override RectangleF GetBounds()
        {
            return new RectangleF(location, RectSize);
        }
    }
}
