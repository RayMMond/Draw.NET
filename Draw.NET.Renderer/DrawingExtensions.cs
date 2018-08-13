/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       DrawingExtensions 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 10:00:58
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer
{
    public static class Extensions
    {
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

    }
}

namespace System.Drawing
{
    public static class Extensions
    {
        public static SizeF ToSizeF(this PointF pt)
        {
            return new SizeF(pt);
        }

    }
}
