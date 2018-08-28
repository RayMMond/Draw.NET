using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Draw.NET
{
    public static class DrawingExtensions
    {
        public static float GetResizeFactor(this SizeF sz1, SizeF sz2)
        {
            float dh = sz1.Height / sz2.Height;
            float dw = sz1.Width / sz2.Width;
            if (Math.Abs(dh - 1) > Math.Abs(dw - 1))
                return dw;
            else
                return dh;
        }


        public static PointF GetCornerPoint(this RectangleF rect, CornerType type)
        {
            switch (type)
            {
                case CornerType.LeftTop:
                    return rect.Location;
                case CornerType.LeftMiddle:
                    return new PointF(rect.X, rect.Y + rect.Height / 2);
                case CornerType.LeftBottom:
                    return new PointF(rect.X, rect.Bottom);
                case CornerType.Top:
                    return new PointF(rect.X + rect.Width / 2, rect.Y);
                case CornerType.Bottom:
                    return new PointF(rect.X + rect.Width / 2, rect.Bottom);
                case CornerType.RightTop:
                    return new PointF(rect.Right, rect.Y);
                case CornerType.RightMiddle:
                    return new PointF(rect.Right, rect.Y + rect.Height / 2);
                case CornerType.RightBottom:
                    return new PointF(rect.Right, rect.Bottom);
                case CornerType.Center:
                    return rect.Location + new SizeF(rect.Width / 2, rect.Height / 2);
                default:
                    return PointF.Empty;
            }
        }

        /// <summary>
        /// 对已有的矩形做缩放
        /// </summary>
        /// <param name="rect">原始矩形</param>
        /// <param name="type">操作类型，左上角、中段、右上角等</param>
        /// <param name="p">当前光标位置</param>
        /// <param name="resizeFixRadio">是否等比例</param>
        /// <returns></returns>
        public static RectangleF Resize(this RectangleF rect, CornerType type, PointF p, bool resizeFixRadio)
        {
            RectangleF result = Rectangle.Empty;
            switch (type)
            {
                case CornerType.LeftTop:
                    result = ResizeByLeftTopHandle(rect, p, resizeFixRadio);
                    break;
                case CornerType.LeftBottom:
                    result = ResizeByLeftBottomHandle(rect, p, resizeFixRadio);
                    break;
                case CornerType.RightTop:
                    result = ResizeByRightTopHandle(rect, p, resizeFixRadio);
                    break;
                case CornerType.RightBottom:
                    result = ResizeByRightBottom(rect, p, resizeFixRadio);
                    break;
                case CornerType.LeftMiddle:
                    result = ResizeByLeftMiddleHandle(rect, p);
                    break;
                case CornerType.Top:
                    result = ResizeByTopHandle(rect, p);
                    break;
                case CornerType.Bottom:
                    result = ResizeByBottomHandle(rect, p);
                    break;
                case CornerType.RightMiddle:
                    result = ResizeByRightMiddleBottom(rect, p);
                    break;
                case CornerType.Center:
                default:
                    break;
            }
            return result;
        }

        public static RectangleF GetBounds(this List<PointF> pts)
        {
            float x = float.MaxValue, y = float.MaxValue, _x = float.MinValue, _y = float.MinValue;
            for (int i = 0; i < pts.Count; i++)
            {
                var b = pts[i];
                if (b.X < x) x = b.X;
                if (b.Y < y) y = b.Y;
                if (b.X > _x) _x = b.X;
                if (b.Y > _y) _y = b.Y;
            }
            if (_x - x != 0 || _y - y != 0)
            {
                return new RectangleF(x, y, _x - x, _y - y);
            }
            else
            {
                return new RectangleF(new PointF(x, y), SizeF.Empty);
            }

        }

        public static SizeF ToSizeF(this PointF pt)
        {
            return new SizeF(pt);
        }

        #region 四角的缩放
        private static void GetLineABValue(PointF p1, PointF p2, out float a, out float b)
        {
            if (p1.X == p2.X)
            {
                a = 1;
            }
            else
            {
                a = (p1.Y - p2.Y) / (p1.X - p2.X);
            }
            b = p1.Y - p1.X * a;
        }
        private static RectangleF ResizeByLeftTopHandle(RectangleF rect, PointF p, bool resizeFixRadio)
        {
            PointF p1 = new PointF(rect.Right, rect.Bottom);
            PointF p2 = rect.Location;
            GetLineABValue(p1, p2, out float a, out float b);
            RectangleF result;
            if (p.Y >= p1.Y && p.X >= p1.X)
            {
                return RectangleF.FromLTRB(p1.X - 1, p1.Y - 1, p1.X, p1.Y);
            }
            if (resizeFixRadio)
            {
                if (p.Y > a * p.X + b)
                {
                    result = RectangleF.FromLTRB(p.X, (float)(a * p.X + b), p1.X, p1.Y);
                }
                else
                {
                    result = RectangleF.FromLTRB((float)((p.Y - b) / a), p.Y, p1.X, p1.Y);
                }
            }
            else
            {
                result = RectangleF.FromLTRB(p.X, p.Y, p1.X, p1.Y);
            }

            return result;
        }
        private static RectangleF ResizeByLeftMiddleHandle(RectangleF rect, PointF p)
        {
            if (p.X >= rect.Right)
            {
                return RectangleF.FromLTRB(rect.Right - 1, rect.Y, rect.Right, rect.Bottom);
            }
            return RectangleF.FromLTRB(p.X, rect.Y, rect.Right, rect.Bottom);
        }
        private static RectangleF ResizeByLeftBottomHandle(RectangleF rect, PointF p, bool resizeFixRadio)
        {
            PointF p1 = new PointF(rect.Right, rect.Y);
            PointF p2 = new PointF(rect.X, rect.Bottom);
            GetLineABValue(p1, p2, out float a, out float b);

            RectangleF result;
            if (p.Y <= p1.Y && p.X >= p1.X)
            {
                return RectangleF.FromLTRB(p1.X - 1, p1.Y, p1.X, p1.Y + 1);
            }
            if (resizeFixRadio)
            {
                if (p.Y > a * p.X + b)
                {
                    result = RectangleF.FromLTRB((float)((p.Y - b) / a), p1.Y, p1.X, p.Y);
                }
                else
                {
                    result = RectangleF.FromLTRB(p.X, p1.Y, p1.X, (float)(a * p.X + b));
                }
            }
            else
            {
                result = RectangleF.FromLTRB(p.X, p1.Y, p1.X, p.Y);
            }

            return result;
        }
        private static RectangleF ResizeByBottomHandle(RectangleF rect, PointF p)
        {
            if (p.Y <= rect.Y)
            {
                return RectangleF.FromLTRB(rect.X, rect.Y, rect.Right, rect.Y + 1);
            }
            return RectangleF.FromLTRB(rect.X, rect.Y, rect.Right, p.Y);
        }
        private static RectangleF ResizeByRightBottom(RectangleF rect, PointF p, bool resizeFixRadio)
        {
            PointF p1 = new PointF(rect.X, rect.Y);
            PointF p2 = new PointF(rect.Right, rect.Bottom);
            GetLineABValue(p1, p2, out float a, out float b);

            RectangleF result;
            if (p.Y <= p1.Y && p.X <= p1.X)
            {
                return RectangleF.FromLTRB(p1.X, p1.Y, p1.X + 1, p1.Y + 1);
            }
            if (resizeFixRadio)
            {
                if (p.Y > a * p.X + b)
                {
                    result = RectangleF.FromLTRB(p1.X, p1.Y, (float)((p.Y - b) / a), p.Y);
                }
                else
                {
                    result = RectangleF.FromLTRB(p1.X, p1.Y, p.X, (float)(a * p.X + b));
                }
            }
            else
            {
                result = RectangleF.FromLTRB(p1.X, p1.Y, p.X, p.Y);
            }

            return result;
        }
        private static RectangleF ResizeByRightMiddleBottom(RectangleF rect, PointF p)
        {
            if (p.X <= rect.X)
            {
                return RectangleF.FromLTRB(rect.X, rect.Y, rect.X + 1, rect.Bottom);
            }
            return RectangleF.FromLTRB(rect.X, rect.Y, p.X, rect.Bottom);
        }
        private static RectangleF ResizeByRightTopHandle(RectangleF rect, PointF p, bool resizeFixRadio)
        {
            PointF p1 = new PointF(rect.X, rect.Bottom);
            PointF p2 = new PointF(rect.Right, rect.Y);
            GetLineABValue(p1, p2, out float a, out float b);

            RectangleF result;
            if (p.Y >= p1.Y && p.X <= p1.X)
            {
                return RectangleF.FromLTRB(p1.X, p1.Y - 1, p1.X + 1, p1.Y);
            }
            if (resizeFixRadio)
            {
                if (p.Y > a * p.X + b)
                {
                    result = RectangleF.FromLTRB(p1.X, (float)(a * p.X + b), p.X, p1.Y);
                }
                else
                {
                    result = RectangleF.FromLTRB(p1.X, p.Y, (float)((p.Y - b) / a), p1.Y);
                }
            }
            else
            {
                result = RectangleF.FromLTRB(p1.X, p.Y, p.X, p1.Y);
            }

            return result;
        }
        private static RectangleF ResizeByTopHandle(RectangleF rect, PointF p)
        {
            if (p.Y >= rect.Bottom)
            {
                return RectangleF.FromLTRB(rect.X, rect.Bottom - 1, rect.Right, rect.Bottom);
            }
            return RectangleF.FromLTRB(rect.X, p.Y, rect.Right, rect.Bottom);
        }


        #endregion

    }
}
