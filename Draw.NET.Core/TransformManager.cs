/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       TransformManager 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/19 13:55:46
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Renderer;

namespace Draw.NET.Core
{
    /// <summary>
    /// 对整体的平移、缩放管理，核心是计算转换矩阵
    /// 此矩阵的本质是将逻辑坐标系到屏幕坐标系的线性变换
    /// </summary>
    internal class TransformManager : IDisposable
    {
        private RenderConfig __config;
        private bool disposedValue = false;
        private PointF? beginMovePoint;

        internal System.Drawing.Drawing2D.Matrix Transform
        {
            get { return __config.TransformMatrix; }
        }


        public TransformManager(RenderConfig config)
        {
            __config = config;
        }

        public void Reset()
        {
            if (disposedValue)
                return;
            __config.TransformMatrix.Reset();
        }
        /// <summary>
        /// 开始平移调用
        /// </summary>
        /// <param name="pt"></param>
        public void BeginMove(PointF pt)
        {
            if (disposedValue)
                return;
            if (beginMovePoint == null)
            {

#if PerfMon
                System.Diagnostics.Debug.WriteLine($"Enter Moveing state: {pt}");
#endif
                beginMovePoint = pt;
            }
        }
        /// <summary>
        /// 平移中调用
        /// </summary>
        /// <param name="pt"></param>
        public void Move(PointF pt)
        {
            if (disposedValue)
                return;
            if (beginMovePoint.HasValue && beginMovePoint.Value != pt)
            {
                //#if PerfMon
                //                System.Diagnostics.Debug.WriteLine($"Moveing: {pt}");
                //#endif


                __config.TransformMatrix.Translate(
                    pt.X - beginMovePoint.Value.X,
                    pt.Y - beginMovePoint.Value.Y,
                    System.Drawing.Drawing2D.MatrixOrder.Append);

                beginMovePoint = pt;
#if PerfMon
                System.Diagnostics.Debug.WriteLine($"{beginMovePoint.Value.X - pt.X}, {beginMovePoint.Value.Y - pt.Y}");
#endif
            }
        }
        /// <summary>
        /// 直接移动转换矩阵到指定点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(float x, float y)
        {
            __config.TransformMatrix.Translate(x / __config.TransformMatrix.Elements[0], y / __config.TransformMatrix.Elements[0], System.Drawing.Drawing2D.MatrixOrder.Append);

        }
        /// <summary>
        /// 结束移动操作
        /// </summary>
        public void EndMove()
        {
            if (beginMovePoint != null)
            {

#if PerfMon
                System.Diagnostics.Debug.WriteLine($"Leave Moving state.");
#endif
                beginMovePoint = null;
            }
        }

        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="scalePoint">缩放点</param>
        /// <param name="scaleSize">缩放比例</param>
        public void Scale(PointF scalePoint, PointF scaleSize)
        {
            if (disposedValue)
                return;
            __config.TransformMatrix.Translate(scalePoint.X, scalePoint.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
            __config.TransformMatrix.Scale(scaleSize.X, scaleSize.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
            __config.TransformMatrix.Translate(-scalePoint.X, -scalePoint.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
        }



        public void Scale(PointF scalePoint, float v)
        {
            if (disposedValue)
                return;

            __config.TransformMatrix.Translate(-scalePoint.X, -scalePoint.Y, System.Drawing.Drawing2D.MatrixOrder.Append);
            __config.TransformMatrix.Scale(v, v, System.Drawing.Drawing2D.MatrixOrder.Append);
            __config.TransformMatrix.Translate(scalePoint.X, scalePoint.Y, System.Drawing.Drawing2D.MatrixOrder.Append);

        }


        public void Rotate(float angle, PointF pt = default(PointF))
        {
            if (disposedValue)
                return;
            if (pt != default(PointF))
            {
                __config.TransformMatrix.RotateAt(angle, pt);
            }
            else
            {
                __config.TransformMatrix.Rotate(angle);
            }
        }

        /// <summary>
        /// 针对屏幕点，反向求对应的逻辑坐标系中的点
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF GetActualPoint(PointF pt)
        {
            if (disposedValue)
                return Point.Empty;
            if (Transform.IsInvertible)
            {
                using (var clone = Transform.Clone())
                {
                    clone.Invert();
                    var pts = new PointF[] { pt };
                    clone.TransformPoints(pts);
                    return pts[0];
                }
            }
            return pt;
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                __config = null;
                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }


        #endregion


    }
}
