/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       DrawingExtensions 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 16:48:09
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Core.Layers;
using Draw.NET.Core.Shapes;



namespace Draw.NET.Core
{
    public static class Compute
    {
        /// <summary>
        /// 根据两点获取外包矩形
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static RectangleF GetRectangleByPoints(PointF p1, PointF p2)
        {
            float width, height, X, Y;
            if (p1.X > p2.X)
            {
                X = p2.X;
                width = p1.X - p2.X;
            }
            else
            {
                X = p1.X;
                width = p2.X - p1.X;
            }
            if (p1.Y > p2.Y)
            {
                Y = p2.Y;
                height = p1.Y - p2.Y;
            }
            else
            {
                Y = p1.Y;
                height = p2.Y - p1.Y;
            }
            return new RectangleF(X, Y, width, height);
        }
    }

    public static class DrawingFrameworkExtension
    {
        /// <summary>
        /// 判断指定点是否可以命中任何图形
        /// </summary>
        /// <param name="layers"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static AbstractShape IsVisible(this List<UserLayer> layers, PointF pt)
        {
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                var shape = layers[i].IsVisible(pt);
                if (shape != null)
                {
                    return shape;
                }
            }
            return null;
        }
        /// <summary>
        /// 判断指定矩形是否可以命中任何图形
        /// </summary>
        /// <param name="layers"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static List<AbstractShape> IsVisible(this List<UserLayer> layers, RectangleF rect)
        {
            var list = new List<AbstractShape>();
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                var shapes = layers[i].IsVisible(rect);
                if (shapes?.Count > 0)
                {
                    list.AddRange(shapes);
                }
            }
            return list;
        }
    }
}
